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
          const dataSegura = response.data?.data || response.data;
          setDino(dataSegura);
          setLoading(false);
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
    <div style={{ maxWidth: '900px', margin: '2rem auto', padding: '1rem', marginBottom: '4rem' }}>
      <Link href="/dinosaurios">
        <button style={{ marginBottom: '1.5rem', padding: '0.6rem 1.2rem', cursor: 'pointer', backgroundColor: '#990000', color: 'white', border: '1px solid #fbd000', borderRadius: '4px', fontWeight: 'bold' }}>
          ← Volver al Catálogo
        </button>
      </Link>

      {/* FICHA */}
      <div style={{ backgroundColor: '#111518', borderRadius: '10px', border: '2px solid #fbd000', boxShadow: '0 0 30px rgba(251,208,0,0.15)', overflow: 'hidden' }}>
        
        {/* Cabecera de la ficha */}
        <div style={{ backgroundColor: '#fbd000', padding: '0.8rem 1.5rem', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <span style={{ fontWeight: 'bold', color: '#111', fontSize: '0.85rem', letterSpacing: '2px' }}>ARCHIVO PALEONTOLÓGICO — ESPÉCIMEN CLASIFICADO</span>
          <span style={{ fontWeight: 'bold', color: '#111', fontSize: '0.85rem' }}>ID: #{params?.id}</span>
        </div>

        <div style={{ display: 'flex', gap: '2rem', padding: '2rem', flexWrap: 'wrap' }}>
          
          {/* Imagen */}
          <div style={{ flex: '1 1 300px' }}>
            <img
              src={imagenUrl}
              alt={nombre}
              onError={(e) => e.target.src = 'https://via.placeholder.com/400x300/000/fbd000?text=Imagen+no+disponible'}
              style={{ width: '100%', height: '300px', objectFit: 'cover', borderRadius: '8px', border: '2px solid #333' }}
            />
          </div>

          {/* Info */}
          <div style={{ flex: '1 1 300px', display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <h2 style={{ color: '#fbd000', fontSize: '2.2rem', margin: 0, textTransform: 'uppercase', letterSpacing: '2px' }}>{nombre}</h2>
            <p style={{ color: '#aaa', fontSize: '1rem', fontStyle: 'italic', lineHeight: '1.6', margin: 0 }}>{descripcionFinal}</p>

            {/* Datos en tabla tipo ficha */}
            <div style={{ backgroundColor: 'rgba(255,255,255,0.05)', borderRadius: '8px', overflow: 'hidden', border: '1px solid #333' }}>
              {[
                { label: '🦕 PERÍODO', value: getPeriodoNombre() },
                { label: '🥩 DIETA', value: getDietaNombre() },
                { label: '⚖️ PESO', value: `${dino.peso || dino.Peso || 'N/A'} kg` },
              ].map((item, i) => (
                <div key={i} style={{ display: 'flex', borderBottom: i < 2 ? '1px solid #333' : 'none' }}>
                  <div style={{ backgroundColor: 'rgba(251,208,0,0.1)', padding: '0.8rem 1rem', minWidth: '140px', color: '#00ffcc', fontWeight: 'bold', fontSize: '0.85rem' }}>
                    {item.label}
                  </div>
                  <div style={{ padding: '0.8rem 1rem', color: '#fff', fontSize: '1rem' }}>
                    {item.value}
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>

        {/* Pie de ficha */}
        <div style={{ backgroundColor: 'rgba(255,255,255,0.03)', borderTop: '1px solid #333', padding: '0.8rem 1.5rem', textAlign: 'center' }}>
          <span style={{ color: '#555', fontSize: '0.8rem', letterSpacing: '1px' }}>LEGADO MESOZOICO ARGENTINO — REGISTRO PALEONTOLÓGICO OFICIAL</span>
        </div>
      </div>
    </div>
  );
}