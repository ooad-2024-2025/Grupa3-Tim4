import { useState } from 'react';
import { useAuth } from '../services/auth';

export function DonacijaForm() {
  const [opis, setOpis] = useState('');
  const { user } = useAuth();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await fetch('/api/donacije', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ opis, korisnikId: user?.id })
    });
    setOpis('');
    alert('Donacija poslana.');
  };

  return (
    <div className="p-6 max-w-xl mx-auto">
      <Header />
      <h2 className="text-2xl font-semibold mb-4">Nova Donacija</h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <textarea
          value={opis}
          onChange={e => setOpis(e.target.value)}
          className="w-full border p-2 rounded"
          rows={4}
          placeholder="Unesite opis donacije"
          required
        />
        <button type="submit" className="bg-pink-600 text-white px-4 py-2 rounded">Pošalji</button>
      </form>
      <Footer />
    </div>
  );
}