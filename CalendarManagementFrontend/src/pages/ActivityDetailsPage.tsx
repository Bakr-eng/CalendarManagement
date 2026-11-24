import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import ChecklistSection from "../components/ChecklistSection";

type Activity = {
  id: number;
  date: string;
  type: string;

  responsiblePersonId: number | null;
  responsiblePersonName: string | null;

  customerName: string | null;

  locationName: string | null;
  locationCity: string | null;
};

function ActivityDetailsPage() {
  const { id } = useParams();
  const [activity, setActivity] = useState<Activity | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetch(`/api/activities/${id}`)
      .then((res) => {
        if (!res.ok) {
          throw new Error("Failed to fetch activity");
        }

        return res.json();
      })
      .then((data) => {
        setActivity(data);
      })
      .catch((error) => {
        console.error(error);
      });
  }, [id]);

  if (!activity) {
    return (
      <div style={{ padding: "20px", color: "yellow" }}>
        <p>Laddar aktivitet...</p>
      </div>
    );
  }

  return (
    <div
      style={{
        maxWidth: "800px",
        margin: "40px auto",
        padding: "24px",
      }}
    >
      <button
        onClick={() => navigate("/calendar")}
        style={{
          marginBottom: "20px",
          backgroundColor: "#d8a80a",
          color: "#080000",
          border: "none",
          padding: "10px 18px",
          borderRadius: "8px",
          cursor: "pointer",
          fontWeight: "bold",
        }}
      >
        ← Tillbaka till kalendern
      </button>

      <h1>Aktivitetsdetaljer</h1>

      <div
        style={{
          backgroundColor: "#080000",
          color: "#d8a80a",
          padding: "24px",
          borderRadius: "12px",
          boxShadow: "0 4px 12px rgba(236, 195, 13, 0.1)",
        }}
      >
        <h2>Aktivitet #{activity.id}</h2>

        <div style={{ marginTop: "20px" }}>
          <p>
            <strong>Typ:</strong> {activity.type}
          </p>

          <p>
            <strong>Datum:</strong>{" "}
            {new Date(activity.date).toLocaleDateString("sv-SE")}
          </p>

          <p>
            <strong>Status:</strong> Planerad
          </p>

          <p>
            <strong>Ansvarig person:</strong>{" "}
            {activity.responsiblePersonName ?? "Ingen tilldelad"}
          </p>

          <p>
            <strong>Kund:</strong> {activity.customerName ?? "Ingen kund"}
          </p>

          <p>
            <strong>Plats:</strong>{" "}
            {activity.locationName
              ? `${activity.locationName}, ${activity.locationCity}`
              : "Ingen plats"}
          </p>
        </div>
      </div>
    
      <div
        style={{
          backgroundColor: "#080000",
          color: "#d8a80a",
          padding: "24px",
          borderRadius: "12px",
          boxShadow: "0 4px 12px rgba(236, 195, 13, 0.1)",
        }}
      >
        <h2>Checklista</h2>

        <ChecklistSection title="Dagsschema" />
        <ChecklistSection title="Hotell" />
        <ChecklistSection title="Resa" />
        <ChecklistSection title="Teknik" />
        <ChecklistSection title="Kontakt" />
        <ChecklistSection title="Plats" />
      </div>
    </div>
  );
}

export default ActivityDetailsPage;
