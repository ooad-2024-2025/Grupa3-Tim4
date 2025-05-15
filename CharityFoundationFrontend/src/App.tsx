// === src/App.tsx ===
import {
  BrowserRouter as Router,
  Routes,
  Route
} from 'react-router-dom';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import Kontakt from './pages/Kontakt';
import Zahtjevi from './pages/Zahtjevi';
import Akcije from './pages/Akcije';
import AdminDashboard from './pages/AdminDashboard';
import DonatorDashboard from './pages/DonatorDashboard';
import PrimalacDashboard from './pages/PrimalacDashboard';
import VolonterDashboard from './pages/VolonterDashboard';
import Donacije from './pages/Donacije';
import { ProtectedRoute } from './routes/ProtectedRoute';
import { Header } from './components/Header';
import { Footer } from './components/Footer';

export default function App() {
  return (
    <Router>
      <Header />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/kontakt" element={<Kontakt />} />

        <Route
          path="/donacije"
          element={
            <ProtectedRoute allowed={[1]}>
              <Donacije />
            </ProtectedRoute>
          }
        />

        <Route
  path="/zahtjevi"
  element={
    <ProtectedRoute allowed={[2]}>
      <Zahtjevi />
    </ProtectedRoute>
  }
/>

        <Route
  path="/akcije"
  element={
    <ProtectedRoute allowed={[3]}>
      <Akcije />
    </ProtectedRoute>
  }
/>


        <Route
          path="/admin"
          element={
            <ProtectedRoute allowed={[0]}>
              <AdminDashboard />
            </ProtectedRoute>
          }
        />

        <Route
          path="/donator"
          element={
            <ProtectedRoute allowed={[1]}>
              <DonatorDashboard />
            </ProtectedRoute>
          }
        />

        <Route
          path="/primalac"
          element={
            <ProtectedRoute allowed={[2]}>
              <PrimalacDashboard />
            </ProtectedRoute>
          }
        />

        <Route
          path="/volonter"
          element={
            <ProtectedRoute allowed={[3]}>
              <VolonterDashboard />
            </ProtectedRoute>
          }
        />
      </Routes>
      <Footer />
    </Router>
  );
}