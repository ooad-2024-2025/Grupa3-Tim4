import { useEffect, useState } from "react";
import axios from "axios";

interface Zahtjev {
  id: number;
  idKorisnika: number;
  opis: string;
  datum: string;
  status: number;
  urgentnost: number;
}

export default function Zahtjevi() {
  const [zahtjevi, setZahtjevi] = useState<Zahtjev[]>([]);

  useEffect(() => {
    axios.get("/api/zahtjev")
      .then(res => setZahtjevi(res.data))
      .catch(console.error);
  }, []);

  const getStatusLabel = (status: number) => {
    switch (status) {
      case 0:
        return "Na čekanju";
      case 1:
        return "Odobren";
      case 2:
        return "Odbijen";
      default:
        return "Nepoznat";
    }
  };

  const getUrgentnost = (level: number) => {
    switch (level) {
      case 2: return "Visoka";
      case 1: return "Srednja";
      case 0: default: return "Niska";
    }
  };

  const updateStatus = async (id: number, status: number) => {
    try {
      await axios.put(`/api/zahtjev/${id}/status`, { status });
      setZahtjevi(prev =>
        prev.map(z => z.id === id ? { ...z, status } : z)
      );
    } catch (error) {
      console.error("Greška pri ažuriranju statusa:", error);
    }
  };

  return (
    <div className="p-6 max-w-5xl mx-auto">
      <h2 className="text-2xl font-semibold mb-4">Svi Zahtjevi za Pomoć</h2>

      {zahtjevi.length === 0 ? (
        <p>Nema zahtjeva.</p>
      ) : (
        <ul className="space-y-4">
          {zahtjevi.map((z) => (
            <li key={z.id} className="p-4 bg-white shadow rounded border">
              <p><strong>Korisnik ID:</strong> {z.idKorisnika}</p>
              <p><strong>Opis:</strong> {z.opis}</p>
              <p><strong>Urgentnost:</strong> {getUrgentnost(z.urgentnost)}</p>
              <p><strong>Status:</strong> {getStatusLabel(z.status)}</p>
              <p><strong>Datum:</strong> {new Date(z.datum).toLocaleDateString()}</p>

              {z.status === 0 && (
                <div className="mt-2 space-x-2">
                  <button
                    onClick={() => updateStatus(z.id, 1)}
                    className="bg-green-600 text-white px-3 py-1 rounded"
                  >
                    Odobri
                  </button>
                  <button
                    onClick={() => updateStatus(z.id, 2)}
                    className="bg-red-600 text-white px-3 py-1 rounded"
                  >
                    Odbij
                  </button>
                </div>
              )}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
