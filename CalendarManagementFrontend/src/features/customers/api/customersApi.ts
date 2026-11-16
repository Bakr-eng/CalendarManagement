import type { Customer, CreateCustomerRequest } from "../types/customer";

const API_URL = import.meta.env.VITE_API_URL; 

export async function getCustomers(): Promise<Customer[]> {
  const response = await fetch(`${API_URL}/api/customers`);

  if (!response.ok) {
    throw new Error("Kunde inte hämta kunder");
  }

  return response.json();
}

export async function createCustomer(request: CreateCustomerRequest): Promise<Customer> {
  const response = await fetch(`${API_URL}/api/customers`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Kunde inte skapa kunden");
  }

  return response.json();
}