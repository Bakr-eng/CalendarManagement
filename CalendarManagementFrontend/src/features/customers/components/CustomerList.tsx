import type { Customer } from "../types/customer";

interface CustomerListProps {
  customers: Customer[];
}

export function CustomerList({ customers }: CustomerListProps) {
  if (customers.length === 0) {
    return <p>Inga kunder ännu.</p>;
  }

  return (
    <table>
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
          <tr key={customer.id}>
            <td>{customer.name}</td>
            <td>{customer.contactPerson ?? "–"}</td>
            <td>{customer.email ?? "–"}</td>
            <td>{customer.phone ?? "–"}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}