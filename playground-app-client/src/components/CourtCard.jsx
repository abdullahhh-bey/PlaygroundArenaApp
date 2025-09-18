export default function CourtCard({ title, img }) {
  return (
    <div className="card shadow border-0 rounded-3 h-100">
      <div className="position-relative">
        <span className="badge bg-danger position-absolute top-0 end-0 px-3 py-1">
          Type
        </span>
      </div>
      <div className="card-body text-center py-3">
        <h5 className="card-title py-2">{title}</h5>
        <button
          style={{
            backgroundColor: "#22305d",
            color: "white",
          }}
          className="btn btn-success px-4 mt-3 py-2"
        >
          See Time Slots
        </button>
      </div>
    </div>
  );
}
