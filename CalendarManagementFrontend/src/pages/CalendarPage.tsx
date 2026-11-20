import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/react/daygrid";
import interactionPlugin from "@fullcalendar/react/interaction";
import classicThemePlugin from "@fullcalendar/react/themes/classic";
import svLocale from "@fullcalendar/react/locales/sv";

import "@fullcalendar/react/skeleton.css";
import "@fullcalendar/react/themes/classic/theme.css";
import "@fullcalendar/react/themes/classic/palette.css";

import "./CalendarPage.css";
import CreateActivityForm from "../features/activities/components/CreateActivityForm";

type CalendarEvent = {
  id: string;
  title: string;
  date: string;
  color?: string;
  responsiblePersonName: string;
};

type Activity = {
  id: number;
  date: string;
  type: string;
  responsiblePersonId: number | null;
  responsiblePerson: {
    id: number;
    name: string;
  } | null;
};

function CalendarPage() {
  const navigate = useNavigate();

  const [events, setEvents] = useState<CalendarEvent[]>([]);
  const [selectedDate, setSelectedDate] = useState<string | null>(null);

  useEffect(() => {
    fetch("/api/activities")
      .then((res) => {
        console.log("Status:", res.status);

        if (!res.ok) {
          throw new Error(`HTTP ${res.status}`);
        }

        return res.json();
      })
      .then((activities: Activity[]) => {
        console.log("Activities:", activities);

        const calendarEvents: CalendarEvent[] = activities.map(
          (activity) => ({
            id: activity.id.toString(),
            title: activity.type,
            date: activity.date,
            color: "#3b82f6",
            responsiblePersonName:
            activity.responsiblePerson?.name ?? "Not assigned",
          })
        );

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
        displayEventTime={false}
        headerToolbar={{
          left: "prev,next today",
          center: "title",
          right: "dayGridMonth",
        }}
        events={events}
        height="auto"
        eventClick={(info) => {
          navigate(`/activities/${info.event.id}`);
        }}
        dateClick={(info) => {
          setSelectedDate(info.dateStr);
        }}
      />

      {selectedDate && (
        <div className="modal-overlay">
          <div className="modal">
            <CreateActivityForm
              selectedDate={selectedDate}
              onCancel={() => setSelectedDate(null)}
              onCreated={() => {
                setSelectedDate(null);
                window.location.reload();
              }}
            />
          </div>
        </div>
      )}
    </div>
  );
}

export default CalendarPage;