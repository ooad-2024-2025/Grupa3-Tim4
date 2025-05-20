import { useEffect, useState } from "react";
import axios from "axios";
import { useAuth } from "../../services/auth";

interface Akcija {
  naziv: string;
  opis: string;
  datum: string;
}

interface VolonterAkcija {
  id: number;
  akcijaID: number;
  statusUcesca: number;
  akcija: Akcija;
}

export default function MojeAkcije() {
  const { user } = useAuth();
  const [akcije, setAkcije] = useState<VolonterAkcija[]>([]);

  useEffect(() => {
    if (!user) return;

    axios
      .get(`/api/volonterakcija/volonter/${user.id}`)
      .then((res) => setAkcije(res.data))
      .catch(console.error);
  }, [user]);

  const getStatus = (status: number) => {
    switch (status) {
      case 0: return "Prijavljen";
      case 1: return "Potvrđen";
      case 2: return "Otkazao";
      case 3: return "Prisustvovao";
      default: return "Nepoznat";
    }
  };

  return (
    <div className="p-6 max-w-4xl mx-auto">
      <h2 className="text-2xl font-semibold mb-4">Moje Akcije</h2>
      {akcije.length === 0 ? (
        <p className="text-gray-600">Niste prijavljeni ni na jednu akciju.</p>
      ) : (
        <ul className="space-y-4">
          {akcije.map((va) => (
            <li key={va.id} className="p-4 bg-white shadow rounded border">
              <p><strong>Naziv:</strong> {va.akcija.naziv}</p>
              <p><strong>Opis:</strong> {va.akcija.opis}</p>
              <p><strong>Datum:</strong> {new Date(va.akcija.datum).toLocaleDateString()}</p>
              <p><strong>Status:</strong> {getStatus(va.statusUcesca)}</p>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
