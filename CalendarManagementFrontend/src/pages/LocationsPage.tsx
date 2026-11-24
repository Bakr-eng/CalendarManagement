import { useState } from "react";
import { Modal } from "../components/ui/Modal";
import { LocationDetails } from "../features/locations/components/LocationDetails";
import { LocationForm } from "../features/locations/components/LocationForm";
import { LocationList } from "../features/locations/components/LocationList";
import { useLocations } from "../features/locations/hooks/useLocations";
import type { Location } from "../features/locations/types/location";

function LocationsPage() {
  const { locations, isLoading, error, addLocation, editLocation, removeLocation } = useLocations();

  // Platsen som visas i rutan, eller null om rutan är stängd
  const [selectedLocation, setSelectedLocation] = useState<Location | null>(null);
  // false = visa detaljer, true = visa formuläret för att uppdatera
  const [isEditing, setIsEditing] = useState(false);
  const [deleteError, setDeleteError] = useState<string | null>(null);

  function openDetails(location: Location) {
    setSelectedLocation(location);
    setIsEditing(false);
    setDeleteError(null);
  }

  function closeModal() {
    setSelectedLocation(null);
    setIsEditing(false);
    setDeleteError(null);
  }

  async function handleDelete(location: Location) {
    const confirmed = window.confirm(`Vill du ta bort ${location.name}, ${location.city}?`);
    if (!confirmed) {
      return;
    }

    try {
      await removeLocation(location.id);
      closeModal();
    } catch (err) {
      setDeleteError(err instanceof Error ? err.message : "Något gick fel");
    }
  }

  return (
    <div>
      <h1>Platser</h1>

      <LocationForm onSubmit={addLocation} />

      {isLoading && <p>Laddar platser...</p>}
      {error && <p role="alert">{error}</p>}
      {!isLoading && !error && (
        <LocationList locations={locations} onSelect={openDetails} />
      )}

      {selectedLocation && (
        <Modal
          title={isEditing ? "Uppdatera plats" : `${selectedLocation.name}, ${selectedLocation.city}`}
          onClose={closeModal}
        >
          {deleteError && (
            <p className="modal-error" role="alert">
              {deleteError}
            </p>
          )}

          {isEditing ? (
            <LocationForm
              initialLocation={selectedLocation}
              onSubmit={async (request) => {
                await editLocation(selectedLocation.id, request);
                closeModal();
              }}
              onCancel={() => setIsEditing(false)}
            />
          ) : (
            <LocationDetails
              location={selectedLocation}
              onEdit={() => setIsEditing(true)}
              onDelete={() => handleDelete(selectedLocation)}
            />
          )}
        </Modal>
      )}
    </div>
  );
}

export default LocationsPage;