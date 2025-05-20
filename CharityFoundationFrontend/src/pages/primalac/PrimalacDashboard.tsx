export default function PrimalacDashboard() {
  return (
    <div>
      <section className="p-6 max-w-4xl mx-auto">
        <h2 className="text-3xl font-bold mb-4 text-pink-600">
          Dobrodošli, Primalac pomoći!
        </h2>
        <p className="mb-4">
          Možete podnijeti zahtjev za pomoć i pratiti status istog.
        </p>
        <ul className="list-disc pl-6 text-gray-700 space-y-2">
          <li>Pregled humanitarnih akcija</li>
          <li>Podnošenje zahtjeva za pomoć</li>
          <li>Praćenje statusa i odgovora</li>
        </ul>
      </section>
    </div>
  );
}
