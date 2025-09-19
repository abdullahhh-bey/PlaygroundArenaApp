import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Arenas from "./pages/Arenas";
import Courts from "./pages/Courts";
import Navbar from "./components/Navbar";
import Bookings from "./pages/Bookings";

function App() {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/" element={<Arenas />} />
        {/*For passing the arena id in url and get it using useParams() hook*/}
        <Route path="/arenas/:arenaId/courts" element={<Courts />} />
        <Route path="courts/:courtId/slots" element={<Bookings />} />
      </Routes>
    </Router>
  );
}

export default App;
