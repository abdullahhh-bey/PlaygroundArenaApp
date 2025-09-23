import BookingList from "../components/BookingList";
import CalendarBox from "../components/CalendarBox";
import AddBookingForm from "../components/AddBookingForm";
import AvailableSlots from "../components/AvailableSlots";
import { useParams } from "react-router";
import { useState } from "react";

export default function Bookings() {

    const [selectedDate, setSelectedDate] = useState(new Date());
    const { courtId } = useParams();
    const [selectedSlots , setSelectedSlots] = useState([]);

    const toggleButton = (slot) => {
        setSelectedSlots((prev) => {
            const check = prev.find(s => s.Id === slot.Id)
            if(check){
                return prev.filter(s => s.Id !== slot.Id)
            } else {
                return [...prev , slot]
            }
        })

        console.log(selectedSlots)
    }

      const totalPrice = selectedSlots.reduce((sum, slot) => sum + slot.price, 0);

  return (

    <div className="container-fluid px-4 py-5">
      <div className="row mb-5  d-flex justify-content-center">

        <div className="col-md-5 col-lg-5 d-flex justify-content-center">
          <BookingList id={courtId} date={selectedDate} />
        </div>
        <div className="col-md-5 col-lg-5 d-flex justify-content-center">
          <CalendarBox onDateSelect={setSelectedDate} />
        </div>
      </div>

      <div className="row mb-5">
        <div className="col-md-8 mx-auto">
          <AddBookingForm id={courtId} slots={selectedSlots} price={totalPrice} />
        </div>
      </div>

      <div className="row">
        <div className="col-12 pt-4">
          <AvailableSlots id={courtId} date={selectedDate} selectedSlot={selectedSlots}
          setSlot={setSelectedSlots}/>
        </div>
      </div>
    </div>
  );
}
