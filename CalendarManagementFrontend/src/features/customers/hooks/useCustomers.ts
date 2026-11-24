import { useEffect, useState } from "react";
import { createCustomer, deleteCustomer, getCustomers, updateCustomer } from "../api/customersApi";
import type { Customer, CreateCustomerRequest, UpdateCustomerRequest } from "../types/customer";

function sortByName(list: Customer[]): Customer[] {
  return [...list].sort((a, b) => a.name.localeCompare(b.name, "sv"));
}

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

  async function addCustomer(request: CreateCustomerRequest) {
    const created = await createCustomer(request);
    setCustomers((prev) => sortByName([...prev, created]));
  }

  async function editCustomer(id: number, request: UpdateCustomerRequest) {
    const updated = await updateCustomer(id, request);
    setCustomers((prev) => sortByName(prev.map((c) => (c.id === id ? updated : c))));
  }

  async function removeCustomer(id: number) {
    await deleteCustomer(id);
    setCustomers((prev) => prev.filter((c) => c.id !== id));
  }

  return { customers, isLoading, error, addCustomer, editCustomer, removeCustomer };
}