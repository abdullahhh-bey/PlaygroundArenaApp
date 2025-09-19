import { useEffect, useState } from "react";
import apiCall from "../services/axios";

export default function AvailableSlots({ id, date }) {

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const [slots, setSlots] = useState([]);
  const [price , setPrice] = useState()
  const [addprice , setAddPrice] = useState(false)

  useEffect(() => {
    if (!id || !date) return; 

    const fetchSlots = async () => {
      setLoading(true);
      try {
        const res = await apiCall.get(
          `/courts/${id}/slots/available`,
          {
            params: { date }, 
          }
        );
        console.log(res.data);
        setSlots(res.data.slots || []);
      } catch (err) {
        console.error(err);
        setError(err.message || "Something went wrong");
      } finally {
        setLoading(false);
      }
    };

    fetchSlots();
  }, [id, date]);


  if (loading) {
    return <h1>Loading...</h1>;
  }

  if (error) {
    return <h4 className="text-danger text-center">{error}</h4>;
  }




  return (
    <>
      <h4 className="fw-bold text-center mb-4" style={{ color: "#22305d" }}>
        Available Slots for this Court
      </h4>
      <div className="d-flex flex-wrap justify-content-center gap-3">
        {slots.length > 0 ? (
          slots.map((slot, index) => (
            <div
              key={index}
              className="border rounded px-3 py-2"
              style={{
                minWidth: "150px",
                textAlign: "center",
                background: "#fff",
                borderColor: "#22305d",
                color: "#22305d",
                fontWeight: "500",
              }}
            >
              <button className="btn btn-outline-success py-2 my-3 px-4"><h5>{slot.start}-{slot.end}</h5></button>
              <div>Price: {slot.price}</div>
            </div>
          ))
        ) : (
          <p className="text-center">No slots available</p>
        )}
      </div>
    </>
  );
}
