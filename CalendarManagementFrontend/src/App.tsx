
import { lazy, Suspense } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import AppLayout from "./components/layout/AppLayout";
import HomePage from "./pages/HomePage";
import ActivityDetailsPage from "./pages/ActivityDetailsPage";

// Laddas först när någon går till sidan
const CalendarPage = lazy(() => import("./pages/CalendarPage"));
const CustomersPage = lazy(() => import("./pages/CustomersPage"));
const LocationsPage = lazy(() => import("./pages/LocationsPage"));

function App() {
  return (
    <BrowserRouter>
      <Suspense fallback={<p>Laddar...</p>}>
        <Routes>
          {/* Vanliga sidor med huvudmenyn */}
          <Route element={<AppLayout />}>
            <Route path="/" element={<HomePage />} />
            <Route path="/calendar" element={<CalendarPage />} />
            <Route path="/customers" element={<CustomersPage />} />
            <Route path="/locations" element={<LocationsPage />} />
          </Route>

          {/* Aktivitetssida med egen sidomeny */}
          <Route
            path="/activities/:id"
            element={<ActivityDetailsPage />}
          />
        </Routes>
      </Suspense>
    </BrowserRouter>
  );
}

export default App;
