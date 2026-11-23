import { useState, type SubmitEvent } from "react";
import type { CreateLocationRequest } from "../types/location";
import "./LocationForm.css";

interface LocationFormProps {
  onSubmit: (request: CreateLocationRequest) => Promise<void>;
}

export function LocationForm({ onSubmit }: LocationFormProps) {
  const [name, setName] = useState("");
  const [city, setCity] = useState("");
  const [address, setAddress] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [isSaving, setIsSaving] = useState(false);

    async function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!name.trim() || !city.trim()) {
      setError("Namn och stad måste fyllas i.");
      return;
    }

    setIsSaving(true);
    setError(null);

    try {
      await onSubmit({
        name,
        city,
        address: address || undefined,
      });

      setName("");
      setCity("");
      setAddress("");
    } catch (err) {
      setError(err instanceof Error ? err.message : "Något gick fel");
    } finally {
      setIsSaving(false);
    }
  }
  return (
    <form onSubmit={handleSubmit} className="location-form">
      <h2>Ny plats</h2>

      <div className="location-field">
        <label htmlFor="location-name">
          Namn <span className="location-required">*</span>
        </label>
        <input
          id="location-name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </div>

      <div className="location-field">
        <label htmlFor="location-city">
          Stad <span className="location-required">*</span>
        </label>
        <input
          id="location-city"
          value={city}
          onChange={(e) => setCity(e.target.value)}
        />
      </div>

      <div className="location-field">
        <label htmlFor="location-address">Adress</label>
        <input
          id="location-address"
          value={address}
          onChange={(e) => setAddress(e.target.value)}
        />
      </div>

      {error && (
        <p className="location-error" role="alert">
          {error}
        </p>
      )}

      <button type="submit" className="location-submit" disabled={isSaving}>
        {isSaving ? "Sparar..." : "Spara plats"}
      </button>
    </form>
  );
}