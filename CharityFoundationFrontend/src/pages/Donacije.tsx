import { useEffect, useState } from 'react';
import { useAuth } from '../services/auth';

export default function Donacije() {
  const [donacije, setDonacije] = useState([]);
  const { user } = useAuth();

  useEffect(() => {
    if (!user) return;

    fetch(`http://localhost:5000/api/donacija/korisnik/${user.id}`)
      .then(res => res.json())
      .then(setDonacije)
      .catch(console.error);
  }, [user]);

  return (
    <div>
      <h2 className="text-2xl font-semibold p-6">Moje Donacije</h2>
      <ul className="max-w-3xl mx-auto space-y-2">
        {donacije.map((d: any) => (
          <li key={d.id} className="p-4 bg-white shadow rounded">
            <p><strong>{d.vrstaPomoci}</strong></p>
            <p>Iznos: {d.iznos} KM</p>
            <p>Status: {d.status === 1 ? 'Odobreno' : 'Na čekanju'}</p>
            <p>Datum: {new Date(d.datumDonacije).toLocaleString()}</p>
          </li>
        ))}
      </ul>
    </div>
  );
}
