import { useState } from "react";
import apiCall from "../services/axios";

export default function AddBookingForm({ id, date, slots, price }) {
  const [success, setSuccess] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const body = {
        userId: 1,
        courtId: parseInt(id),
        timeSlotId: slots.map((s) => s.id)
      };

      console.log(body);

      const res = await apiCall.post(`admincontrol/bookings`, body);

      console.log(res.data);
      setSuccess("Booking successful!");
    } catch (err) {
      console.error(err);
      setError(err.message || "Something went wrong");
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <h1>Loading...</h1>;
  if (error !== "") return <h4 className="text-danger text-center">{error}</h4>;
  if (success !== "")
    return (
      <div className="d-flex justify-content-center align-items-center">
        <h1>{success}</h1>
      </div>
    );

  return (
    <div className="card shadow-sm p-4">
      {error === "" && success === "" && (
        <>
          <h5 className="fw-bold mb-3" style={{ color: "#22305d" }}>
            Add Booking
          </h5>
          <form className="row g-3" onSubmit={handleSubmit}>
            <div className="col-md-6">
              <input
                type="text"
                className="form-control"
                placeholder="Your Name ..."
              />
            </div>
            <div className="col-md-6">
              <input
                type="text"
                value={`Total Price: ${price}`}
                className="form-control"
                disabled={true}
              />
            </div>
            <div className="col-12 text-center">
              <button type="submit" className="btn btn-success px-4">
                Book Now
              </button>
            </div>
          </form>
        </>
      )}
    </div>
  );
}
