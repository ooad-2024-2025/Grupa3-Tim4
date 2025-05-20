import { useEffect, useState } from "react";
import axios from "axios";
import { useAuth } from "../../services/auth";

interface Zahtjev {
  id: number;
  opis: string;
  datum: string;
  status: number;
}

export default function MojiZahtjevi() {
  const { user } = useAuth();
  const [zahtjevi, setZahtjevi] = useState<Zahtjev[]>([]);

  useEffect(() => {
    if (!user) return;

    axios
      .get(`/api/zahtjev/korisnik/${user.id}`)
      .then((res) => setZahtjevi(res.data))
      .catch(console.error);
  }, [user]);

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

  return (
    <div className="p-6 max-w-4xl mx-auto">
      <h2 className="text-2xl font-semibold mb-4">Moji Zahtjevi za Pomoć</h2>

      {zahtjevi.length === 0 ? (
        <p className="text-gray-600">Nemate aktivnih zahtjeva.</p>
      ) : (
        <ul className="space-y-4">
          {zahtjevi.map((z) => (
            <li key={z.id} className="p-4 bg-white shadow rounded border">
              <p><strong>Opis:</strong> {z.opis}</p>
              <p><strong>Status:</strong> {getStatusLabel(z.status)}</p>
              <p><strong>Datum:</strong> {new Date(z.datum).toLocaleDateString()}</p>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
