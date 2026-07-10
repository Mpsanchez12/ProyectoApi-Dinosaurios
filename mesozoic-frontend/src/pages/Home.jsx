import { Link } from 'wouter';
import fondoHome from '../assets/dinos/fondo-home.jpeg';

export default function Home() {
  return (
    <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', textAlign: 'center' }}>
      
      {/* PORTADA (HERO SECTION) */}
      <div style={{ 
        width: '100%', 
        backgroundImage: `url(${fondoHome})`, 
        backgroundSize: 'cover', 
        backgroundPosition: 'center',
        padding: '6rem 1rem',
        borderBottom: '4px solid #304755',
        position: 'relative'
      }}>
        <div style={{ position: 'absolute', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(225, 214, 194, 0.4)' }}></div>
        
        <div style={{ position: 'relative', zIndex: 1 }}>
          <h1 style={{ fontSize: '4rem', color: '#224357', margin: '0 0 1rem 0', textShadow: '1px 1px 2px rgba(255,255,255,0.5)' }}>
            Explorá el Pasado
          </h1>
          
          <p style={{ fontSize: '1.3rem', fontWeight:'600',maxWidth: '800px', margin: '0 auto 2rem auto', lineHeight: '1.6', color: '#224357' }}>
            Legado Mesozoico Argentino te invita a descubrir a los gigantes que habitaron nuestro planeta, con un enfoque especial en las especies descubiertas en territorio argentino.
          </p>
          
          <Link href="/dinosaurios">
            <button style={{ 
              padding: '1.2rem 3rem', 
              fontSize: '1.3rem', 
              backgroundColor: 'transparent', 
              color: '#304e61', 
              border: '3px solid #304e61', 
              borderRadius: '6px', 
              cursor: 'pointer', 
              fontWeight: 'bold',
              textTransform: 'uppercase'
            }}>
              Ver Catálogo de Especies 
            </button>
          </Link>
        </div>
      </div>

      {/* SECCIÓN INFERIOR */}
      <div style={{ padding: '4rem 2rem', color: '#224357', maxWidth: '900px', lineHeight: '1.8' }}>
        <h2 style={{ color: '#224357', marginBottom: '1rem' }}>El registro fósil argentino</h2>
        <p style={{ fontSize: '1.1rem' }}>
          Nuestro país posee uno de los registros paleontológicos más ricos e importantes del mundo. Desde el temible Giganotosaurus hasta el colosal Argentinosaurus, nuestra base de datos te permite conocer la dieta, el peso y el periodo histórico de cada especie de forma rápida y sencilla.
        </p>
      </div>
    </div>
  );
}