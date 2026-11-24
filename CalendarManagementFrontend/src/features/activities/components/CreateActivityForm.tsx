import { useEffect, useState, type SubmitEvent } from "react";
import { getUsers, type User } from "../services/userService";
import { createActivity } from "../services/activityService";
import { CustomerSelect } from "../../customers/components/CustomerSelect";
import { LocationSelect } from "../../locations/components/LocationSelect";
import "./activities.css";

type Props = {
  selectedDate?: string;
  onCreated?: () => void;
  onCancel?: () => void;
};

const activityTypes = [
  "Gig",
  "Private",
  "Rehearsal",
  "Vacation",
  "Travel Day",
  "Pre-production",
  "Individual Work",
];

export default function CreateActivityForm({
  selectedDate,
  onCreated,
  onCancel,
}: Props) {
  const [date, setDate] = useState(selectedDate ?? "");
  const [type, setType] = useState("Gig");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [users, setUsers] = useState<User[]>([]);
  const [usersLoading, setUsersLoading] = useState(true);
  const [usersError, setUsersError] = useState("");
  const [selectedUserId, setSelectedUserId] = useState("");
  const [customerId, setCustomerId] = useState<number | null>(null);
  const [locationId, setLocationId] = useState<number | null>(null);

  useEffect(() => {
    let active = true;

    async function loadUsers() {
      try {
        const data = await getUsers();

        if (active) {
          setUsers(data);
        }
      } catch {
        if (active) {
          setUsersError("Could not load users.");
        }
      } finally {
        if (active) {
          setUsersLoading(false);
        }
      }
    }

    loadUsers();

    return () => {
      active = false;
    };
  }, []);

  async function handleSubmit(e: SubmitEvent<HTMLFormElement>) {
    e.preventDefault();

    setLoading(true);
    setError("");

    try {
      await createActivity({
        date: `${date}T00:00:00`,
        type,
        responsiblePersonId: selectedUserId ? Number(selectedUserId) : null,
        customerId,
        locationId,
      });

      onCreated?.();
    } catch {
      setError("Could not create activity.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <form onSubmit={handleSubmit} className="create-activity-form">
      <h2>Create Activity</h2>

      {usersLoading && <p>Loading users...</p>}

      {usersError && (
        <p className="activity-error" role="alert">
          {usersError}
        </p>
      )}

      <div className="activity-field">
        <label htmlFor="date">Date</label>
        <input
          id="date"
          type="date"
          value={date}
          onChange={(e) => setDate(e.target.value)}
          required
        />
      </div>

      <div className="activity-field">
        <label htmlFor="type">Activity type</label>
        <select id="type" value={type} onChange={(e) => setType(e.target.value)}>
          {activityTypes.map((activityType) => (
            <option key={activityType} value={activityType}>
              {activityType}
            </option>
          ))}
        </select>
      </div>

      <div className="activity-field">
        <label htmlFor="responsiblePerson">Responsible person</label>
        <select
          id="responsiblePerson"
          value={selectedUserId}
          onChange={(e) => setSelectedUserId(e.target.value)}
          disabled={usersLoading || !!usersError}
        >
          <option value="">Not assigned</option>
          {users.map((user) => (
            <option key={user.id} value={user.id}>
              {user.name}
            </option>
          ))}
        </select>
      </div>

      <div className="activity-field">
        <label htmlFor="activity-customer">Kund</label>
        <CustomerSelect id="activity-customer" value={customerId} onChange={setCustomerId} />
      </div>

      <div className="activity-field">
        <label htmlFor="activity-location">Plats</label>
        <LocationSelect id="activity-location" value={locationId} onChange={setLocationId} />
      </div>

      {error && (
        <p className="activity-error" role="alert">
          {error}
        </p>
      )}

      <div className="activity-actions">
        <button type="button" className="activity-cancel" onClick={onCancel}>
          Cancel
        </button>

        <button type="submit" className="activity-submit" disabled={loading}>
          {loading ? "Creating..." : "Create Activity"}
        </button>
      </div>
    </form>
  );
}