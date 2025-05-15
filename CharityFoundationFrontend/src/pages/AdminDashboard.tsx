import { Header } from '../components/Header';
import { Footer } from '../components/Footer';

export default function AdminDashboard() {
  return (
    <div>
      <Header />
      <section className="p-6 max-w-4xl mx-auto">
        <h2 className="text-3xl font-bold mb-4 text-pink-600">Administrator Panel</h2>
        <p className="mb-4">Upravljajte korisnicima, pregledajte sve donacije i zahtjeve, te generišite izvještaje.</p>
        <ul className="list-disc pl-6 text-gray-700 space-y-2">
          <li>Pregled i uređivanje korisnika</li>
          <li>Uvid u sve donacije i zahtjeve</li>
          <li>Statistički izvještaji i analiza</li>
        </ul>
      </section>
      <Footer />
    </div>
  );
}