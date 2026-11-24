// Hämtar och håller platserna
import { useEffect, useState } from "react";
import { createLocation, deleteLocation, getLocations, updateLocation } from "../api/locationsApi";
import type { Location, CreateLocationRequest, UpdateLocationRequest } from "../types/location";

// Sortera på namn, och på stad om två platser har samma namn
function sortLocations(list: Location[]): Location[] {
  return [...list].sort(
    (a, b) => a.name.localeCompare(b.name, "sv") || a.city.localeCompare(b.city, "sv")
  );
}

export function useLocations() {
  const [locations, setLocations] = useState<Location[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let ignore = false;

    getLocations()
      .then((data) => {
        if (!ignore) setLocations(data);
      })
      .catch((err: unknown) => {
        if (!ignore) setError(err instanceof Error ? err.message : "Något gick fel");
      })
      .finally(() => {
        // körs alltid, oavsett om det gick bra eller fel
        if (!ignore) setIsLoading(false);
      });

    return () => {
      ignore = true;
    };
  }, []);

  async function addLocation(request: CreateLocationRequest) {
    const created = await createLocation(request);
    setLocations((prev) => sortLocations([...prev, created]));
  }

  async function editLocation(id: number, request: UpdateLocationRequest) {
    const updated = await updateLocation(id, request);
    setLocations((prev) => sortLocations(prev.map((l) => (l.id === id ? updated : l))));
  }

  async function removeLocation(id: number) {
    await deleteLocation(id);
    setLocations((prev) => prev.filter((l) => l.id !== id));
  }

  return { locations, isLoading, error, addLocation, editLocation, removeLocation };
}