import { useEffect, useState } from "react";

import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/react/daygrid";
import interactionPlugin from "@fullcalendar/react/interaction";
import classicThemePlugin from "@fullcalendar/react/themes/classic";
import svLocale from "@fullcalendar/react/locales/sv";

import "@fullcalendar/react/skeleton.css";
import "@fullcalendar/react/themes/classic/theme.css";
import "@fullcalendar/react/themes/classic/palette.css";

import "./CalendarPage.css";

type CalendarEvent = {
  title: string;
  date: string;
  color?: string;
};

function CalendarPage() {
  const [selectedEvent, setSelectedEvent] = useState<string | null>(null);
  const [events, setEvents] = useState<CalendarEvent[]>([]);

  useEffect(() => {
    fetch("http://localhost:5196/api/activities")
      .then((res) => {
        console.log("Status:", res.status);

        if (!res.ok) {
          throw new Error(`HTTP ${res.status}`);
        }

        return res.json();
      })
      .then((activities) => {
        console.log("Activities:", activities);

        const calendarEvents = activities.map((activity: any) => ({
          title: activity.type,
          date: activity.date,
          color: "#3b82f6",
        }));

        setEvents(calendarEvents);
      })
      .catch((error) => {
        console.error("Failed to fetch activities:", error);
      });
  }, []);

  return (
    <div style={{ padding: "20px" }}>
      <h1>Kalender</h1>

      <FullCalendar
        plugins={[
          dayGridPlugin,
          interactionPlugin,
          classicThemePlugin,
        ]}
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
          setSelectedEvent(info.event.title);
        }}
      />

      {selectedEvent && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>Aktivitet</h2>

            <p>{selectedEvent}</p>

            <button onClick={() => setSelectedEvent(null)}>
              Stäng
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

export default CalendarPage;
