import CourtCard from "../components/CourtCard";

export default function Courts() {
  const courts = [
    { title: "Tennis Court A", img: "" },
    { title: "Tennis Court A", img: "" },
    { title: "Tennis Court A", img: "" },
    { title: "Tennis Court A", img: "" },
  ];

  return (
    <div className="container py-5">
    
      <div className="d-flex justify-content-center mb-4 pb-4">
        <select className="border-2 border-black shadow-md form-select w-50">
            <option value="">Filter by type...</option>
            <option value="tennis">Tennis</option>
            <option value="football">Football</option>
            <option value="basketball">Basketball</option>
            <option value="cricket">Cricket</option>
        </select>
      </div>

      <div className="row d-flex justify-content-center align-items-center">
        <div className="col-lg-3">
          <h2 className="text-center fw-bold mb-4 pb-4" style={{ 
            color: "#22305d" 
            }}>
              Available Courts
          </h2>
        </div>
      </div>

      <div className="row g-4">
        {courts.map((court, i) => (
          <div key={i} className="col-12 col-sm-6 col-md-4 col-lg-4 py-3 px-4 ">
            <CourtCard {...court} />
          </div>
        ))}
      </div>

    </div>
  );
}
