import { useState, Suspense, lazy } from 'react';
import { Route, Switch, Link } from 'wouter';
import ProtectedRoute from './components/ProtectedRoute';
import { useAuthStore } from './store/authStore';

const Home = lazy(() => import('./pages/Home'));
const Login = lazy(() => import('./pages/Login'));
const DinosauriosList = lazy(() => import('./pages/DinosauriosList'));
const DinosaurioDetail = lazy(() => import('./pages/DinosaurioDetail'));
const AdminPanel = lazy(() => import('./pages/AdminPanel'));

function App() {
  const { isAuthenticated, role, logout } = useAuthStore();
  const [hoveredLink, setHoveredLink] = useState(null);

  return (
    <div>
      <header style={{ padding: '2rem', textAlign: 'center', backgroundColor: '#ac9a83' }}>
        <h1 style={{ color: '#3b5768', margin: 0, textTransform: 'uppercase' }}>
          Legado Mesozoico Argentino 🦖
        </h1>
        
        {isAuthenticated && (
          <nav style={{ marginTop: '1.5rem', display: 'flex', justifyContent: 'center', gap: '2rem' }}>
            {['INICIO', 'CATÁLOGO', 'PANEL ADMIN'].map((item, index) => {
              const paths = ['/', '/dinosaurios', '/admin'];
              const isHovered = hoveredLink === index;
              
              if (item === 'PANEL ADMIN' && !(role === 'Admin' || role === 'User')) return null;

              return (
                <Link 
                  key={item} 
                  href={paths[index]} 
                  style={{ 
                    color: '#3b5768', 
                    fontWeight: 'bold', 
                    textDecoration: 'none',
                    textShadow: isHovered ? '0 0 8px #3b5768' : 'none',
                    transform: isHovered ? 'scale(1.1)' : 'scale(1)',
                    transition: 'all 0.3s ease'
                  }}
                  onMouseEnter={() => setHoveredLink(index)}
                  onMouseLeave={() => setHoveredLink(null)}
                >
                  {item}
                </Link>
              );
            })}
            
            <button 
              onClick={logout} 
              style={{ 
                background: 'transparent', 
                border: '2px solid #3b5768', 
                color: '#3b5768', 
                cursor: 'pointer', 
                fontWeight: 'bold', 
                padding: '0.2rem 0.8rem', 
                borderRadius: '4px',
                transition: 'all 0.3s ease',
                boxShadow: hoveredLink === 'logout' ? '0 0 8px #3b5768' : 'none'
              }}
              onMouseEnter={() => setHoveredLink('logout')}
              onMouseLeave={() => setHoveredLink(null)}
            >
              CERRAR SESIÓN
            </button>
          </nav>
        )}
      </header>

      <main style={{ padding: '2rem', maxWidth: '1200px', margin: '0 auto' }}>
        <Suspense fallback={<div style={{ color: '#3b5768', textAlign: 'center' }}>CARGANDO...</div>}>
          <Switch>
            {!isAuthenticated ? <Route component={Login} /> : (
              <>
                <Route path="/" component={Home} />
                <Route path="/dinosaurios" component={DinosauriosList} />
                <Route path="/dinosaurios/:id" component={DinosaurioDetail} />
                <Route path="/admin">
                  <ProtectedRoute requiredRole="Admin">
                    <AdminPanel />
                  </ProtectedRoute>
                </Route>
              </>
            )}
          </Switch>
        </Suspense>
      </main>
    </div>
  );
}

export default App;