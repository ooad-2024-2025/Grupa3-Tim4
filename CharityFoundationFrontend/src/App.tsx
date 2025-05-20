import {
  BrowserRouter as Router,
  Routes,
  Route
} from 'react-router-dom';

import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import Kontakt from './pages/Kontakt';

import { ProtectedRoute } from './routes/ProtectedRoute';
import { Header } from './components/Header';
import { Footer } from './components/Footer';
import { AuthProvider } from './services/auth';

// Admin
import AdminDashboard from './pages/admin/AdminDashboard';
import Korisnici from './pages/admin/Korisnici';
import Donacije from './pages/admin/Donacije';
import Zahtjevi from './pages/admin/Zahtjevi';
import Akcije from './pages/admin/Akcije';
import Izvjestaji from './pages/admin/Izvjestaji';

// Donator
import DonatorDashboard from './pages/donator/DonatorDashboard';
import NovaDonacija from './pages/donator/NovaDonacija';
import MojeDonacije from './pages/donator/MojeDonacije';

// Primalac
import PrimalacDashboard from './pages/primalac/PrimalacDashboard';
import NoviZahtjev from './pages/primalac/NoviZahtjev';
import MojiZahtjevi from './pages/primalac/MojiZahtjevi';

// Volonter
import VolonterDashboard from './pages/volonter/VolonterDashboard';
import MojeAkcije from './pages/volonter/MojeAkcije';

export default function App() {
  return (
    <Router>
      <AuthProvider>
        <Header />
        <Routes>
          {/* Javne rute */}
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/kontakt" element={<Kontakt />} />

          {/* ADMIN */}
          <Route
            path="/admin"
            element={
              <ProtectedRoute allowed={["Administrator"]}>
                <AdminDashboard />
              </ProtectedRoute>
            }
          />
          <Route
            path="/admin/korisnici"
            element={
              <ProtectedRoute allowed={["Administrator"]}>
                <Korisnici />
              </ProtectedRoute>
            }
          />
          <Route
            path="/admin/donacije"
            element={
              <ProtectedRoute allowed={["Administrator"]}>
                <Donacije />
              </ProtectedRoute>
            }
          />
          <Route
            path="/admin/zahtjevi"
            element={
              <ProtectedRoute allowed={["Administrator"]}>
                <Zahtjevi />
              </ProtectedRoute>
            }
          />
          <Route
            path="/admin/izvjestaji"
            element={
              <ProtectedRoute allowed={["Administrator"]}>
                <Izvjestaji />
              </ProtectedRoute>
            }
          />
          <Route
            path="/admin/akcije"
            element={
              <ProtectedRoute allowed={["Administrator"]}>
                <Akcije />
              </ProtectedRoute>
            }
          />

          {/* DONATOR */}
          <Route
            path="/donator"
            element={
              <ProtectedRoute allowed={["Donator"]}>
                <DonatorDashboard />
              </ProtectedRoute>
            }
          />
          <Route
            path="/donator/novadonacija"
            element={
              <ProtectedRoute allowed={["Donator"]}>
                <NovaDonacija />
              </ProtectedRoute>
            }
          />
          <Route
            path="/donator/mojedonacije"
            element={
              <ProtectedRoute allowed={["Donator"]}>
                <MojeDonacije />
              </ProtectedRoute>
            }
          />

          {/* PRIMALAC */}
          <Route
            path="/primalac"
            element={
              <ProtectedRoute allowed={["PrimalacPomoci"]}>
                <PrimalacDashboard />
              </ProtectedRoute>
            }
          />
          <Route
            path="/primalac/novizahtjev"
            element={
              <ProtectedRoute allowed={["PrimalacPomoci"]}>
                <NoviZahtjev />
              </ProtectedRoute>
            }
          />
          <Route
            path="/primalac/mojizahtjevi"
            element={
              <ProtectedRoute allowed={["PrimalacPomoci"]}>
                <MojiZahtjevi />
              </ProtectedRoute>
            }
          />

          {/* VOLONTER */}
          <Route
            path="/volonter"
            element={
              <ProtectedRoute allowed={["Volonter"]}>
                <VolonterDashboard />
              </ProtectedRoute>
            }
          />
          <Route
            path="/volonter/mojeakcije"
            element={
              <ProtectedRoute allowed={["Volonter"]}>
                <MojeAkcije />
              </ProtectedRoute>
            }
          />
        </Routes>
        <Footer />
      </AuthProvider>
    </Router>
  );
}
