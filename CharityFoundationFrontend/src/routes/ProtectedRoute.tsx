import { Navigate } from "react-router-dom";
import { useAuth } from "../services/auth";

type Props = {
  children: React.ReactNode;
  allowed: string[]; // npr. ["Administrator"]
};

export const ProtectedRoute = ({ children, allowed }: Props) => {
  const { user, isLoading } = useAuth();

  if (isLoading) return <div>Učitavanje...</div>;

  if (!user) return <Navigate to="/login" replace />;

  if (!allowed.includes(user.uloga)) {
    return <Navigate to="/" replace />;
  }

  return <>{children}</>;
};
