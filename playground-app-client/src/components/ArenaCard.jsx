import { Link } from "react-router-dom";



export default function ArenaCard({ name, email, location }) {
  return (
    <>
      <div className="card shadow-sm border-0 py-3 px-3 rounded-3 shadow-lg h-80">
        <div className="card-body ">
          <h5 className="card-title text-center pb-3">
            <strong></strong> {name}
          </h5>
          <p className="card-text">
            <strong>Email:</strong> {email}
          </p>
          <p className="card-text">
            <strong>Location:</strong> {location}
          </p>
        </div>
      </div>
      <div className="text-center py-4">
        <Link
          to="/courts"
          style={{
            backgroundColor: "#22305d",
            color: "white",
            padding: "10px 20px",
            fontSize: "16px",
            fontWeight: "bold",
            borderRadius: "8px",
            cursor: "pointer",
          }}
          className="btn px-5 "
        >
          View Courts
        </Link>
      </div>
    </>
  );
}
