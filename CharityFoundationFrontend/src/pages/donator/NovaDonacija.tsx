import { useState } from "react";
import axios from "axios";
import { useAuth } from "../../services/auth";

export default function NovaDonacija() {
  const { user } = useAuth();
  const [iznos, setIznos] = useState("");
  const [vrstaPomoci, setVrstaPomoci] = useState("");
  const [poruka, setPoruka] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      await axios.post("/api/donacija", {
        idKorisnika: user?.id,
        iznos: parseFloat(iznos),
        vrstaPomoci,
      });

      setPoruka("Donacija uspješno dodana.");
      setIznos("");
      setVrstaPomoci("");
    } catch (error) {
      console.error("Greška pri dodavanju donacije:", error);
      setPoruka("Došlo je do greške.");
    }
  };

  return (
    <div className="p-6 max-w-2xl mx-auto">
      <h2 className="text-2xl font-semibold mb-4">Nova Donacija</h2>
      {poruka && (
        <p className="mb-4 text-blue-600 font-medium">{poruka}</p>
      )}
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block font-semibold mb-1">Iznos (KM):</label>
          <input
            type="number"
            value={iznos}
            onChange={(e) => setIznos(e.target.value)}
            required
            className="w-full border p-2 rounded"
          />
        </div>
        <div>
          <label className="block font-semibold mb-1">Vrsta pomoći:</label>
          <input
            type="text"
            value={vrstaPomoci}
            onChange={(e) => setVrstaPomoci(e.target.value)}
            required
            className="w-full border p-2 rounded"
          />
        </div>
        <button
          type="submit"
          className="bg-blue-600 text-white px-4 py-2 rounded"
        >
          Pošalji Donaciju
        </button>
      </form>
    </div>
  );
}
