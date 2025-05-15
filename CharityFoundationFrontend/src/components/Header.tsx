export default function Header() {
  return (
    <header className="bg-white shadow p-4">
      <div className="container mx-auto flex justify-between items-center">
        <h1 className="text-2xl font-bold">Charity Foundation</h1>
        <nav className="space-x-4">
          <a href="/" className="hover:underline">Početna</a>
          <a href="/donacije" className="hover:underline">Donacije</a>
          <a href="/zahtjevi" className="hover:underline">Zahtjevi</a>
          <a href="/izvjestaji" className="hover:underline">Izvještaji</a>
          <a href="/kontakt" className="hover:underline">Kontakt</a>
          <a href="/login" className="hover:underline">Login</a>
<a href="/register" className="hover:underline">Register</a>

        </nav>
      </div>
    </header>
  );
}
