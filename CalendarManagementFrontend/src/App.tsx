import { BrowserRouter, Routes, Route } from "react-router-dom";
import CalendarPage from "./pages/CalendarPage";
import CustomersPage from "./pages/CustomersPage";
import LocationsPage from "./pages/LocationsPage";
import ActivityDetailsPage from "./pages/ActivityDetailsPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<CalendarPage />} />
        <Route path="/calendar" element={<CalendarPage />} />
        <Route path="/customers" element={<CustomersPage />} />
        <Route path="/locations" element={<LocationsPage />} />
        <Route path="/activities/:id" element={<ActivityDetailsPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;