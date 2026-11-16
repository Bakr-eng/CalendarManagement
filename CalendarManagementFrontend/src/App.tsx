import { BrowserRouter, Routes, Route } from "react-router-dom";
import CalendarPage from "./pages/CalendarPage";
import CustomersPage from "./pages/CustomersPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<CalendarPage />} />
        <Route path="/calendar" element={<CalendarPage />} />
        <Route path="/customers" element={<CustomersPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;