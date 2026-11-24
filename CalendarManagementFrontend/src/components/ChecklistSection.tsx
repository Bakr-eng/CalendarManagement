import { useState } from "react";

type ChecklistItem = {
  id: number;
  text: string;
  completed: boolean;
};

type ChecklistSectionProps = {
  title: string;
};

export default function ChecklistSection({
  title,
}: ChecklistSectionProps) {
  const [items, setItems] = useState<ChecklistItem[]>([]);
  const [newItem, setNewItem] = useState("");

  const addItem = () => {
    if (!newItem.trim()) return;

    setItems([
      ...items,
      {
        id: Date.now(),
        text: newItem,
        completed: false,
      },
    ]);

    setNewItem("");
  };

  const toggleItem = (id: number) => {
    setItems(
      items.map((item) =>
        item.id === id
          ? { ...item, completed: !item.completed }
          : item
      )
    );
  };

  const deleteItem = (id: number) => {
    setItems(items.filter((item) => item.id !== id));
  };

  const completedCount = items.filter(
    (item) => item.completed
  ).length;

  const progressText = `${completedCount}/${items.length}`;

  const getStatus = () => {
    if (items.length === 0) {
      return {
        text: "Ej påbörjad",
        color: "#dc3545",
      };
    }

    if (completedCount === items.length) {
      return {
        text: "Klar",
        color: "#28a745",
      };
    }

    return {
      text: "Pågående",
      color: "#ffc107",
    };
  };

  const status = getStatus();

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
          marginBottom: "8px",
        }}
      >
        <h3 style={{ margin: 0 }}>{title}</h3>

        <span
          style={{
            backgroundColor: status.color,
            color:
              status.text === "Pågående"
                ? "#000"
                : "#fff",
            padding: "4px 10px",
            borderRadius: "999px",
            fontSize: "12px",
            fontWeight: "bold",
          }}
        >
          {status.text}
        </span>
      </div>

      <div
        style={{
          fontSize: "12px",
          color: "#999",
          marginBottom: "16px",
        }}
      >
        Klara punkter: {progressText}
      </div>

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
            padding: "8px 14px",
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
        {items.map((item) => (
          <li
            key={item.id}
            style={{
              display: "flex",
              alignItems: "center",
              gap: "10px",
              padding: "8px 0",
              borderBottom: "1px solid #222",
            }}
          >
            <input
              type="checkbox"
              checked={item.completed}
              onChange={() => toggleItem(item.id)}
            />

            <span
              style={{
                flex: 1,
                textDecoration: item.completed
                  ? "line-through"
                  : "none",
                color: item.completed
                  ? "#888"
                  : "#fff",
              }}
            >
              {item.text}
            </span>

            <button
              onClick={() => deleteItem(item.id)}
              title="Ta bort punkt"
              style={{
                background: "transparent",
                border: "none",
                cursor: "pointer",
                fontSize: "18px",
                color: "#dc3545",
                padding: "4px",
              }}
            >
              🗑️
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}