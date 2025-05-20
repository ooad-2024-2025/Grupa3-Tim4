import { useEffect, useState } from "react";
import axios from "axios";

interface Donacija {
  id: number;
  idKorisnika: number;
  iznos: number;
  vrstaPomoci: string;
  status: number;
  datumDonacije: string;
}

const statusLabel: { [key: number]: string } = {
  0: "Na čekanju",
  1: "Prihvaćena",
  2: "Odbijena",
  3: "Isporučena"
};

export default function Donacije() {
  const [donacije, setDonacije] = useState<Donacija[]>([]);

  useEffect(() => {
    axios.get("/api/donacija")
      .then(res => setDonacije(res.data))
      .catch(console.error);
  }, []);

  const updateStatus = async (id: number, status: number) => {
    try {
      await axios.put(`/api/donacija/${id}/status`, { status });
      setDonacije(prev =>
        prev.map(d => d.id === id ? { ...d, status } : d)
      );
    } catch (error) {
      console.error("Greška pri ažuriranju statusa:", error);
    }
  };

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <h2 className="text-2xl font-bold mb-4">Sve Donacije</h2>
      <ul className="space-y-3">
        {donacije.map(d => (
          <li key={d.id} className="p-4 bg-white rounded shadow border">
            <p><strong>Korisnik ID:</strong> {d.idKorisnika}</p>
            <p><strong>Vrsta pomoći:</strong> {d.vrstaPomoci}</p>
            <p><strong>Iznos:</strong> {d.iznos} KM</p>
            <p><strong>Status:</strong> {statusLabel[d.status]}</p>
            <p><strong>Datum:</strong> {new Date(d.datumDonacije).toLocaleString()}</p>

            {d.status === 0 && (
              <div className="mt-2 space-x-2">
                <button
                  onClick={() => updateStatus(d.id, 1)}
                  className="px-3 py-1 bg-green-600 text-white rounded"
                >
                  Prihvati
                </button>
                <button
                  onClick={() => updateStatus(d.id, 2)}
                  className="px-3 py-1 bg-red-600 text-white rounded"
                >
                  Odbij
                </button>
              </div>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}
