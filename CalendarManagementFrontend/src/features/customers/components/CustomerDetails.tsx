import type { Customer } from "../types/customer";

interface CustomerDetailsProps {
  customer: Customer;
  onEdit: () => void;
  onDelete: () => void;
}

export function CustomerDetails({ customer, onEdit, onDelete }: CustomerDetailsProps) {
  return (
    <>
      <dl className="details-list">
        <dt>Namn</dt>
        <dd>{customer.name}</dd>

        <dt>Kontaktperson</dt>
        <dd>{customer.contactPerson ?? "–"}</dd>

        <dt>E-post</dt>
        <dd>{customer.email ?? "–"}</dd>

        <dt>Telefon</dt>
        <dd>{customer.phone ?? "–"}</dd>
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