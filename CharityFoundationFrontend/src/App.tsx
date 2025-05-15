import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./pages/Login";
import Register from "./pages/Register";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/" element={<div className="text-center text-2xl mt-10">Dobrodošli u Charity Foundation</div>} />
      </Routes>
    </Router>
  );
}

export default App;