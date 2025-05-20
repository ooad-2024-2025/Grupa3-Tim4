export default function VolonterDashboard() {
  return (
    <div>
      <section className="p-6 max-w-4xl mx-auto">
        <h2 className="text-3xl font-bold mb-4 text-pink-600">Dobrodošli, Volonteru!</h2>
        <p className="mb-4">Pregled dostupnih akcija i prijava na iste.</p>
        <ul className="list-disc pl-6 text-gray-700 space-y-2">
          <li>Pregled svih aktivnih akcija</li>
          <li>Prijava na volontiranje</li>
          <li>Status prijava i povratne informacije</li>
        </ul>
      </section>
    </div>
  );
}