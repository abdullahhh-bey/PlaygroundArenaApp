



export default function AddBookingForm() {
  return (
    <div className="card shadow-sm p-4">
      <h5 className="fw-bold mb-3" style={{ color: "#22305d" }}>
        Add Booking
      </h5>
      <form className="row g-3">
        <div className="col-md-6">
          <input type="text" className="form-control" placeholder="Enter Name..." />
        </div>
        <div className="col-md-6">
          <input type="date" className="form-control" />
        </div>
        <div className="col-md-6">
          <select className="form-select">
            <option>Select Start Time</option>
            <option>10:00 AM</option>
            <option>11:00 AM</option>
          </select>
        </div>
        <div className="col-md-6">
          <select className="form-select">
            <option>Select End Time</option>
            <option>11:00 AM</option>
            <option>12:00 PM</option>
          </select>
        </div>
        <div className="col-12 text-center">
          <button type="submit" className="btn btn-success px-4">
            Add Booking
          </button>
        </div>
      </form>
    </div>
  );
}
