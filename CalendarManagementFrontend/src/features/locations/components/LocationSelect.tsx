import { useLocations } from "../hooks/useLocations";

interface LocationSelectProps {
  id?: string; // så att en <label htmlFor="..."> kan kopplas till listan
  value: number | null;
  onChange: (locationId: number | null) => void;
}

export function LocationSelect({ id, value, onChange }: LocationSelectProps) {
  const { locations, isLoading } = useLocations();

  return (
    <select
      id={id}
      value={value ?? ""}
      onChange={(e) => onChange(e.target.value ? Number(e.target.value) : null)}
      disabled={isLoading}
    >
      <option value="">{isLoading ? "Laddar..." : "Välj plats"}</option>
      {locations.map((location) => (
        <option key={location.id} value={location.id}>
          {location.name}, {location.city}
        </option>
      ))}
    </select>
  );
}