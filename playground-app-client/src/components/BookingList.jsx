
function Button(){
    return (
        <>
            <div className="mx-3">
                <p>Booking Id</p>
            </div>
        </>
    )
}


export default function BookingList() {
  return (
    <div className="card shadow px-3 text-center">
        <h3 className="fw-bold py-3 mb-3 mx-5 px-4" style={{ color: "#22305d" }}>
        Bookings for this Court
      </h3>
      
    </div>
  );
}
