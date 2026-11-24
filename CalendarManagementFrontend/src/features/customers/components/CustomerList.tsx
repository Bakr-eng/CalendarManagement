import type { Customer } from "../types/customer";
import "../../../styles/table.css";

interface CustomerListProps {
  customers: Customer[];
  onEdit: (customer: Customer) => void;
  onDelete: (customer: Customer) => void;
}

export function CustomerList({ customers, onEdit, onDelete }: CustomerListProps) {
  if (customers.length === 0) {
    return <p>Inga kunder ännu.</p>;
  }

  return (
    <div className="table-wrapper">
      <table className="data-table">
        <thead>
          <tr>
            <th>Namn</th>
            <th>Kontaktperson</th>
            <th>E-post</th>
            <th>Telefon</th>
            <th className="table-actions-header">Åtgärder</th>
          </tr>
        </thead>
        <tbody>
          {customers.map((customer) => (
            <tr key={customer.id}>
              <td>{customer.name}</td>
              <td className={customer.contactPerson ? undefined : "cell-empty"}>
                {customer.contactPerson ?? "–"}
              </td>
              <td className={customer.email ? undefined : "cell-empty"}>
                {customer.email ?? "–"}
              </td>
              <td className={customer.phone ? undefined : "cell-empty"}>
                {customer.phone ?? "–"}
              </td>
              <td>
                <div className="table-actions">
                  <button
                    type="button"
                    className="table-button"
                    onClick={() => onEdit(customer)}
                    aria-label={`Redigera ${customer.name}`}
                  >
                    Redigera
                  </button>
                  <button
                    type="button"
                    className="table-button table-button-danger"
                    onClick={() => onDelete(customer)}
                    aria-label={`Ta bort ${customer.name}`}
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