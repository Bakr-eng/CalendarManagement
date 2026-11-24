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
import { ActivitySearch } from "../features/activities/components/ActivitySearch";
import { ActivitySearchResults } from "../features/activities/components/ActivitySearchResults";
import {
  formatActivityDetails,
  getActivities,
  type Activity,
  type ActivityFilter,
} from "../features/activities/services/activityService";

function CalendarPage() {
  const navigate = useNavigate();

  const [activities, setActivities] = useState<Activity[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [filter, setFilter] = useState<ActivityFilter>({});
  const [selectedDate, setSelectedDate] = useState<string | null>(null);
  // Ökas när en aktivitet har skapats, så att listan hämtas igen
  const [reloadKey, setReloadKey] = useState(0);

  const hasFilter =
    !!filter.search?.trim() || !!filter.customerId || !!filter.locationId || !!filter.responsiblePersonId;

  useEffect(() => {
    let ignore = false;

    // Vänta 300 ms efter senaste tangenttrycket innan vi söker,
    // så att vi inte skickar ett anrop för varje bokstav
    const timer = setTimeout(() => {
      getActivities(filter)
        .then((data) => {
          if (!ignore) {
            setActivities(data);
            setError(null);
          }
        })
        .catch((err: unknown) => {
          if (!ignore) setError(err instanceof Error ? err.message : "Något gick fel");
        });
    }, 300);

    return () => {
      ignore = true;
      clearTimeout(timer);
    };
  }, [filter, reloadKey]);

  const events = activities.map((activity) => ({
    id: activity.id.toString(),
    title: activity.type,
    date: activity.date,
    color: "#3b82f6",
    extendedProps: {
      details: formatActivityDetails(activity),
      responsiblePersonName: activity.responsiblePersonName,
    },
  }));

  return (
    <div style={{ padding: "20px" }}>
      <h1>Kalender</h1>

      <ActivitySearch filter={filter} onChange={setFilter} />

      {error && <p role="alert">{error}</p>}

      {hasFilter && (
        <ActivitySearchResults
          activities={activities}
          onSelect={(activity) => navigate(`/activities/${activity.id}`)}
        />
      )}

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
        eventContent={(arg) => {
          const details = arg.event.extendedProps.details as string;
          const person = arg.event.extendedProps.responsiblePersonName as string | null;

          return (
            <div className="calendar-event">
              <strong>{arg.event.title}</strong>
              {details && <span>{details}</span>}
              {person && <span className="calendar-event-person">{person}</span>}
            </div>
          );
        }}
        eventClick={(info) => {
          navigate(`/activities/${info.event.id}`);
        }}
        dateClick={(info) => {
          setSelectedDate(info.dateStr);
        }}
      />

      {selectedDate && (
        <div className="calendar-modal-overlay">
          <div className="calendar-modal">
            <CreateActivityForm
              selectedDate={selectedDate}
              onCancel={() => setSelectedDate(null)}
              onCreated={() => {
                setSelectedDate(null);
                setReloadKey((key) => key + 1);
              }}
            />
          </div>
        </div>
      )}
    </div>
  );
}

export default CalendarPage;