import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/react/daygrid";
import interactionPlugin from "@fullcalendar/react/interaction";
import classicThemePlugin from "@fullcalendar/react/themes/classic";
import svLocale from "@fullcalendar/react/locales/sv";
import "@fullcalendar/react/skeleton.css";
import "@fullcalendar/react/themes/classic/theme.css";
import "@fullcalendar/react/themes/classic/palette.css";
import "./CalendarPage.css";

function CalendarPage() {
  const events = [
  {
    title: "🎤 Gig | Joel | Stockholm",
    date: "2026-11-15",
    color: "#3b82f6", // blå
  },
  {
    title: "🎸 Rep | Edin | Nyköping",
    date: "2026-11-18",
    color: "#22c55e", // grön
  },
  {
    title: "🚐 Resdag | Bakr | Göteborg",
    date: "2026-11-20",
    color: "#f59e0b", // orange
  },
  {
    title: "🏨 Privat | Joel",
    date: "2026-11-24",
    color: "#ef4444", // röd
  },
  {
    title: "🎭 Pre Prod | Edin",
    date: "2026-11-28",
    color: "#8b5cf6", // lila
  },
];

  return (
    <div style={{ padding: "20px" }}>
      <h1>Kalender</h1>

      <FullCalendar
        plugins={[dayGridPlugin, interactionPlugin, classicThemePlugin]}
        locale={svLocale}
        initialView="dayGridMonth"
        headerToolbar={{
          left: "prev,next today",
          center: "title",
          right: "dayGridMonth",
        }}
        events={events}
        height="auto"
        eventClick={(info) => {
          alert(info.event.title);
        }}
      />
    </div>
  );
}

export default CalendarPage;