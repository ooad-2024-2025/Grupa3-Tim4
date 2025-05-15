import { Navigate } from 'react-router-dom';
import { useAuth } from '../services/auth';

export function ProtectedRoute({ children, allowed }: { children: JSX.Element, allowed: number[] }) {
  const { user, isLoading } = useAuth();

  if (isLoading) return null; // ⏳ čeka učitavanje
  if (!user) return <Navigate to="/login" replace />;
  if (!allowed.includes(user.tip)) return <Navigate to="/" replace />;

  return children;
}
