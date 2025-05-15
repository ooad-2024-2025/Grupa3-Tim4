import { useEffect, useState } from 'react';

export function NotifikacijaList() {
  const [notifikacije, setNotifikacije] = useState([]);

  useEffect(() => {
    fetch('/api/notifikacije')
      .then(res => res.json())
      .then(setNotifikacije);
  }, []);

  return (
    <div className="p-6 max-w-4xl mx-auto">
      <Header />
      <h2 className="text-2xl font-semibold mb-4">Notifikacije</h2>
      <ul className="space-y-2">
        {notifikacije.map((n: any) => (
          <li key={n.id} className="bg-gray-100 p-4 rounded shadow">
            {n.sadrzaj}
          </li>
        ))}
      </ul>
      <Footer />
    </div>
  );
}
