import { useEffect, useState } from "react";
import { CustomerSelect } from "../../customers/components/CustomerSelect";
import { LocationSelect } from "../../locations/components/LocationSelect";
import type { ActivityFilter } from "../services/activityService";
import { getUsers, type User } from "../services/userService";
import "./activities.css";

interface ActivitySearchProps {
  filter: ActivityFilter;
  onChange: (filter: ActivityFilter) => void;
}

export function ActivitySearch({ filter, onChange }: ActivitySearchProps) {
  const [users, setUsers] = useState<User[]>([]);

  useEffect(() => {
    let ignore = false;

    getUsers()
      .then((data) => {
        if (!ignore) setUsers(data);
      })
      .catch(() => {
        // Utan personer går det ändå att söka på resten
      });

    return () => {
      ignore = true;
    };
  }, []);

  const hasFilter =
    !!filter.search || !!filter.customerId || !!filter.locationId || !!filter.responsiblePersonId;

  return (
    <div className="activity-search" role="search">
      <div className="activity-search-field activity-search-text">
        <label htmlFor="activity-search-input">Sök</label>
        <input
          id="activity-search-input"
          type="search"
          placeholder="Kund, plats, stad, ansvarig eller typ"
          value={filter.search ?? ""}
          onChange={(e) => onChange({ ...filter, search: e.target.value })}
        />
      </div>

      <div className="activity-search-field">
        <label htmlFor="activity-filter-customer">Kund</label>
        <CustomerSelect
          id="activity-filter-customer"
          value={filter.customerId ?? null}
          onChange={(customerId) => onChange({ ...filter, customerId })}
        />
      </div>

      <div className="activity-search-field">
        <label htmlFor="activity-filter-location">Plats</label>
        <LocationSelect
          id="activity-filter-location"
          value={filter.locationId ?? null}
          onChange={(locationId) => onChange({ ...filter, locationId })}
        />
      </div>

      <div className="activity-search-field">
        <label htmlFor="activity-filter-person">Ansvarig</label>
        <select
          id="activity-filter-person"
          value={filter.responsiblePersonId ?? ""}
          onChange={(e) =>
            onChange({
              ...filter,
              responsiblePersonId: e.target.value ? Number(e.target.value) : null,
            })
          }
        >
          <option value="">Alla</option>
          {users.map((user) => (
            <option key={user.id} value={user.id}>
              {user.name}
            </option>
          ))}
        </select>
      </div>

      {hasFilter && (
        <button type="button" className="activity-search-clear" onClick={() => onChange({})}>
          Rensa
        </button>
      )}
    </div>
  );
}