import { useState } from "react";
import { Modal } from "../components/ui/Modal";
import { CustomerDetails } from "../features/customers/components/CustomerDetails";
import { CustomerForm } from "../features/customers/components/CustomerForm";
import { CustomerList } from "../features/customers/components/CustomerList";
import { useCustomers } from "../features/customers/hooks/useCustomers";
import type { Customer } from "../features/customers/types/customer";

function CustomersPage() {
  const { customers, isLoading, error, addCustomer, editCustomer, removeCustomer } = useCustomers();

  // Kunden som visas i rutan, eller null om rutan är stängd
  const [selectedCustomer, setSelectedCustomer] = useState<Customer | null>(null);
  // false = visa detaljer, true = visa formuläret för att uppdatera
  const [isEditing, setIsEditing] = useState(false);
  const [deleteError, setDeleteError] = useState<string | null>(null);

  function openDetails(customer: Customer) {
    setSelectedCustomer(customer);
    setIsEditing(false);
    setDeleteError(null);
  }

  function closeModal() {
    setSelectedCustomer(null);
    setIsEditing(false);
    setDeleteError(null);
  }

  async function handleDelete(customer: Customer) {
    const confirmed = window.confirm(`Vill du ta bort ${customer.name}?`);
    if (!confirmed) {
      return;
    }

    try {
      await removeCustomer(customer.id);
      closeModal();
    } catch (err) {
      setDeleteError(err instanceof Error ? err.message : "Något gick fel");
    }
  }

  return (
    <div>
      <h1>Kunder</h1>

      <CustomerForm onSubmit={addCustomer} />

      {isLoading && <p>Laddar kunder...</p>}
      {error && <p role="alert">{error}</p>}
      {!isLoading && !error && (
        <CustomerList customers={customers} onSelect={openDetails} />
      )}

      {selectedCustomer && (
        <Modal
          title={isEditing ? "Uppdatera kund" : selectedCustomer.name}
          onClose={closeModal}
        >
          {deleteError && (
            <p className="modal-error" role="alert">
              {deleteError}
            </p>
          )}

          {isEditing ? (
            <CustomerForm
              initialCustomer={selectedCustomer}
              onSubmit={async (request) => {
                await editCustomer(selectedCustomer.id, request);
                closeModal();
              }}
              onCancel={() => setIsEditing(false)}
            />
          ) : (
            <CustomerDetails
              customer={selectedCustomer}
              onEdit={() => setIsEditing(true)}
              onDelete={() => handleDelete(selectedCustomer)}
            />
          )}
        </Modal>
      )}
    </div>
  );
}

export default CustomersPage;