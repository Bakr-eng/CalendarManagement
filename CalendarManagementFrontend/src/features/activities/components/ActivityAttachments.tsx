import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import "./ActivityAttachments.css";

type Attachment = {
  id: number;
  activityId: number;
  fileName: string;
  contentType: string;
  fileSize: number;
  uploadedAtUtc: string;
};

export default function ActivityAttachments() {
  const { id } = useParams();
  const activityId = Number(id);

  const [files, setFiles] = useState<Attachment[]>([]);
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState("");
  const baseUrl = `/api/activities/${activityId}/attachments`;
  const [previewFile, setPreviewFile] = useState<Attachment | null>(null);
  const [dragging, setDragging] = useState(false);

function isImage(file: Attachment) {
    return [
      "image/png",
      "image/jpeg",
      "image/webp",
      "image/gif"
    ].includes(file.contentType);
  }

  function canPreview(file: Attachment) {
    return isImage(file) ||
      file.contentType === "application/pdf";
  }

  async function loadFiles() {
    const response = await fetch(baseUrl);

    if (!response.ok) {
      throw new Error("Kunde inte hämta bilagor.");
    }

    const data: Attachment[] = await response.json();
    setFiles(data);
  }

  
useEffect(() => {
  if (!Number.isInteger(activityId) || activityId <= 0) {
    return;
  }

  let cancelled = false;

  async function fetchAttachments() {
    try {
      const response = await fetch(
        `/api/activities/${activityId}/attachments`
      );

      if (!response.ok) {
        throw new Error("Kunde inte hämta bilagor.");
      }

      const data: Attachment[] = await response.json();

      if (!cancelled) {
        setFiles(data);
      }
    } catch (err) {
      if (!cancelled) {
        setError(
          err instanceof Error
            ? err.message
            : "Kunde inte hämta bilagor."
        );
      }
    }
  }

  void fetchAttachments();

  return () => {
    cancelled = true;
  };
}, [activityId]);


  async function uploadFiles(selected: FileList | null) {
    if (!selected?.length || uploading) return;

    const items = Array.from(selected);

    if (items.length > 20) {
      setError("Välj högst 20 filer åt gången.");
      return;
    }

    setUploading(true);
    setError("");

    try {
      for (const file of items) {
        if (file.size > 100 * 1024 * 1024) {
          throw new Error(`${file.name} är större än 100 MB.`);
        }

        const form = new FormData();
        form.append("file", file);

        const tokenResponse = await fetch("/api/antiforgery/token", {
  credentials: "same-origin",
  cache: "no-store"
});

if (!tokenResponse.ok) {
  throw new Error("Kunde inte hämta säkerhetstoken.");
}

const { token } = await tokenResponse.json();

const response = await fetch(baseUrl, {
  method: "POST",
  headers: {
    "X-CSRF-TOKEN": token
  },
  credentials: "same-origin",
  body: form
});

        if (!response.ok) {
          throw new Error(`Kunde inte ladda upp ${file.name}.`);
        }
      }

      await loadFiles();
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Ett fel uppstod."
      );

      // Visa även filer som hann laddas upp.
      await loadFiles().catch(() => {});
    } finally {
      setUploading(false);
    }
  }

  async function deleteFile(attachment: Attachment) {
    if (!window.confirm(`Ta bort ${attachment.fileName}?`)) {
      return;
    }

    try {
      const response = await fetch(
        `${baseUrl}/${attachment.id}`,
        { method: "DELETE" }
      );

      if (!response.ok) {
        throw new Error("Kunde inte ta bort filen.");
      }

      await loadFiles();
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Ett fel uppstod."
      );
    }
  }

  function formatSize(bytes: number) {
    return bytes >= 1024 * 1024
      ? `${(bytes / (1024 * 1024)).toFixed(1)} MB`
      : `${(bytes / 1024).toFixed(0)} KB`;
  }

  useEffect(() => {
    if (!previewFile) return;

    function handleKeyDown(event: KeyboardEvent) {
      if (event.key === "Escape") {
        setPreviewFile(null);
      }
    }

    window.addEventListener("keydown", handleKeyDown);

    return () => {
      window.removeEventListener("keydown", handleKeyDown);
    };
  }, [previewFile]);


  return (
    <section className="attachment-section">
      <div className="attachment-heading">
        <div>
          <h1>Bilagor</h1>
          <p>Dokument och filer kopplade till aktiviteten.</p>
        </div>

        <span>{files.length} filer</span>
      </div>

      
<div
  className={`attachment-dropzone ${dragging ? "dragging" : ""}`}
  onDragEnter={e => {
    e.preventDefault();
    setDragging(true);
  }}
  onDragOver={e => {
    e.preventDefault();
    e.dataTransfer.dropEffect = "copy";
  }}
  onDragLeave={e => {
    e.preventDefault();

    if (!e.currentTarget.contains(e.relatedTarget as Node | null)) {
      setDragging(false);
    }
  }}
  onDrop={e => {
    e.preventDefault();
    setDragging(false);

    void uploadFiles(e.dataTransfer.files);
  }}
>
  <span className="attachment-dropzone-icon">☁</span>

  <h3>Dra och släpp filer här</h3>

  <p>PDF, dokument, bilder och andra tillåtna filer</p>

  <span className="attachment-dropzone-separator">
    eller
  </span>

    <label className="attachment-upload">
      {uploading ? "Laddar upp..." : "+ Välj filer"}

      <input
        type="file"
        multiple
        disabled={uploading}
        accept=".pdf,.docx,.xlsx,.pptx,.txt,.csv,.jpg,.jpeg,.png,.webp,.gif"
        onChange={e => {
          void uploadFiles(e.target.files);
          e.target.value = "";
        }}
      />
    </label>
  </div>

      {error && <p className="attachment-error">{error}</p>}

      {files.length === 0 && !error && (
        <p className="attachment-empty">
          Inga filer har lagts till ännu.
        </p>
      )}

      <div className="attachment-list">
        {files.map(file => (
          <div className="attachment-item" key={file.id}>
            {canPreview(file) ? (
  <button
    type="button"
    className="attachment-preview-trigger"
    onClick={() => setPreviewFile(file)}
    title={`Visa ${file.fileName}`}
  >
    {isImage(file) ? (
      <img
        src={`${baseUrl}/${file.id}/preview`}
        alt=""
        className="attachment-thumbnail"
        loading="lazy"
      />
    ) : (
      <span>📕</span>
    )}
  </button>
) : (
  <span className="attachment-file-icon">📄</span>
)}

            <div className="attachment-info">
              {canPreview(file) ? (
                <button
                  type="button"
                  className="attachment-filename-button"
                  onClick={() => setPreviewFile(file)}
                >
                  {file.fileName}
                </button>
              ) : (
                <strong>{file.fileName}</strong>
              )}
              <small>{formatSize(file.fileSize)}</small>
            </div>

            <a
              href={`${baseUrl}/${file.id}/download`}
              className="attachment-download"
            >
              Ladda ned
            </a>

            <button
              type="button"
              onClick={() => void deleteFile(file)}
              className="attachment-delete"
            >
              Ta bort
            </button>
          </div>
        ))}
      </div>

      {previewFile && (
        <div
          className="attachment-modal-backdrop"
          onMouseDown={e => {
            if (e.target === e.currentTarget) {
              setPreviewFile(null);
            }
          }}
        >
          <div
            className="attachment-modal"
            role="dialog"
            aria-modal="true"
            aria-label={`Förhandsgranska ${previewFile.fileName}`}
          >
            <div className="attachment-modal-header">
              <strong>{previewFile.fileName}</strong>

              <button
                type="button"
                onClick={() => setPreviewFile(null)}
                aria-label="Stäng förhandsgranskning"
              >
                ✕
              </button>
            </div>

            {isImage(previewFile) ? (
              <img
                src={`${baseUrl}/${previewFile.id}/preview`}
                alt={previewFile.fileName}
                className="attachment-large-image"
              />
            ) : (
              <iframe
                src={`${baseUrl}/${previewFile.id}/preview`}
                title={previewFile.fileName}
                className="attachment-pdf-preview"
              />
            )}
          </div>
        </div>
      )}
    </section>
  );
}
