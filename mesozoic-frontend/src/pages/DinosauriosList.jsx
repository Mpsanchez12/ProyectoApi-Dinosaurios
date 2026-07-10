import { useEffect, useState } from 'react';
import { useLocation } from 'wouter';
import api from '../services/api'; 

export default function DinosauriosList() {
  const [dinos, setDinos] = useState([]);
  const [, setLocation] = useLocation();

  useEffect(() => {
    api.get('/Dinosaurios').then(res => setDinos(res.data));
  }, []);

  return (
    <div style={{ padding: '2rem' }}>
      <h2 style={{ 
        color: '#3b5768', 
        textAlign: 'center', 
        marginBottom: '3rem', 
        textTransform: 'uppercase',
        letterSpacing: '3px'
      }}>
        Catálogo General de Especímenes
      </h2>
      
      <div style={{ 
        display: 'grid', 
        gap: '2.5rem', 
        gridTemplateColumns: 'repeat(auto-fill, minmax(250px, 1fr))' 
      }}>
        {dinos.map(dino => (
          <div 
            key={dino.id} 
            onClick={() => setLocation(`/dinosaurios/${dino.id}`)} 
            style={{ 
              backgroundColor: '#3b5768',
              border: '1px solid #3b5768',
              padding: '15px',
              boxShadow: '4px 4px 10px rgba(0,0,0,0.15)',
              cursor: 'pointer',
              transition: 'transform 0.3s ease, box-shadow 0.3s ease'
            }}
            onMouseEnter={(e) => {
              e.currentTarget.style.transform = 'scale(1.03)';
              e.currentTarget.style.boxShadow = '8px 8px 20px rgba(0,0,0,0.2)';
            }}
            onMouseLeave={(e) => {
              e.currentTarget.style.transform = 'scale(1)';
              e.currentTarget.style.boxShadow = '4px 4px 10px rgba(0,0,0,0.15)';
            }}
          >
            {/* Marco tipo lámina fotográfica */}
            <div style={{ 
              border: '4px solid #3b5768', 
              boxShadow: 'inset 0 0 10px rgba(0,0,0,0.1)', 
              overflow: 'hidden',
              height: '220px'
            }}>
              <img 
                src={`/dinosaurios/${dino.nombre.toLowerCase().replace(/\s+/g, '')}.jpeg`} 
                alt={dino.nombre} 
                style={{ 
                  width: '100%', 
                  height: '100%', 
                  objectFit: 'cover', // Llena bien el marco sin dejar bordes vacíos
                  filter: 'sepia(10%) brightness(1.05)' // Integración antigua
                }} 
              />
            </div>
            
            {/* Nombre con estilo de tipografía de catálogo */}
            <div style={{ 
              marginTop: '15px', 
              textAlign: 'center', 
              color: '#ffffff', 
              fontFamily: 'serif', 
              fontWeight: 'bold',
              fontStyle: 'italic',
              fontSize: '1.2rem'
            }}>
              {dino.nombre}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}