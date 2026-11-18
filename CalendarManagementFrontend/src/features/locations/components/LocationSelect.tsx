import { useLocations } from "../hooks/useLocations";

interface LocationSelectProps {
  value: number | null;
  onChange: (locationId: number | null) => void;
}

export function LocationSelect({ value, onChange }: LocationSelectProps) {
  const { locations, isLoading } = useLocations();

  return (
    <select
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