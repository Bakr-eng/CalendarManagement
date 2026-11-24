import type { Location } from "../types/location";

interface LocationDetailsProps {
  location: Location;
  onEdit: () => void;
  onDelete: () => void;
}

export function LocationDetails({ location, onEdit, onDelete }: LocationDetailsProps) {
  return (
    <>
      <dl className="details-list">
        <dt>Namn</dt>
        <dd>{location.name}</dd>

        <dt>Stad</dt>
        <dd>{location.city}</dd>

        <dt>Adress</dt>
        <dd>{location.address ?? "–"}</dd>
      </dl>

      <div className="modal-actions">
        <button type="button" className="modal-button modal-button-primary" onClick={onEdit}>
          Uppdatera
        </button>
        <button type="button" className="modal-button modal-button-danger" onClick={onDelete}>
          Ta bort
        </button>
      </div>
    </>
  );
}