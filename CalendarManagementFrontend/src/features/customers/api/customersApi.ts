import type { Customer, CreateCustomerRequest, UpdateCustomerRequest } from "../types/customer";

// Anropen går via Vite-proxyn i vite.config.ts till backend
const BASE_URL = "/api/customers";


// Den här funktionen plockar ut texterna så att användaren ser vad som är fel.
async function readErrorMessage(response: Response, fallback: string): Promise<string> {
  if (response.status !== 400) {
    return fallback;
  }

  try {
    const problem: { errors?: Record<string, string[]> } = await response.json();
    const messages = Object.values(problem.errors ?? {}).flat();
    return messages.length > 0 ? messages.join(" ") : fallback;
  } catch {
    return fallback; // svaret var inte JSON
  }
}

export async function getCustomers(): Promise<Customer[]> {
  const response = await fetch(BASE_URL);

  if (!response.ok) {
    throw new Error("Kunde inte hämta kunder");
  }

  return response.json();
}

// Hämtar en kund med hela personnumret (används i detaljerna)
export async function getCustomer(id: number): Promise<Customer> {
  const response = await fetch(`${BASE_URL}/${id}`);

  if (!response.ok) {
    throw new Error("Kunde inte hämta kunden");
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
    throw new Error(await readErrorMessage(response, "Kunde inte skapa kunden"));
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
    throw new Error(await readErrorMessage(response, "Kunde inte uppdatera kunden"));
  }

  return response.json();
}

export async function deleteCustomer(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: "DELETE" });

  if (!response.ok) {
    throw new Error("Kunde inte ta bort kunden");
  }
  // 204 No Content – inget att läsa
}