import { useEffect, useState } from "react";
import ArenaCard from "../components/ArenaCard";
import apiCall from "../services/axios";

export default function Arenas() {
  
  const [loading, setLoading] = useState(false)
  const [arenas , setArenas] = useState([]);
  const [error , setError] = useState("")

  useEffect(() => {
    setLoading(true);
    setLoading(true)
    apiCall.get('/arenas')
    .then(res =>{
      console.log(res.data)
      setArenas(res.data);
    })
    .catch(err => {
      setError(err);
    })

    setLoading(false)
  },[])




  if(loading)
  {
    return(
    <>
      <div className="container py-5">
        <h2
          className="text-center fw-bold mb-5 pt-4 pb-2"
          style={{ color: "#22305d" }}
        >
            Getting Arenas for you... <br />
            <span className="pt-3">Please Wait</span>
        </h2>
      </div>
    </>
    );
  }



  return (
    <div className="container py-5">
      { error !== "" && 
        <>
          <h2
            className="text-center fw-bold mb-5 pt-4 pb-2"
            style={{ color: "#22305d" }}
            >
              Awww... We hit an error <br /> {error}
          </h2>
        </>
      }
      { arenas.length > 0 && 
      <>
      <h2
        className="text-center fw-bold mb-5 pt-4 pb-2"
        style={{ color: "#22305d" }}
      >
        Available Arenas
      </h2>
      <div className="row g-4">
        {arenas.map((arena, i) => (
          <div key={i} className="col-12 col-sm-6 col-md-4 col-lg-4 px-5">
            <ArenaCard {...arena} />
          </div>
        ))}
      </div>
      </>
      }
    </div>
  );
}
