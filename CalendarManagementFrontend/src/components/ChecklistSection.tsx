import { useState } from "react";

type ChecklistSectionProps = {
  title: string;
};

export default function ChecklistSection({
  title,
}: ChecklistSectionProps) {
  const [items, setItems] = useState<string[]>([]);
  const [newItem, setNewItem] = useState("");

  const addItem = () => {
    if (!newItem.trim()) return;

    setItems([...items, newItem]);
    setNewItem("");
  };

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
      <h3>{title}</h3>

      <div
        style={{
          display: "flex",
          gap: "10px",
          marginBottom: "16px",
        }}
      >
        <input
          type="text"
          value={newItem}
          onChange={(e) => setNewItem(e.target.value)}
          placeholder="Ny punkt..."
          style={{
            flex: 1,
            padding: "10px",
            borderRadius: "8px",
            border: "1px solid #555",
          }}
        />

        <button
          onClick={addItem}
          style={{
            backgroundColor: "#d8a80a",
            color: "#000",
            border: "none",
            padding: "10px 16px",
            borderRadius: "8px",
            cursor: "pointer",
            fontWeight: "bold",
          }}
        >
          Lägg till
        </button>
      </div>

      <ul
        style={{
          listStyle: "none",
          padding: 0,
          margin: 0,
        }}
      >
        {items.map((item, index) => (
          <li
            key={index}
            style={{
              padding: "8px 0",
              borderBottom: "1px solid #222",
            }}
          >
            {item}
          </li>
        ))}
      </ul>
    </div>
  );
}