//Hämtar och håller kunderna------------
import { useEffect, useState } from "react";
import { createCustomer, getCustomers } from "../api/customersApi";
import type { Customer, CreateCustomerRequest } from "../types/customer";

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
      .finally(() => { //körs alltid, oavsett om det gick bra eller fel
        if (!ignore) setIsLoading(false);
      });

    return () => {
      ignore = true;
    };
  }, []);

  async function addCustomer(request: CreateCustomerRequest) {
    const created = await createCustomer(request);
    setCustomers((prev) =>
      [...prev, created].sort((a, b) => a.name.localeCompare(b.name, "sv"))
    );
  }

  return { customers, isLoading, error, addCustomer };
}