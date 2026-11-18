// Hämtar och håller platserna
import { useEffect, useState } from "react";
import { createLocation, getLocations } from "../api/locationsApi";
import type { Location, CreateLocationRequest } from "../types/location";

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
    setLocations((prev) =>
      [...prev, created].sort((a, b) => a.name.localeCompare(b.name, "sv"))
    );
  }

  return { locations, isLoading, error, addLocation };
}