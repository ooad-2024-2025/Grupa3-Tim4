import { createContext, useContext, useEffect, useState } from "react";
import axios from 'axios';

type User = {
  id: number;
  ime: string;
  prezime: string;
  email: string;
  uloga: string;
};

type AuthContextType = {
  user: User | null;
  isLoading: boolean;
  token: string | null;
  login: (email: string, lozinka: string) => Promise<User | boolean>;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType>({
  user: null,
  isLoading: true,
  token: null,
  login: async () => false,
  logout: () => {},
});

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const storedUser = localStorage.getItem("user");
    const storedToken = localStorage.getItem("token");

    if (storedUser && storedToken) {
      setUser(JSON.parse(storedUser));
      setToken(storedToken);
    }

    setIsLoading(false);
  }, []);

  const login = async (email: string, lozinka: string): Promise<User | false> => {
    const res = await fetch("http://localhost:5000/api/korisnik/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, lozinka }),
    });

    if (res.ok) {
      const data = await res.json();
      setUser(data.korisnik);
      setToken(data.token);

      localStorage.setItem("user", JSON.stringify(data.korisnik));
      localStorage.setItem("token", data.token);
      
      axios.defaults.headers.common['Authorization'] = `Bearer ${data.token}`;
      return data.korisnik;
    }

    return false;
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    localStorage.removeItem("user");
    localStorage.removeItem("token");
  };

  return (
    <AuthContext.Provider value={{ user, token, login, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
