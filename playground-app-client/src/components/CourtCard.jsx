import { useState } from "react";

function CourtCard({ name, courtType }) {




  //btn style by gpt
  const [hovered, setHovered] = useState(false);
  const btnStyle = {
    background: "linear-gradient(90deg, #2EC4B6 50%, #2B3A67 50%)",
    backgroundSize: "200% 100%",
    backgroundPosition: hovered ? "left bottom" : "right bottom" ,
    color: "white",
    border: "none",
    padding: "12px 24px",
    fontSize: "16px",
    fontWeight: "bold",
    borderRadius: "8px",
    cursor: "pointer",
    transition: "background-position 0.5s ease-in-out",
  };


  return (
    <div className="card shadow border-0 rounded-3 h-100">
      <div className="position-relative">
        <span className="badge bg-danger position-absolute top-0 end-0 px-3 py-1">
          {courtType}
        </span>
      </div>

      <div className="card-body text-center py-4">
        <h5 className="card-title py-4">{name}</h5>
        <button
          style={btnStyle}
          onMouseEnter={() => setHovered(true)}
          onMouseLeave={() => setHovered(false)}
        >
          See Time Slots
        </button>
      </div>
    </div>
  );
}

export default CourtCard;
