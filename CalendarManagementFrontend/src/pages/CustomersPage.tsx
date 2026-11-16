import { CustomerForm } from "../features/customers/components/CustomerForm";
import { CustomerList } from "../features/customers/components/CustomerList";
import { useCustomers } from "../features/customers/hooks/useCustomers";

function CustomersPage() {
  const { customers, isLoading, error, addCustomer } = useCustomers();

  return (
    <div>
      <h1>Kunder</h1>

      <CustomerForm onSubmit={addCustomer} />

      {isLoading && <p>Laddar kunder...</p>}
      {error && <p role="alert">{error}</p>}
      {!isLoading && !error && <CustomerList customers={customers} />}
    </div>
  );
}

export default CustomersPage;