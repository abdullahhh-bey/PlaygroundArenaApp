import { useEffect, useState } from "react";
import CourtCard from "../components/CourtCard";
import { useParams } from "react-router";
import apiCall from "../services/axios";

export default function Courts() {

  const { arenaId } = useParams();

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [courts, setCourts] = useState([]);

  useEffect(() => {
    setLoading(true);

    apiCall
      .get(`/arenas/${arenaId}/courts`)
      .then((res) => {
        console.log(res.data.courtDetails)
        setCourts(res.data.courtDetails); 
      })
      .catch((err) => {
        setError(err);
      })
    
      setLoading(false);
  }, [arenaId]);

  if (loading) {
    return (
      <div className="container py-5">
        <div className="d-flex justify-content-center mb-4 pb-4">
          <h2>Hold on! I'm getting it for you...</h2>
        </div>
      </div>
    );
  }

  return (
    <div className="container py-5">

      {error !== "" && (
        <div className="row d-flex justify-content-center align-items-center">
          <div className="col-lg-6">
            <h2
              className="text-center fw-bold mb-4 pb-4"
              style={{ color: "#22305d" }}
            >
              Aww... We hit an error <br /> {error.message}
            </h2>
          </div>
        </div>
      )}

      {courts.length > 0 && (
        <>
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
              <h2
                className="text-center fw-bold mb-4 pb-4"
                style={{ color: "#22305d" }}
              >
                Available Courts
              </h2>
            </div>
          </div>

          <div className="row g-4">
            {courts.map((court, index) => (
              <div
                key={index}
                className="col-12 col-sm-6 col-md-4 col-lg-4 py-3 px-4"
              >
                <CourtCard {...court} />
              </div>
            ))}
          </div>
        </>
      )}

      {courts.length === 0 && error === "" && (
        <p className="text-center mt-4">No courts available.</p>
      )}
    </div>
  );
}
