import type { Customer } from "../types/customer";
import "../../../styles/table.css";

interface CustomerListProps {
  customers: Customer[];
  onSelect: (customer: Customer) => void;
}

export function CustomerList({ customers, onSelect }: CustomerListProps) {
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
          </tr>
        </thead>
        <tbody>
          {customers.map((customer) => (
            <tr key={customer.id} className="clickable-row" onClick={() => onSelect(customer)}>
              <td>
                <button type="button" className="row-button">
                  {customer.name}
                </button>
              </td>
              <td className={customer.contactPerson ? undefined : "cell-empty"}>
                {customer.contactPerson ?? "–"}
              </td>
              <td className={customer.email ? undefined : "cell-empty"}>
                {customer.email ?? "–"}
              </td>
              <td className={customer.phone ? undefined : "cell-empty"}>
                {customer.phone ?? "–"}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}