import type { Location } from "../types/location";
import "../../../styles/table.css";

interface LocationListProps {
  locations: Location[];
  onEdit: (location: Location) => void;
  onDelete: (location: Location) => void;
}

export function LocationList({ locations, onEdit, onDelete }: LocationListProps) {
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
            <th className="table-actions-header">Åtgärder</th>
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
              <td>
                <div className="table-actions">
                  <button
                    type="button"
                    className="table-button"
                    onClick={() => onEdit(location)}
                    aria-label={`Redigera ${location.name}`}
                  >
                    Redigera
                  </button>
                  <button
                    type="button"
                    className="table-button table-button-danger"
                    onClick={() => onDelete(location)}
                    aria-label={`Ta bort ${location.name}`}
                  >
                    Ta bort
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}