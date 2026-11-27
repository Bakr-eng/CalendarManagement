import { useEffect, useState } from "react";
import {
  createCustomer,
  deleteCustomer,
  getCustomer,
  getCustomers,
  updateCustomer,
} from "../api/customersApi";
import type { Customer, CreateCustomerRequest, UpdateCustomerRequest } from "../types/customer";

export function useCustomers() {
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let ignore = false;

    getCustomers()
      .then((data) => {
        if (!ignore) setCustomers(data);
      })
      .catch((err: unknown) => {
        if (!ignore) setError(err instanceof Error ? err.message : "Något gick fel");
      })
      .finally(() => {
        if (!ignore) setIsLoading(false);
      });

    return () => {
      ignore = true;
    };
  }, []);

  async function reloadCustomers() {
    setCustomers(await getCustomers());
  }

  async function addCustomer(request: CreateCustomerRequest) {
    await createCustomer(request);
    await reloadCustomers();
  }

  async function editCustomer(id: number, request: UpdateCustomerRequest) {
    await updateCustomer(id, request);
    await reloadCustomers();
  }

  async function removeCustomer(id: number) {
    await deleteCustomer(id);
    setCustomers((prev) => prev.filter((c) => c.id !== id));
  }

  // Hämtar en kund med hela personnumret, till detaljrutan
  function loadCustomer(id: number) {
    return getCustomer(id);
  }

  return { customers, isLoading, error, addCustomer, editCustomer, removeCustomer, loadCustomer };
}