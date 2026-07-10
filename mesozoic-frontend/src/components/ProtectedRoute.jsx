import { useAuthStore } from '../store/authStore';
import { Redirect } from 'wouter';

export default function ProtectedRoute({ children, requiredRole }) {
  // Traemos los datos de la sesión desde Zustand
  const { isAuthenticated, role } = useAuthStore();

  // Si no está logueado, lo pateamos al login
  if (!isAuthenticated) {
    return <Redirect to="/login" />;
  }

  // Si requiere un rol específico (ej: Admin) y no lo tiene, mostramos error
  if (requiredRole && role !== requiredRole) {
    return (
      <div style={{ textAlign: 'center', color: '#990000', marginTop: '2rem' }}>
        <h2>ACCESO DENEGADO 🛑</h2>
        <p>No tienes nivel de autorización suficiente para ver esta sección.</p>
      </div>
    );
  }

  // Si pasa todas las pruebas, le mostramos el contenido (children)
  return children;
}