import { Header } from '../components/Header';
import { Footer } from '../components/Footer';

export default function DonatorDashboard() {
  return (
    <div>
      <section className="p-6 max-w-4xl mx-auto">
        <h2 className="text-3xl font-bold mb-4 text-pink-600">Dobrodošli, Donatore!</h2>
        <p className="mb-4">Možete pregledati i dodati nove donacije, te vidjeti notifikacije.</p>
        <ul className="list-disc pl-6 text-gray-700 space-y-2">
          <li>Pregled svih vaših donacija</li>
          <li>Dodavanje nove donacije</li>
          <li>Primanje obavijesti o realizaciji</li>
        </ul>
      </section>
    </div>
  );
}