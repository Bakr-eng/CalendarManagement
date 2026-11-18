import { LocationForm } from "../features/locations/components/LocationForm";
import { LocationList } from "../features/locations/components/LocationList";
import { useLocations } from "../features/locations/hooks/useLocations";

function LocationsPage() {
  const { locations, isLoading, error, addLocation } = useLocations();

  return (
    <div>
      <h1>Platser</h1>

      <LocationForm onSubmit={addLocation} />

      {isLoading && <p>Laddar platser...</p>}
      {error && <p role="alert">{error}</p>}
      {!isLoading && !error && <LocationList locations={locations} />}
    </div>
  );
}

export default LocationsPage;