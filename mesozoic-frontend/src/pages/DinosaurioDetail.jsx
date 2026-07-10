import { useEffect, useState } from 'react';
import { Link } from 'wouter';
import api from '../services/api';

const descripcionesLocales = {
  herrerasaurus: "Uno de los primeros dinosaurios terópodos, un ágil depredador bipedal de la formación Ischigualasto.",
  eoraptor: "Pequeño dinosaurio omnívoro basal, considerado uno de los ancestros más antiguos de los dinosaurios.",
  panphagia: "Dinosaurio sauropodomorfo primitivo, evidencia crucial de la transición hacia los gigantes herbívoros.",
  sanjuansaurus: "Terópodo herrerasáurido temprano, un feroz cazador contemporáneo de los primeros dinosaurios.",
  patagonosaurus: "Un enorme saurópodo cetiosáurido del Jurásico Medio, caracterizado por su largo cuello y gran robustez.",
  piatnitzkysaurus: "Terópodo megalosauroideo de tamaño medio, un depredador dominante de su ecosistema jurásico.",
  eoabelisaurus: "El abelisáurido más antiguo conocido, mostrando las primeras etapas de reducción de los brazos.",
  volkheimeria: "Un saurópodo primitivo del Jurásico Medio encontrado en la Patagonia, pariente del Patagonosaurus.",
  carnotaurus: "Famoso terópodo abelisáurido distinguido por sus icónicos cuernos frontales y brazos extremadamente cortos.",
  argentinosaurus: "Un titanosaurio colosal, ampliamente considerado como uno de los animales terrestres más grandes de la historia.",
  giganotosaurus: "Un enorme terópodo carcarodontosáurido, uno de los mayores depredadores terrestres conocidos a nivel mundial.",
  amargasaurus: "Saurópodo dicreosáurido inconfundible por las largas espinas neurales que formaban una vela en su cuello.",
  megaraptor: "Misterioso y letal terópodo caracterizado por unas garras gigantescas, curvas y letales en sus manos.",
  saltasaurus: "Un titanosaurio compacto y blindado con placas óseas en su gruesa piel, un hallazgo científico revolucionario.",
  abelisaurus: "Terópodo depredador que da nombre a toda su familia, conocido principalmente por su enorme y robusto cráneo."
};

export default function DinosaurioDetail({ params }) {
  const [dino, setDino] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (params?.id) {
      api.get(`/Dinosaurios/${params.id}`)
        .then(response => {
          // Extraemos los datos: probamos si vienen en .data o directamente en la respuesta
          const dataSegura = response.data?.data || response.data;
          setDino(dataSegura);
          setLoading(false);
          console.log("Datos recibidos:", dataSegura); // Útil para verificar en consola F12
        })
        .catch((err) => {
          console.error("Error al cargar:", err);
          setLoading(false);
        });
    }
  }, [params?.id]);

  if (loading) return <h3 style={{ color: '#fbd000', textAlign: 'center', marginTop: '5rem' }}>Cargando ficha del espécimen...</h3>;
  if (!dino) return <h3 style={{ color: '#ff3366', textAlign: 'center', marginTop: '5rem' }}>Espécimen no encontrado.</h3>;

  const nombre = dino.nombre || dino.Nombre || 'Espécimen';
  const nombreLimpio = nombre.toLowerCase().replace(/\s+/g, '');
  const imagenUrl = `/dinosaurios/${nombreLimpio}.jpeg`;
  const descripcionFinal = dino.descripcion || dino.Descripcion || descripcionesLocales[nombreLimpio] || "Descripción técnica en proceso de actualización.";

  const getPeriodoNombre = () => {
    // Intentamos obtener el ID de varias formas posibles que puede enviar la API
    const id = dino.PeriodoId;
    
    switch(String(id)) {
      case '1': return 'Triásico';
      case '2': return 'Jurásico';
      case '3': return 'Cretácico';
      default: return 'No catalogado';
    }
  };

  const getDietaNombre = () => {
    const listaDietas = dino.Dietas;
    if (listaDietas && listaDietas.length > 0) {
      return listaDietas.map(d => d.nombre || d.Nombre).join(' / ');
    }
    return dino.dieta || dino.Dieta || 'No especificada';
  };

  return (
    <div style={{ maxWidth: '1000px', margin: '0 auto', padding: '1rem', marginBottom: '4rem' }}>
      <Link href="/dinosaurios">
        <button style={{ marginBottom: '1.5rem', padding: '0.6rem 1.2rem', cursor: 'pointer', backgroundColor: '#990000', color: 'white', border: '1px solid #fbd000', borderRadius: '4px', fontWeight: 'bold' }}>
          ← Volver al Catálogo
        </button>
      </Link>

      <div style={{ backgroundColor: '#111518', padding: '3rem', borderRadius: '8px', borderTop: '5px solid #fbd000', display: 'flex', gap: '3rem', flexWrap: 'wrap', boxShadow: '0 0 20px rgba(0,0,0,0.5)' }}>
        <div style={{ flex: '1 1 350px' }}>
          <img 
            src={imagenUrl} 
            alt={nombre}
            onError={(e) => e.target.src = 'https://via.placeholder.com/400x400/000/fbd000?text=Datos+Clasificados'}
            style={{ width: '100%', height: '100%', objectFit: 'cover', borderRadius: '8px', border: '2px solid #333' }}
          />
        </div>

        <div style={{ flex: '1 1 400px' }}>
          <h2 style={{ color: '#fbd000', fontSize: '3rem', margin: '0 0 1rem 0', textTransform: 'uppercase', letterSpacing: '2px' }}>{nombre}</h2>
          <p style={{ color: '#ccc', fontSize: '1.2rem', fontStyle: 'italic', marginBottom: '2rem', lineHeight: '1.6' }}>"{descripcionFinal}"</p>

          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1.5rem', backgroundColor: 'rgba(255,255,255,0.05)', padding: '1.5rem', borderRadius: '8px' }}>
            <div>
              <span style={{ color: '#00ffcc', fontWeight: 'bold', display: 'block', marginBottom: '0.5rem' }}>PERIODO:</span>
              <div style={{ fontSize: '1.3rem', color: '#fff' }}>{getPeriodoNombre()}</div>
            </div>
            <div>
              <span style={{ color: '#00ffcc', fontWeight: 'bold', display: 'block', marginBottom: '0.5rem' }}>DIETA:</span>
              <div style={{ fontSize: '1.3rem', color: '#fff' }}>{getDietaNombre()}</div>
            </div>
            <div>
              <span style={{ color: '#00ffcc', fontWeight: 'bold', display: 'block', marginBottom: '0.5rem' }}>PESO:</span>
              <div style={{ fontSize: '1.3rem', color: '#fff' }}>{dino.peso || dino.Peso || "N/A"} kg</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}