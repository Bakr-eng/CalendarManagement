import type { Customer, CreateCustomerRequest } from "../types/customer";

// Anropen går via Vite-proxyn i vite.config.ts till backend
const BASE_URL = "/api/customers";

export async function getCustomers(): Promise<Customer[]> {
  const response = await fetch(BASE_URL);

  if (!response.ok) {
    throw new Error("Kunde inte hämta kunder");
  }

  return response.json();
}

export async function createCustomer(request: CreateCustomerRequest): Promise<Customer> {
  const response = await fetch(BASE_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Kunde inte skapa kunden");
  }

  return response.json();
}