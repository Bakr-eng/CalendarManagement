import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ChecklistSection from "../components/ChecklistSection";
import ActivityDetailsLayout from "../features/activities/components/ActivityDetailsLayout";

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
    <ActivityDetailsLayout>
      <div
        style={{
          margin: "0px auto",
          padding: "24px",
        }}
    >

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

        <ChecklistSection
  title="Dagsschema"
  activityId={activity.id}
/>

<ChecklistSection
  title="Hotell"
  activityId={activity.id}
/>

<ChecklistSection
  title="Resa"
  activityId={activity.id}
/>

<ChecklistSection
  title="Teknik"
  activityId={activity.id}
/>

<ChecklistSection
  title="Kontakt"
  activityId={activity.id}
/>

<ChecklistSection
  title="Plats"
  activityId={activity.id}
/>
      </div>
    </div>
    </ActivityDetailsLayout>
  );
}

export default ActivityDetailsPage;
