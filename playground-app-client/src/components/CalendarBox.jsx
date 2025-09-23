import { useState } from "react";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";


//gpt
export default function CalendarBox({ onDateSelect }) {
  const [date, setDate] = useState(new Date());

  //gpt
  const formatLocalDate = (d) => {
  const yyyy = d.getFullYear();
  const mm = String(d.getMonth() + 1).padStart(2, "0");
  const dd = String(d.getDate()).padStart(2, "0");
  return `${yyyy}-${mm}-${dd}`;
};

  const handleDate = (newDate) => { 
    setDate(newDate);
    const updatedDate = formatLocalDate(newDate)
    onDateSelect(updatedDate);
  };

  return (
    <div className="card shadow-lg p-4 text-center" style={{ width: "25rem" }}>
      <h5 className="fw-bold mb-3" style={{ color: "#e63946" }}>
        Calendar
      </h5>

      <Calendar
        onChange={handleDate}  
        value={date}
        className="w-100 border-0"
      />

      <p className="mt-3 text-muted">
        Selected date: <strong>{date.toDateString()}</strong>
      </p>
    </div>
  );
}
