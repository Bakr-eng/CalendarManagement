import { useState } from "react";
import { NavLink, Outlet } from "react-router-dom";
import "./AppLayout.css";

interface NavItem {
  to: string;
  label: string;
  icon: string;
}

const navItems: NavItem[] = [
  { to: "/", label: "Hem", icon: "🏠" },
  { to: "/calendar", label: "Kalender", icon: "📅" },
  { to: "/customers", label: "Kunder", icon: "👥" },
  { to: "/locations", label: "Platser", icon: "📍" },
];

function AppLayout() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className="layout">
      <aside className={isOpen ? "sidebar sidebar-open" : "sidebar"}>
        <button
          type="button"
          className="sidebar-toggle"
          onClick={() => setIsOpen((prev) => !prev)}
          aria-label={isOpen ? "Stäng meny" : "Öppna meny"}
          aria-expanded={isOpen}
          aria-controls="sidebar-nav"
        >
          ☰
        </button>

        {/* Länkarna finns bara när menyn är öppen */}
        {isOpen && (
          <nav id="sidebar-nav" className="sidebar-nav" aria-label="Huvudmeny">
            {navItems.map((item) => (
              <NavLink
                key={item.to}
                to={item.to}
                end={item.to === "/"}
                className={({ isActive }) =>
                  isActive ? "sidebar-link sidebar-link-active" : "sidebar-link"
                }
              >
                <span className="sidebar-icon" aria-hidden="true">
                  {item.icon}
                </span>
                <span className="sidebar-label">{item.label}</span>
              </NavLink>
            ))}
          </nav>
        )}
      </aside>

      <main className="layout-content">
        <Outlet />
      </main>
    </div>
  );
}

export default AppLayout;