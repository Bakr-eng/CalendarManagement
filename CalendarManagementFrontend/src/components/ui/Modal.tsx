import { useEffect, useRef, type ReactNode } from "react";
import "./Modal.css";

interface ModalProps {
  title: string;
  onClose: () => void;
  children: ReactNode;
}

// Återanvändbar ruta som öppnas ovanpå sidan.
// Bygger på HTML-elementet <dialog>, som själv sköter Escape och fokus.
export function Modal({ title, onClose, children }: ModalProps) {
  const dialogRef = useRef<HTMLDialogElement>(null);

  useEffect(() => {
    dialogRef.current?.showModal();
  }, []);

  return (
    <dialog
      ref={dialogRef}
      className="modal"
      aria-labelledby="modal-title"
      onClose={onClose}
      onClick={(e) => {
        // Klick på den mörka bakgrunden (utanför rutan) stänger
        if (e.target === dialogRef.current) {
          onClose();
        }
      }}
    >
      <div className="modal-content">
        <div className="modal-header">
          <h2 id="modal-title">{title}</h2>
          <button type="button" className="modal-close" onClick={onClose} aria-label="Stäng">
            ×
          </button>
        </div>

        {children}
      </div>
    </dialog>
  );
}