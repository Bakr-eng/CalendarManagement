import type { Location } from "../types/location";
import "../../../styles/table.css";

interface LocationListProps {
  locations: Location[];
}

export function LocationList({ locations }: LocationListProps) {
  if (locations.length === 0) {
    return <p>Inga platser ännu.</p>;
  }

  return (
    <div className="table-wrapper">
      <table className="data-table">
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
              <td className={location.address ? undefined : "cell-empty"}>
                {location.address ?? "–"}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}