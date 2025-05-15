import { createContext, useContext, useEffect, useState } from 'react';

type User = {
  id: number;
  ime: string;
  tip: number;
};

type AuthContextType = {
  user: User | null;
  isLoading: boolean;
  login: (email: string, lozinka: string) => Promise<User | null>;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType>({
  user: null,
  isLoading: true,
  login: async () => null,
  logout: () => {},
});

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const stored = localStorage.getItem('user');
    if (stored) setUser(JSON.parse(stored));
    setIsLoading(false);
  }, []);

  const login = async (email: string, lozinka: string) => {
    const res = await fetch('http://localhost:5000/api/korisnik/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, lozinka }),
    });

    if (res.ok) {
      const data: User = await res.json();
      setUser(data);
      localStorage.setItem('user', JSON.stringify(data));
      return data;
    }

    return null;
  };

  const logout = () => {
    setUser(null);
    localStorage.removeItem('user');
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
