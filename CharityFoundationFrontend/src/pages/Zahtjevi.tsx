import { useEffect, useState } from 'react';
import { useAuth } from '../services/auth';

export default function Zahjevi() {
  const [zahtjevi, setZahtjevi] = useState([]);
  const { user } = useAuth();

  useEffect(() => {
    if (!user) return;

    fetch(`http://localhost:5000/api/zahtjev/korisnik/${user.id}`)
      .then(res => res.json())
      .then(setZahtjevi)
      .catch(console.error);
  }, [user]);

  return (
    <div>
      <h2 className="text-2xl font-semibold p-6">Moji Zahtjevi</h2>
      <ul className="max-w-3xl mx-auto space-y-2">
  {zahtjevi.map((z: any) => (
    <li key={z.id} className="p-4 bg-white shadow rounded">
      <p><strong>Opis potrebe:</strong> {z.opisPotrebe}</p>
      <p>Urgentnost: {z.urgentnost === 1 ? 'Srednje' : z.urgentnost === 2 ? 'Visoka' : 'Niska'}</p>
      <p>Status: {z.status === 1 ? 'Odobreno' : 'Na čekanju'}</p>
    </li>
  ))}
</ul>

    </div>
  );
}
