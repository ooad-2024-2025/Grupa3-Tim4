import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../services/auth';

export default function Login() {
  const [email, setEmail] = useState('');
  const [lozinka, setLozinka] = useState('');
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const data = await login(email, lozinka);

    if (data && typeof data !== "boolean") {
      switch (data.ime) {
        case "Admin":
          navigate('/admin');
          break;
        case "Dino":
          navigate('/donator');
          break;
        case "Selma":
          navigate('/primalac');
          break;
        case "Vedad":
          navigate('/volonter');
          break;
        default:
          navigate('/');
      }
    } else {
      alert('Pogrešan email ili lozinka');
    }
  };

  return (
    <div>
      <div className="max-w-md mx-auto p-6">
        <h2 className="text-2xl font-bold mb-4">Prijava</h2>
        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            type="email"
            value={email}
            onChange={e => setEmail(e.target.value)}
            className="w-full border p-2 rounded"
            placeholder="Email"
            required
          />
          <input
            type="password"
            value={lozinka}
            onChange={e => setLozinka(e.target.value)}
            className="w-full border p-2 rounded"
            placeholder="Lozinka"
            required
          />
          <button type="submit" className="bg-pink-600 text-white px-4 py-2 rounded">
            Prijavi se
          </button>
        </form>
      </div>
    </div>
  );
}
