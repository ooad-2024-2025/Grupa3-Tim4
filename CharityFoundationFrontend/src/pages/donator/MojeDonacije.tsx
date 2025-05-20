import { useEffect, useState } from "react";
import axios from "axios";
import { useAuth } from "../../services/auth";

interface Donacija {
  id: number;
  iznos: number;
  vrstaPomoci: string;
  status: number;
  datumDonacije: string;
}

export default function MojeDonacije() {
  const [donacije, setDonacije] = useState<Donacija[]>([]);
  const { user } = useAuth();

  useEffect(() => {
    if (!user) return;

    fetch(`http://localhost:5000/api/donacija/korisnik/${user.id}`, {
      method: 'GET', // or 'POST', 'PUT', etc.
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem("token")}`, // add the JWT token here
      }})
      .then((res) => res.json())
      .then(setDonacije)
      .catch(console.error);
  }, [user]);

  const getStatusLabel = (status: number) => {
    switch (status) {
      case 0:
        return "Na čekanju";
      case 1:
        return "Odobreno";
      case 2:
        return "Odbijeno";
      case 3:
        return "Isporučeno";
      default:
        return "Nepoznat";
    }
  };

  return (
    <div className="p-6">
      <h2 className="text-2xl font-semibold mb-4">Moje Donacije</h2>
      {donacije === null || donacije.length === 0 ? (
        <p>Nema donacija.</p>
      ) : (
        <ul className="space-y-3 max-w-3xl mx-auto">
          {donacije.map((d) => (
            <li key={d.id} className="p-4 bg-white shadow rounded border">
              <p><strong>Vrsta pomoći:</strong> {d.vrstaPomoci}</p>
              <p><strong>Iznos:</strong> {d.iznos} KM</p>
              <p><strong>Status:</strong> {getStatusLabel(d.status)}</p>
              <p><strong>Datum:</strong> {new Date(d.datumDonacije).toLocaleString()}</p>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
