import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/react/daygrid";
import interactionPlugin from "@fullcalendar/react/interaction";
import classicThemePlugin from "@fullcalendar/react/themes/classic";
import svLocale from "@fullcalendar/react/locales/sv";
import "@fullcalendar/react/skeleton.css";
import "@fullcalendar/react/themes/classic/theme.css";
import "@fullcalendar/react/themes/classic/palette.css";

function CalendarPage() {
  const events = [
    {
      title: "🎤 Gig | Joel | Stockholm",
      date: "2026-11-15",
    },
    {
      title: "🎸 Rep | Edin | Nyköping",
      date: "2026-11-18",
    },
    {
      title: "🚐 Resdag | Bakr | Göteborg",
      date: "2026-11-20",
    },
    {
      title: "🏨 Privat | Joel",
      date: "2026-11-24",
    },
    {
      title: "🎭 Pre Prod | Edin",
      date: "2026-11-28",
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
      />
    </div>
  );
}

export default CalendarPage;