import { Link } from "react-router-dom";
import "./HomePage.css";

interface MenuItem {
  to: string;
  title: string;
  description: string;
  icon: string;
}

const menuItems: MenuItem[] = [
  {
    to: "/calendar",
    title: "Kalender",
    description: "Se och lägg till aktiviteter",
    icon: "📅",
  },
  {
    to: "/customers",
    title: "Kunder",
    description: "Lista och lägg till kunder",
    icon: "👥",
  },
  {
    to: "/locations",
    title: "Platser",
    description: "Lista och lägg till platser och städer",
    icon: "📍",
  },
];

function HomePage() {
  return (
    <div className="home">
      <h1 className="home-title">Calendar Management</h1>

      <nav className="home-menu" aria-label="Huvudmeny">
        {menuItems.map((item) => (
          <Link key={item.to} to={item.to} className="home-card">
            <span className="home-card-icon" aria-hidden="true">
              {item.icon}
            </span>

            <span className="home-card-text">
              <span className="home-card-title">{item.title}</span>
              <span className="home-card-description">{item.description}</span>
            </span>

            <span className="home-card-arrow" aria-hidden="true">
              ›
            </span>
          </Link>
        ))}
      </nav>
    </div>
  );
}

export default HomePage;