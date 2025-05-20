import { useState } from "react";
import axios from "axios";
import { useAuth } from "../../services/auth";

export default function NoviZahtjev() {
  const { user } = useAuth();
  const [opis, setOpis] = useState("");
  const [poruka, setPoruka] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      await axios.post("/api/zahtjev", {
        korisnikId: user?.id,
        opis,
      });

      setPoruka("Zahtjev uspješno poslan.");
      setOpis("");
    } catch (error) {
      console.error("Greška pri slanju zahtjeva:", error);
      setPoruka("Došlo je do greške.");
    }
  };

  return (
    <div className="p-6 max-w-2xl mx-auto">
      <h2 className="text-2xl font-semibold mb-4">Novi Zahtjev za Pomoć</h2>

      {poruka && <p className="mb-4 text-blue-600 font-medium">{poruka}</p>}

      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block font-semibold mb-1">Opis pomoći koju tražite:</label>
          <textarea
            value={opis}
            onChange={(e) => setOpis(e.target.value)}
            required
            className="w-full border p-2 rounded"
            rows={4}
          />
        </div>
        <button
          type="submit"
          className="bg-blue-600 text-white px-4 py-2 rounded"
        >
          Pošalji Zahtjev
        </button>
      </form>
    </div>
  );
}
