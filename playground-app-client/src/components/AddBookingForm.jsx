import apiCall from "../services/axios";




export default function AddBookingForm({id , price}) {

    const handleSubmit = () => {
        
        apiCall.post(`/admincontrol/bookings`, {})
    }    

  return (
    <div className="card shadow-sm p-4">
      <h5 className="fw-bold mb-3" style={{ color: "#22305d" }}>
        Add Booking
      </h5>
      <form className="row g-3">
        <div className="col-md-6">
          <input type="text" className="form-control" placeholder="Your Name ..." />
        </div>
        <div className="col-md-6">
          <input type="date" className="form-control" />
        </div>
        <div className="col-md-6">
          <input type="text" value={price} className="form-control" disabled="true" />
        </div>
        <div className="col-12 text-center">
          <button onChange={handleSubmit} className="btn btn-success px-4">
            Book Now
          </button>
        </div>
      </form>
    </div>
  );
}
