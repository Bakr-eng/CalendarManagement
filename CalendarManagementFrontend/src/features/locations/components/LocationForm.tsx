import { useState, type SubmitEvent } from "react";
import type { CreateLocationRequest } from "../types/location";

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
    <form onSubmit={handleSubmit}>
      <h2>Ny plats</h2>

      <label>
        Namn *
        <input value={name} onChange={(e) => setName(e.target.value)} />
      </label>

      <label>
        Stad *
        <input value={city} onChange={(e) => setCity(e.target.value)} />
      </label>

      <label>
        Adress
        <input value={address} onChange={(e) => setAddress(e.target.value)} />
      </label>

      {error && <p role="alert">{error}</p>}

      <button type="submit" disabled={isSaving}>
        {isSaving ? "Sparar..." : "Spara plats"}
      </button>
    </form>
  );
}