import { useState } from "react";
import "../styles/page.css";
import { Modal } from "../components/ui/Modal";
import { CustomerDetails } from "../features/customers/components/CustomerDetails";
import { CustomerForm } from "../features/customers/components/CustomerForm";
import { CustomerList } from "../features/customers/components/CustomerList";
import { useCustomers } from "../features/customers/hooks/useCustomers";
import type { Customer } from "../features/customers/types/customer";

function CustomersPage() {
  const { customers, isLoading, error, addCustomer, editCustomer, removeCustomer, loadCustomer } =
    useCustomers();

  // Kunden som visas i rutan, eller null om rutan är stängd
  const [selectedCustomer, setSelectedCustomer] = useState<Customer | null>(null);
  // true medan hela kunden (med fullt personnummer) hämtas
  const [isLoadingDetails, setIsLoadingDetails] = useState(false);
  // false = visa detaljer, true = visa formuläret för att uppdatera
  const [isEditing, setIsEditing] = useState(false);
  const [loadError, setLoadError] = useState<string | null>(null);
  const [deleteError, setDeleteError] = useState<string | null>(null);
  // true = rutan för att lägga till visas
  const [isCreating, setIsCreating] = useState(false);

  async function openDetails(customer: Customer) {
    // Visa rutan direkt med namnet från listan...
    setSelectedCustomer(customer);
    setIsEditing(false);
    setLoadError(null);
    setDeleteError(null);
    setIsLoadingDetails(true);

    // ...och hämta sedan kunden igen, för i listan är personnumret maskerat
    try {
      const fullCustomer = await loadCustomer(customer.id);
      setSelectedCustomer(fullCustomer);
    } catch (err) {
      setLoadError(err instanceof Error ? err.message : "Något gick fel");
    } finally {
      setIsLoadingDetails(false);
    }
  }

  function closeModal() {
    setSelectedCustomer(null);
    setIsEditing(false);
    setLoadError(null);
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
      <div className="page-header">
        <h1>Kunder</h1>
        <button type="button" className="page-add-button" onClick={() => setIsCreating(true)}>
          + Lägg till kund
        </button>
      </div>

      {isLoading && <p>Laddar kunder...</p>}
      {error && <p role="alert">{error}</p>}
      {!isLoading && !error && (
        <CustomerList customers={customers} onSelect={openDetails} />
      )}

      {isCreating && (
        <Modal title="Ny kund" onClose={() => setIsCreating(false)}>
          <CustomerForm
            onSubmit={async (request) => {
              await addCustomer(request);
              setIsCreating(false);
            }}
            onCancel={() => setIsCreating(false)}
          />
        </Modal>
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

          {loadError ? (
            <p className="modal-error" role="alert">
              {loadError}
            </p>
          ) : isLoadingDetails ? (
            <p>Laddar...</p>
          ) : isEditing ? (
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