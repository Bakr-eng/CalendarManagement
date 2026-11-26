import { Link } from "react-router-dom";
import { useState, type ReactNode } from "react";
import "./ActivityDetailsLayout.css";

type Section =
  | "overview"
  | "schedule"
  | "hotel"
  | "travel"
  | "technical"
  | "contact"
  | "venue"
  | "attachments";

const sections: { id: Section; label: string }[] = [
  { id: "overview", label: "Översikt" },
  { id: "schedule", label: "Dagschema" },
  { id: "hotel", label: "Hotell" },
  { id: "travel", label: "Resa" },
  { id: "technical", label: "Teknik" },
  { id: "contact", label: "Kontakt" },
  { id: "venue", label: "Plats" },
  { id: "attachments", label: "Bilagor" },
];

type Props = {
  children: ReactNode;
  sectionStatuses: Record<string, string>;
};

export default function ActivityDetailsLayout({ children, sectionStatuses }: Props) {
  const [activeSection, setActiveSection] =
    useState<Section>("overview");

  const selected = sections.find(
    section => section.id === activeSection
  );
 
  const getStatusIcon = (status?: string) => {
  switch (status) {
    case "Complete":
      return "🟢";
    case "InProgress":
      return "🟠";
    default:
      return "🔴";
  }
};
  return (
  <>
    <header className="activity-topbar">
      <Link to="/calendar" className="activity-back">
        ← Tillbaka till kalendern
      </Link>
    </header>

    <div className="activity-layout">
      <aside className="activity-sidebar">
        <h2>Aktivitet</h2>

        <nav aria-label="Aktivitetsmeny">
          {sections.map(section => (
            <button
              key={section.id}
              type="button"
              className={
                activeSection === section.id
                  ? "activity-nav-item active"
                  : "activity-nav-item"
              }
              onClick={() => setActiveSection(section.id)}
            >
              {
  getStatusIcon(
    section.label === "Hotell"
      ? sectionStatuses["Hotell"]
      : section.label === "Dagschema"
      ? sectionStatuses["Dagsschema"]
      : section.label === "Resa"
      ? sectionStatuses["Resa"]
      : section.label === "Teknik"
      ? sectionStatuses["Teknik"]
      : section.label === "Kontakt"
      ? sectionStatuses["Kontakt"]
      : undefined
  )
}{" "}
{section.label}
            </button>
          ))}
        </nav>
      </aside>

      <main className="activity-main">
        {activeSection === "overview" ? (
          children
        ) : (
          <section className="activity-placeholder">
            <h1>{selected?.label}</h1>
            <p>
              Här kommer innehållet för {selected?.label.toLowerCase()}
              {" "}att visas.
            </p>
          </section>
        )}
      </main>
    </div>
    </>
  );
}
