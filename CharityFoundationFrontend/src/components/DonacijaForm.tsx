import React, { useState } from "react";
import axios from "axios";

interface DonacijaFormProps {
  korisnikId: number;
  onDonacijaAdded: () => void; // callback za osvježenje
}

const DonacijaForm: React.FC<DonacijaFormProps> = ({
  korisnikId,
  onDonacijaAdded,
}) => {
  const [iznos, setIznos] = useState("");
  const [vrstaPomoci, setVrstaPomoci] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      await axios.post("/api/donacija", {
        idKorisnika: korisnikId,
        iznos: parseFloat(iznos),
        vrstaPomoci,
      });

      alert("Donacija uspješno dodana!");
      setIznos("");
      setVrstaPomoci("");
      onDonacijaAdded(); // osvježi listu
    } catch (error) {
      console.error("Greška:", error);
      alert("Greška pri slanju donacije.");
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="space-y-4 p-4 bg-white rounded shadow"
    >
      <div>
        <label className="block font-semibold">Iznos (KM):</label>
        <input
          type="number"
          value={iznos}
          onChange={(e) => setIznos(e.target.value)}
          className="input w-full border p-2"
          required
        />
      </div>
      <div>
        <label className="block font-semibold">Vrsta pomoći:</label>
        <input
          value={vrstaPomoci}
          onChange={(e) => setVrstaPomoci(e.target.value)}
          className="input w-full border p-2"
          required
        />
      </div>
      <button
        type="submit"
        className="btn bg-blue-600 text-white px-4 py-2 rounded"
      >
        Dodaj donaciju
      </button>
    </form>
  );
};

export default DonacijaForm;
