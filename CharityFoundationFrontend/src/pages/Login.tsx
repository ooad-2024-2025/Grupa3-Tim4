import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";

export default function Login() {
  const [email, setEmail] = useState("");
  const [lozinka, setLozinka] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const res = await api.post("/korisnik/login", { email, lozinka });
      localStorage.setItem("token", res.data.token);
      localStorage.setItem("tipKorisnika", res.data.tipKorisnika);
      navigate("/");
    } catch {
      alert("Neuspješna prijava. Provjerite podatke.");
    }
  };

  return (
    <div className="flex justify-center items-center h-screen bg-gray-100">
      <form onSubmit={handleLogin} className="bg-white p-8 rounded shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center text-gray-700">Prijava</h2>
        <input
          type="email"
          placeholder="Email"
          className="w-full p-3 border border-gray-300 rounded mb-4"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Lozinka"
          className="w-full p-3 border border-gray-300 rounded mb-4"
          value={lozinka}
          onChange={(e) => setLozinka(e.target.value)}
          required
        />
        <button type="submit" className="w-full bg-blue-600 hover:bg-blue-700 text-white py-3 rounded transition">
          Prijavi se
        </button>
      </form>
    </div>
  );
}