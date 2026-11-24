import { useState } from "react";
import { CustomerForm } from "../features/customers/components/CustomerForm";
import { CustomerList } from "../features/customers/components/CustomerList";
import { useCustomers } from "../features/customers/hooks/useCustomers";
import type { Customer } from "../features/customers/types/customer";

function CustomersPage() {
  const { customers, isLoading, error, addCustomer, editCustomer, removeCustomer } = useCustomers();
  const [editingCustomer, setEditingCustomer] = useState<Customer | null>(null);
  const [actionError, setActionError] = useState<string | null>(null);

  async function handleDelete(customer: Customer) {
    const confirmed = window.confirm(`Vill du ta bort ${customer.name}?`);
    if (!confirmed) {
      return;
    }

    setActionError(null);

    try {
      await removeCustomer(customer.id);

      // Om kunden som togs bort höll på att redigeras – stäng formuläret
      if (editingCustomer?.id === customer.id) {
        setEditingCustomer(null);
      }
    } catch (err) {
      setActionError(err instanceof Error ? err.message : "Något gick fel");
    }
  }

  return (
    <div>
      <h1>Kunder</h1>

      {editingCustomer ? (
        <CustomerForm
          key={editingCustomer.id}
          initialCustomer={editingCustomer}
          onSubmit={async (request) => {
            await editCustomer(editingCustomer.id, request);
            setEditingCustomer(null);
          }}
          onCancel={() => setEditingCustomer(null)}
        />
      ) : (
        <CustomerForm onSubmit={addCustomer} />
      )}

      {actionError && <p role="alert">{actionError}</p>}

      {isLoading && <p>Laddar kunder...</p>}
      {error && <p role="alert">{error}</p>}
      {!isLoading && !error && (
        <CustomerList
          customers={customers}
          onEdit={(customer) => {
            setActionError(null);
            setEditingCustomer(customer);
          }}
          onDelete={handleDelete}
        />
      )}
    </div>
  );
}

export default CustomersPage;