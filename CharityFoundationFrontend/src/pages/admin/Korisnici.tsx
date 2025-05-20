import { useEffect, useState } from "react";
import axios from "axios";

interface Korisnik {
  id: number;
  ime: string;
  prezime: string;
  email: string;
  lozinka: string;
  tipKorisnika: number;
}

const tipovi: { [key: number]: string } = {
  0: "Administrator",
  1: "Donator",
  2: "Primalac pomoći",
  3: "Volonter"
};

export default function Korisnici() {
  const [korisnici, setKorisnici] = useState<Korisnik[]>([]);

  useEffect(() => {
    fetchKorisnici();
  }, []);

  const fetchKorisnici = async () => {
    try {
      const res = await axios.get("/api/korisnik");
      setKorisnici(res.data);
    } catch (error) {
      console.error("Greška pri dohvatu korisnika:", error);
    }
  };

  const handleChange = (id: number, field: keyof Korisnik, value: any) => {
    setKorisnici(prev =>
      prev.map(k => (k.id === id ? { ...k, [field]: value } : k))
    );
  };

  const handleSave = async (korisnik: Korisnik) => {
    try {
      await axios.put(`/api/korisnik/${korisnik.id}`, korisnik);
      alert("Korisnik uspješno ažuriran.");
    } catch (error) {
      console.error("Greška pri ažuriranju korisnika:", error);
      alert("Neuspješno ažuriranje.");
    }
  };

  return (
    <div className="p-6 max-w-5xl mx-auto">
      <h2 className="text-2xl font-bold mb-4">Korisnici</h2>
      <table className="w-full table-auto border-collapse border">
        <thead>
          <tr className="bg-gray-100">
            <th className="border p-2">Ime</th>
            <th className="border p-2">Prezime</th>
            <th className="border p-2">Email</th>
            <th className="border p-2">Lozinka</th>
            <th className="border p-2">Tip</th>
            <th className="border p-2">Akcija</th>
          </tr>
        </thead>
        <tbody>
          {korisnici.map(k => (
            <tr key={k.id}>
              <td className="border p-2">
                <input
                  value={k.ime}
                  onChange={e => handleChange(k.id, "ime", e.target.value)}
                  className="w-full border rounded px-1"
                />
              </td>
              <td className="border p-2">
                <input
                  value={k.prezime}
                  onChange={e => handleChange(k.id, "prezime", e.target.value)}
                  className="w-full border rounded px-1"
                />
              </td>
              <td className="border p-2">
                <input
                  value={k.email}
                  onChange={e => handleChange(k.id, "email", e.target.value)}
                  className="w-full border rounded px-1"
                />
              </td>
              <td className="border p-2">
                <input
                  value={k.lozinka}
                  onChange={e => handleChange(k.id, "lozinka", e.target.value)}
                  className="w-full border rounded px-1"
                />
              </td>
              <td className="border p-2">
                <select
                  value={k.tipKorisnika}
                  onChange={e =>
                    handleChange(k.id, "tipKorisnika", Number(e.target.value))
                  }
                  className="w-full border rounded"
                >
                  {Object.entries(tipovi).map(([val, label]) => (
                    <option key={val} value={val}>
                      {label}
                    </option>
                  ))}
                </select>
              </td>
              <td className="border p-2">
                <button
                  onClick={() => handleSave(k)}
                  className="bg-blue-600 text-white px-3 py-1 rounded"
                >
                  Spasi
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
