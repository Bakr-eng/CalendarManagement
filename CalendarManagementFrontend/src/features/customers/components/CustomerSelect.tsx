import { useCustomers } from "../hooks/useCustomers";

interface CustomerSelectProps {
  id?: string; // så att en <label htmlFor="..."> kan kopplas till listan
  value: number | null;
  onChange: (customerId: number | null) => void;
}

export function CustomerSelect({ id, value, onChange }: CustomerSelectProps) {
  const { customers, isLoading } = useCustomers();

  return (
    <select
      id={id}
      value={value ?? ""}
      onChange={(e) => onChange(e.target.value ? Number(e.target.value) : null)}
      disabled={isLoading}
    >
      <option value="">{isLoading ? "Laddar..." : "Välj kund"}</option>
      {customers.map((customer) => (
        <option key={customer.id} value={customer.id}>
          {customer.name}
        </option>
      ))}
    </select>
  );
}