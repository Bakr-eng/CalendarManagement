import type { Location, CreateLocationRequest, UpdateLocationRequest } from "../types/location";

// Anropen går via Vite-proxyn i vite.config.ts till backend
const BASE_URL = "/api/locations";

export async function getLocations(): Promise<Location[]> {
  const response = await fetch(BASE_URL);

  if (!response.ok) {
    throw new Error("Kunde inte hämta platser");
  }

  return response.json();
}

export async function createLocation(request: CreateLocationRequest): Promise<Location> {
  const response = await fetch(BASE_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Kunde inte skapa platsen");
  }

  return response.json();
}

export async function updateLocation(id: number, request: UpdateLocationRequest): Promise<Location> {
  const response = await fetch(`${BASE_URL}/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Kunde inte uppdatera platsen");
  }

  return response.json();
}

export async function deleteLocation(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: "DELETE" });

  if (!response.ok) {
    throw new Error("Kunde inte ta bort platsen");
  }
  // 204 No Content – inget att läsa
}