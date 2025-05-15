import { Link } from 'react-router-dom';
import { useAuth } from '../services/auth';

export function Header() {
  const { user, logout } = useAuth();

  return (
    <header className="bg-white shadow-md p-4 flex justify-between items-center sticky top-0 z-50">
      <h1 className="text-xl font-bold text-pink-600">Charity Foundation</h1>
      <nav className="space-x-4">
        <Link to="/" className="hover:text-pink-600">Početna</Link>
        {user && user.tip === 1 && <Link to="/donacije" className="hover:text-pink-600">Moje Donacije</Link>}
        {user && user.tip === 2 && <Link to="/zahtjevi" className="hover:text-pink-600">Moji Zahtjevi</Link>}
        {user && user.tip === 3 && <Link to="/akcije" className="hover:text-pink-600">Moje Akcije</Link>}
        <Link to="/kontakt" className="hover:text-pink-600">Kontakt</Link>
        {user ? (
          <button onClick={logout} className="bg-pink-500 text-white px-3 py-1 rounded">Odjava</button>
        ) : (
          <Link to="/login" className="bg-pink-500 text-white px-3 py-1 rounded">Prijava</Link>
        )}
      </nav>
    </header>
  );
}