// ritar listan "2 aktiviteter" med datum, typ, plats och kund
import { formatActivityDetails, type Activity } from "../services/activityService";
import "./activities.css";

interface ActivitySearchResultsProps {
  activities: Activity[];
  onSelect: (activity: Activity) => void;
}

// Listan som visas när man söker – så att man ser träffar i alla månader,
// inte bara den månad som kalendern visar
export function ActivitySearchResults({ activities, onSelect }: ActivitySearchResultsProps) {
  return (
    <section className="search-results" aria-live="polite">
      <h2>
        {activities.length === 0
          ? "Inga aktiviteter hittades"
          : `${activities.length} ${activities.length === 1 ? "aktivitet" : "aktiviteter"}`}
      </h2>

      {activities.length > 0 && (
        <ul>
          {activities.map((activity) => (
            <li key={activity.id}>
              <button type="button" onClick={() => onSelect(activity)}>
                <span className="search-result-date">
                  {new Date(activity.date).toLocaleDateString("sv-SE", {
                    weekday: "short",
                    day: "numeric",
                    month: "short",
                    year: "numeric",
                  })}
                </span>
                <span className="search-result-type">{activity.type}</span>
                <span className="search-result-details">
                  {formatActivityDetails(activity) || "–"}
                </span>
                <span className="search-result-person">
                  {activity.responsiblePersonName ?? ""}
                </span>
              </button>
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}