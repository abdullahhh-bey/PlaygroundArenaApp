import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Arenas from "./pages/Arenas";
import Courts from "./pages/Courts";
import Navbar from "./components/Navbar";

function App() {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/" element={<Arenas />} />
        <Route path="/courts" element={<Courts />} />
      </Routes>
    </Router>
  );
}

export default App;
