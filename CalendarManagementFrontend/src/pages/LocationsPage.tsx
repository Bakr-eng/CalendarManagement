import { useState } from "react";
import { LocationForm } from "../features/locations/components/LocationForm";
import { LocationList } from "../features/locations/components/LocationList";
import { useLocations } from "../features/locations/hooks/useLocations";
import type { Location } from "../features/locations/types/location";

function LocationsPage() {
  const { locations, isLoading, error, addLocation, editLocation, removeLocation } = useLocations();
  const [editingLocation, setEditingLocation] = useState<Location | null>(null);
  const [actionError, setActionError] = useState<string | null>(null);

  async function handleDelete(location: Location) {
    const confirmed = window.confirm(`Vill du ta bort ${location.name}, ${location.city}?`);
    if (!confirmed) {
      return;
    }

    setActionError(null);

    try {
      await removeLocation(location.id);

      // Om platsen som togs bort höll på att redigeras – stäng formuläret
      if (editingLocation?.id === location.id) {
        setEditingLocation(null);
      }
    } catch (err) {
      setActionError(err instanceof Error ? err.message : "Något gick fel");
    }
  }

  return (
    <div>
      <h1>Platser</h1>

      {editingLocation ? (
        <LocationForm
          key={editingLocation.id}
          initialLocation={editingLocation}
          onSubmit={async (request) => {
            await editLocation(editingLocation.id, request);
            setEditingLocation(null);
          }}
          onCancel={() => setEditingLocation(null)}
        />
      ) : (
        <LocationForm onSubmit={addLocation} />
      )}

      {actionError && <p role="alert">{actionError}</p>}

      {isLoading && <p>Laddar platser...</p>}
      {error && <p role="alert">{error}</p>}
      {!isLoading && !error && (
        <LocationList
          locations={locations}
          onEdit={(location) => {
            setActionError(null);
            setEditingLocation(location);
          }}
          onDelete={handleDelete}
        />
      )}
    </div>
  );
}

export default LocationsPage;