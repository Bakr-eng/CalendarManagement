
import { useState, type SubmitEvent } from "react";
import { createActivity } from "../services/activityService";
import "./CreateActivityForm.css";

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

  async function handleSubmit(e: SubmitEvent<HTMLFormElement>) {
    e.preventDefault();

    setLoading(true);
    setError("");

    try {
      await createActivity({
        date: `${date}T00:00:00`,
        type,
      });

      onCreated?.();
    } catch {
      setError("Could not create activity.");
    } finally {
      setLoading(false);
    }
  }

  return (
  <form
    onSubmit={handleSubmit}
    className="create-activity-form"
  >
    <h2>Create Activity</h2>

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
      <select
        id="type"
        value={type}
        onChange={(e) => setType(e.target.value)}
      >
        {activityTypes.map((activityType) => (
          <option key={activityType} value={activityType}>
            {activityType}
          </option>
        ))}
      </select>
    </div>

    {error && (
      <p className="activity-error" role="alert">
        {error}
      </p>
    )}

    <div className="activity-actions">
      <button
        type="button"
        className="activity-cancel"
        onClick={onCancel}
      >
        Cancel
      </button>

      <button
        type="submit"
        className="activity-submit"
        disabled={loading}
      >
        {loading ? "Creating..." : "Create Activity"}
      </button>
    </div>
  </form>
);

}
