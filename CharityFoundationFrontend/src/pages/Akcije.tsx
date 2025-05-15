import { useEffect, useState } from 'react';
import { useAuth } from '../services/auth';

type Akcija = {
  id: number;
  naziv: string;
  opis: string;
  datum: string;
};

export default function Akcije() {
  const [akcije, setAkcije] = useState<Akcija[]>([]);
  const { user } = useAuth();

  useEffect(() => {
    if (!user) return;

    fetch(`http://localhost:5000/api/akcija/korisnik/${user.id}`)
      .then(res => res.json())
      .then(setAkcije)
      .catch(console.error);
  }, [user]);

  return (
    <div>
      <h2 className="text-2xl font-bold p-6">Moje Dodijeljene Akcije</h2>
      <div className="max-w-4xl mx-auto space-y-4 p-4">
        {akcije.map(akcija => (
          <div key={akcija.id} className="bg-white shadow-md rounded p-4">
            <h3 className="text-xl font-semibold">{akcija.naziv}</h3>
            <p className="mt-2">Opis: {akcija.opis}</p>
            <p className="mt-2 text-sm text-gray-600">
              Datum: {new Date(akcija.datum).toLocaleDateString()}
            </p>
          </div>
        ))}
      </div>
    </div>
  );
}
