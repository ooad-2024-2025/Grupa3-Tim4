import { useEffect, useState } from "react";
import axios from "axios";

interface Akcija {
  id: number;
  naziv: string;
  opis: string;
  datum: string;
}

export default function Akcije() {
  const [akcije, setAkcije] = useState<Akcija[]>([]);
  const [naziv, setNaziv] = useState("");
  const [opis, setOpis] = useState("");
  const [poruka, setPoruka] = useState("");

  const fetchAkcije = () => {
    axios.get("/api/akcija", {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`
      }
    })
      .then(res => setAkcije(res.data))
      .catch(console.error);
  };

  useEffect(() => {
    fetchAkcije();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.post("/api/akcija", { naziv, opis });
      setPoruka("Akcija uspješno dodana.");
      setNaziv("");
      setOpis("");
      fetchAkcije();
    } catch (err) {
      console.error(err);
      setPoruka("Greška pri dodavanju.");
    }
  };

  return (
    <div className="p-6 max-w-5xl mx-auto space-y-6">
      <h2 className="text-2xl font-semibold">Volonterske Akcije</h2>

      <form onSubmit={handleSubmit} className="space-y-4 border p-4 rounded shadow">
        <h3 className="text-xl font-semibold">Dodaj novu akciju</h3>
        {poruka && <p className="text-blue-600">{poruka}</p>}
        <div>
          <label className="block font-medium mb-1">Naziv akcije:</label>
          <input
            value={naziv}
            onChange={(e) => setNaziv(e.target.value)}
            required
            className="w-full border rounded p-2"
          />
        </div>
        <div>
          <label className="block font-medium mb-1">Opis:</label>
          <textarea
            value={opis}
            onChange={(e) => setOpis(e.target.value)}
            required
            className="w-full border rounded p-2"
            rows={3}
          />
        </div>
        <button type="submit" className="bg-blue-600 text-white px-4 py-2 rounded">
          Dodaj
        </button>
      </form>

      <div>
        <h3 className="text-xl font-semibold mb-3">Lista svih akcija</h3>
        <ul className="space-y-4">
          {akcije.map((a) => (
            <li key={a.id} className="p-4 bg-white border rounded shadow">
              <p><strong>Naziv:</strong> {a.naziv}</p>
              <p><strong>Opis:</strong> {a.opis}</p>
              <p><strong>Datum:</strong> {new Date(a.datum).toLocaleDateString()}</p>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}
