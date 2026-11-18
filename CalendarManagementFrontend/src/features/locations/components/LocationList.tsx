import type { Location } from "../types/location";

interface LocationListProps {
  locations: Location[];
}

export function LocationList({ locations }: LocationListProps) {
  if (locations.length === 0) {
    return <p>Inga platser ännu.</p>;
  }

  return (
    <table>
      <thead>
        <tr>
          <th>Namn</th>
          <th>Stad</th>
          <th>Adress</th>
        </tr>
      </thead>
      <tbody>
        {locations.map((location) => (
          <tr key={location.id}>
            <td>{location.name}</td>
            <td>{location.city}</td>
            <td>{location.address ?? "–"}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}