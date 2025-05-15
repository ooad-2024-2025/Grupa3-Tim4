import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";

export default function Register() {
  const [form, setForm] = useState({
    ime: "",
    prezime: "",
    email: "",
    lozinka: "",
    tipKorisnika: "donator"
  });
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api.post("/korisnik/register", form);
      navigate("/login");
    } catch {
      alert("Greška pri registraciji.");
    }
  };

  return (
    <div className="flex justify-center items-center h-screen bg-gray-100">
      <form onSubmit={handleSubmit} className="bg-white p-8 rounded shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center text-gray-700">Registracija</h2>
        <input name="ime" placeholder="Ime" className="w-full p-3 border rounded mb-4" value={form.ime} onChange={handleChange} required />
        <input name="prezime" placeholder="Prezime" className="w-full p-3 border rounded mb-4" value={form.prezime} onChange={handleChange} required />
        <input name="email" type="email" placeholder="Email" className="w-full p-3 border rounded mb-4" value={form.email} onChange={handleChange} required />
        <input name="lozinka" type="password" placeholder="Lozinka" className="w-full p-3 border rounded mb-4" value={form.lozinka} onChange={handleChange} required />
        <select name="tipKorisnika" className="w-full p-3 border rounded mb-6" value={form.tipKorisnika} onChange={handleChange}>
          <option value="donator">Donator</option>
          <option value="volonter">Volonter</option>
          <option value="primalacPomoci">Primalac pomoći</option>
        </select>
        <button type="submit" className="w-full bg-green-600 hover:bg-green-700 text-white py-3 rounded transition">
          Registruj se
        </button>
      </form>
    </div>
  );
}