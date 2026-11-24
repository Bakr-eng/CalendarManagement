type ChecklistSectionProps = {
  title: string;
};

export default function ChecklistSection({
  title,
}: ChecklistSectionProps) {
  return (
    <div
      style={{
        backgroundColor: "#111",
        borderRadius: "12px",
        padding: "20px",
        marginBottom: "20px",
        border: "1px solid #333",
      }}
    >
      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          marginBottom: "16px",
        }}
      >
        <h3>{title}</h3>

        <span
          style={{
            backgroundColor: "#dc3545",
            color: "white",
            padding: "4px 10px",
            borderRadius: "999px",
            fontSize: "12px",
            fontWeight: "bold",
          }}
        >
          Ej påbörjad
        </span>
      </div>

      <button
        style={{
          backgroundColor: "#d8a80a",
          color: "#000",
          border: "none",
          padding: "8px 14px",
          borderRadius: "8px",
          cursor: "pointer",
          fontWeight: "bold",
        }}
      >
        + Lägg till punkt
      </button>
    </div>
  );
}