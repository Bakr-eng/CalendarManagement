import { useState, type SubmitEvent } from "react";
import type { CreateLocationRequest, Location } from "../types/location";
import "./LocationForm.css";

interface LocationFormProps {
  onSubmit: (request: CreateLocationRequest) => Promise<void>;
  initialLocation?: Location; // finns = redigera, saknas = ny plats
  onCancel?: () => void;
}

export function LocationForm({ onSubmit, initialLocation, onCancel }: LocationFormProps) {
  const isEditing = initialLocation !== undefined;

  const [name, setName] = useState(initialLocation?.name ?? "");
  const [city, setCity] = useState(initialLocation?.city ?? "");
  const [address, setAddress] = useState(initialLocation?.address ?? "");
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

      // Töm bara fälten när man skapar en ny plats.
      // Vid redigering stänger sidan formuläret istället.
      if (!isEditing) {
        setName("");
        setCity("");
        setAddress("");
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Något gick fel");
    } finally {
      setIsSaving(false);
    }
  }

  return (
    <form onSubmit={handleSubmit} className="location-form">
      <h2>{isEditing ? "Redigera plats" : "Ny plats"}</h2>

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

      <div className="location-actions">
        <button type="submit" className="location-submit" disabled={isSaving}>
          {isSaving ? "Sparar..." : isEditing ? "Spara ändringar" : "Spara plats"}
        </button>

        {onCancel && (
          <button type="button" className="location-cancel" onClick={onCancel}>
            Avbryt
          </button>
        )}
      </div>
    </form>
  );
}