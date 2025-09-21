import { useState , useEffect } from "react";
import apiCall from "../services/axios";


export default function BookingList({id , date}) {

    const [bookings , setBookings ] = useState([])
    const [error , setError] = useState("")

    console.log(id ,)

    useEffect(() => {
        if (!id || !date) return;

        apiCall.get(`/bookings/${id}`, {
            params: { date }
        })
        .then(res => {
            console.log(res.data)
            setBookings(res.data)
        })
        .catch(err => {
            console.error(err)
            setError("Failed to fetch bookings")
        })

        }, [id, date])


  return (
    <div className="card shadow px-2 text-center">
        <h3 className="fw-bold py-3 mb-3 mx-5 px-4" style={{ color: "#22305d" }}>
        Bookings for this Court
        </h3>
        <div className="d-flex flex-column text-center">
        {bookings.length > 0 ? (
        bookings.map(b => (
            <div className="d-flex ps-4 gap-5" key={b.bookingId}>
            <p>User ID: <span className="h5">{b.userId}</span></p>
            <p>Booking ID: <span className="h5">{b.bookingId}</span></p>
            <p>Start: <span className="h5">{b.startTime}</span></p>
            <p>End: <span className="h5">{b.endTime}</span></p>
            <hr />
            </div>
            ))
            ) : (
            <p>No bookings found.</p>
            )}
        </div>
    </div>
  );
}
