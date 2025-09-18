import { Link } from "react-router-dom";

export default function Navbar() {
  return (
    <nav className="navbar navbar-expand-lg bg-white shadow-sm">
      <div className="container">
        <Link
          className="navbar-brand"
          to="/"
          style={{
            fontFamily: "Brush Script MT, cursive",
            color: "#22305d",
            fontSize: "28px",
          }}
        >
          Book & Play
        </Link>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav ms-auto">
            <li className="nav-item">
              <Link className="nav-link fw-bold text-dark me-5 " to="/">
                Home
              </Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link fw-bold text-dark me-5" to="/">
                About Us
              </Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link fw-bold text-dark me-5" to="/">
                Contact
              </Link>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
}
