import { getErrorMessage } from "../../../shared/api/getErrorMessage";
import type { Customer, CreateCustomerRequest, UpdateCustomerRequest } from "../types/customer";

// Anropen går via Vite-proxyn i vite.config.ts till backend
const BASE_URL = "/api/customers";

export async function getCustomers(): Promise<Customer[]> {
  const response = await fetch(BASE_URL);

  if (!response.ok) {
    throw new Error(await getErrorMessage(response, "Kunde inte hämta kunder"));
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
    throw new Error(await getErrorMessage(response, "Kunde inte skapa kunden"));
  }

  return response.json();
}

export async function updateCustomer(id: number, request: UpdateCustomerRequest): Promise<Customer> {
  const response = await fetch(`${BASE_URL}/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error(await getErrorMessage(response, "Kunde inte uppdatera kunden"));
  }

  return response.json();
}

export async function deleteCustomer(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: "DELETE" });

  if (!response.ok) {
    throw new Error(await getErrorMessage(response, "Kunde inte ta bort kunden"));
  }
  // 204 No Content – inget att läsa
}