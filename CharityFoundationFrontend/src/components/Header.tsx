import { Link } from 'react-router-dom';
import { useAuth } from '../services/auth';

export function Header() {
  const { user, logout } = useAuth();

  return (
    <header className="bg-white shadow-md p-4 flex justify-between items-center sticky top-0 z-50">
      <h1 className="text-xl font-bold text-pink-600">
        Charity Foundation
      </h1>

      <nav className="flex items-center space-x-4">
        <Link to="/" className="hover:text-pink-600">Početna</Link>

        {/* ADMIN */}
        {user?.uloga === 'Administrator' && (
          <>
            <Link to="/admin" className="hover:text-pink-600">Dashboard</Link>
            <Link to="/admin/korisnici" className="hover:text-pink-600">Korisnici</Link>
            <Link to="/admin/donacije" className="hover:text-pink-600">Donacije</Link>
            <Link to="/admin/akcije" className="hover:text-pink-600">Akcije</Link>
            <Link to="/admin/izvjestaji" className="hover:text-pink-600">Izvještaji</Link>
          </>
        )}

        {/* DONATOR */}
        {user?.uloga === 'Donator' && (
          <>
            <Link to="/donator" className="hover:text-pink-600">Dashboard</Link>
            <Link to="/donator/mojedonacije" className="hover:text-pink-600">Moje Donacije</Link>
            <Link to="/donator/novadonacija" className="hover:text-pink-600">Nova Donacija</Link>
          </>
        )}

        {/* PRIMALAC */}
        {user?.uloga === 'PrimalacPomoci' && (
          <>
            <Link to="/primalac" className="hover:text-pink-600">Dashboard</Link>
            <Link to="/primalac/mojizahtjevi" className="hover:text-pink-600">Moji Zahtjevi</Link>
            <Link to="/primalac/novizahtjev" className="hover:text-pink-600">Novi Zahtjev</Link>
          </>
        )}

        {/* VOLONTER */}
        {user?.uloga === 'Volonter' && (
          <>
            <Link to="/volonter" className="hover:text-pink-600">Dashboard</Link>
            <Link to="/volonter/mojeakcije" className="hover:text-pink-600">Moje Akcije</Link>
          </>
        )}

        <Link to="/kontakt" className="hover:text-pink-600">Kontakt</Link>

        {/* Auth dugmad */}
        {user ? (
          <>
            <span className="text-gray-600">{user.ime}</span>
            <button
              onClick={logout}
              className="bg-pink-500 text-white px-3 py-1 rounded hover:bg-pink-600"
            >
              Odjava
            </button>
          </>
        ) : (
          <Link
            to="/login"
            className="bg-pink-500 text-white px-3 py-1 rounded hover:bg-pink-600"
          >
            Prijava
          </Link>
        )}
      </nav>
    </header>
  );
}
