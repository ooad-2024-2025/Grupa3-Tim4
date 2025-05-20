import { useEffect, useState } from "react";
import axios from "axios";

interface Izvjestaj {
  id: number;
  korisnikId: number;
  opis: string;
  datum: string;
}

export default function Izvjestaji() {
  // const [izvjestaji, setIzvjestaji] = useState<Izvjestaj[]>([]);
  const [uniqueUsers, setUniqueUsers] = useState<number[]>([]);

  useEffect(() => {
    axios.get("/api/izvjestaj")
      .then(res => {
        // setIzvjestaji(res.data);
        const korisnici: number[] = Array.from(new Set(
          res.data.map((i: Izvjestaj) => i.korisnikId)
        ));
        setUniqueUsers(korisnici);
      })
      .catch(console.error);
  }, []);

  const downloadPDF = async (userId?: number) => {
    const url = userId !== undefined
      ? `/api/izvjestaj/pdf/korisnik/${userId}`
      : "/api/izvjestaj/pdf";

    try {
      const res = await axios.get(url, { responseType: "blob" });
      const blob = new Blob([res.data], { type: "application/octet-stream" });
      const link = document.createElement("a");
      link.href = window.URL.createObjectURL(blob);
      link.download = userId !== undefined
        ? `Izvjestaj_Korisnik_${userId}.txt`
        : "Izvjestaji_Svi.txt";
      link.click();
    } catch (err) {
      console.error("Greška pri generisanju izvještaja:", err);
    }
  };

  return (
    <div>
      <div className="p-6 max-w-5xl mx-auto space-y-6">
        <h2 className="text-2xl font-semibold">Izvještaji o donacijama i pomoći</h2>

        <button
          onClick={() => downloadPDF()}
          className="bg-blue-600 text-white px-4 py-2 rounded"
        >
          Preuzmi sve izvještaje (.txt)
        </button>

        <h3 className="text-xl font-bold mt-6">Izvještaji po korisnicima</h3>
        <ul className="list-disc pl-5">
          {uniqueUsers.map(uid => (
            <li key={uid} className="mt-2">
              <button
                onClick={() => downloadPDF(uid)}
                className="bg-gray-700 text-white px-3 py-1 rounded"
              >
                Izvještaji za korisnika {uid}
              </button>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}
