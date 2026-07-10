import { useState, useEffect } from 'react';
import api from '../services/api';

export default function AdminPanel() {
  const [dinos, setDinos] = useState([]);
  
  const [editandoId, setEditandoId] = useState(null);

  const [formData, setFormData] = useState({
    nombre: '',
    peso: '',
    periodoId: '1', 
    isActivo: true,
    descripcion: '' 
  });

  useEffect(() => {
    cargarDinos();
  }, []);

  const cargarDinos = async () => {
    try {
      const response = await api.get('/Dinosaurios');
      setDinos(response.data);
    } catch (error) {
      console.error("Error al cargar el catálogo:", error);
    }
  };

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value
    });
  };

  // NUEVO: Función para cargar los datos del dinosaurio en el formulario cuando tocamos "Editar"
  const handleEditar = (dino) => {
    setFormData({
      nombre: dino.nombre,
      peso: dino.peso,
      periodoId: dino.periodoId.toString(), // Aseguramos que sea string para el select
      isActivo: dino.isActivo,
      descripcion: dino.descripcion || '' // Prevenimos nulls
    });
    setEditandoId(dino.id); // Guardamos el ID del que estamos editando
  };

  // NUEVO: Función para cancelar la edición y limpiar el formulario
  const cancelarEdicion = () => {
    setFormData({ nombre: '', peso: '', periodoId: '1', isActivo: true, descripcion: '' });
    setEditandoId(null);
  };

  // NUEVO: Función para eliminar
  const handleEliminar = async (id) => {
    // Pedimos confirmación para no borrar por accidente
    if (window.confirm("¿Estás segura de que querés eliminar este espécimen del catálogo?")) {
      try {
        await api.delete(`/Dinosaurios/${id}`);
        alert("Especie eliminada correctamente. ☄️");
        cargarDinos(); // Recargamos la lista
      } catch (error) {
        console.error("Error al eliminar:", error);
        alert("Hubo un error al intentar eliminar.");
      }
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const payload = {
        // Si estamos editando, mandamos el ID, sino 0 o nada dependiendo de tu backend
        id: editandoId || 0, 
        nombre: formData.nombre,
        peso: Number(formData.peso),
        periodoId: Number(formData.periodoId),
        isActivo: formData.isActivo,
        descripcion: formData.descripcion,
        fechaCreacion: new Date().toISOString()
      };

      if (editandoId) {
        // MODO EDICIÓN: Hacemos un PUT a la ruta con el ID
        await api.put(`/Dinosaurios/${editandoId}`, payload);
        alert("¡Datos del dinosaurio actualizados con éxito! 🦖");
      } else {
        // MODO CREACIÓN: Hacemos un POST
        await api.post('/Dinosaurios', payload);
        alert("¡Nuevo dinosaurio agregado al catálogo! 🥚");
      }
      
      cargarDinos(); 
      cancelarEdicion(); // Limpiamos todo al terminar
      
    } catch (error) {
      console.error("Error al guardar:", error);
      alert("Error al intentar guardar. Revisá la consola (F12) para más detalles.");
    }
  };

  return (
    <div style={{ display: 'flex', gap: '2rem', flexWrap: 'wrap' }}>
      
      <div style={{ flex: '1 1 300px', backgroundColor: '#92745b', padding: '2rem', borderRadius: '8px', border: '1px solid #394f7e' }}>
        <h2 style={{ color: '#fbd000', marginTop: 0 }}>
          {editandoId ? "Editar Especie" : "Agregar Especie"}
        </h2>
        
        <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          
          <div style={{ display: 'flex', flexDirection: 'column' }}>
            <label>Nombre del Dinosaurio:</label>
            <input type="text" name="nombre" value={formData.nombre} onChange={handleChange} required style={{ padding: '0.5rem', background: '#ac987f', color: 'white', border: '1px solid #253558' }} />
          </div>

          <div style={{ display: 'flex', flexDirection: 'column' }}>
            <label>Peso Estimado (Kg):</label>
            <input type="number" name="peso" value={formData.peso} onChange={handleChange} required style={{ padding: '0.5rem', background: '#ac987f', color: 'white', border: '1px solid #253558' }} />
          </div>

          <div style={{ display: 'flex', flexDirection: 'column' }}>
            <label>Periodo Histórico:</label>
            <select name="periodoId" value={formData.periodoId} onChange={handleChange} style={{ padding: '0.5rem', background: '#ac987f', color: 'white', border: '1px solid #253558' }}>
              <option value="1">Triásico</option>
              <option value="2">Jurásico</option>
              <option value="3">Cretácico</option>
            </select>
          </div>

          <div style={{ display: 'flex', flexDirection: 'column' }}>
            <label>Descripción:</label>
            <textarea 
              name="descripcion" 
              value={formData.descripcion} 
              onChange={handleChange} 
              rows="3" 
              style={{ padding: '0.5rem', background: '#ac987f', color: 'white', border: '1px solid #394f7e' }}
            />
          </div>

          <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
            <input type="checkbox" name="isActivo" checked={formData.isActivo} onChange={handleChange} />
            <label>¿Está activo en el catálogo?</label>
          </div>

          <div style={{ display: 'flex', gap: '1rem', marginTop: '1rem' }}>
            <button type="submit" style={{ flex: 1, padding: '0.75rem', backgroundColor: editandoId ? '#005599' : '#990000', color: 'white', border: 'none', cursor: 'pointer', fontWeight: 'bold' }}>
              {editandoId ? "Actualizar" : "Guardar"}
            </button>

            {editandoId && (
              <button type="button" onClick={cancelarEdicion} style={{ flex: 1, padding: '0.75rem', backgroundColor: '#afafaf', color: 'white', border: 'none', cursor: 'pointer', fontWeight: 'bold' }}>
                Cancelar
              </button>
            )}
          </div>
        </form>
      </div>

      <div style={{ flex: '2 1 400px' }}>
        <h2>Registros Actuales ({dinos.length})</h2>
        <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
          {dinos.map(dino => (
            <div key={dino.id} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '1rem', backgroundColor: '#92745b', borderLeft: '4px solid #fbd000' }}>
              <div>
                <span><strong>{dino.nombre}</strong> (ID: {dino.id})</span>
                <br/>
                <span style={{ color: '#17233d', fontSize: '0.9rem' }}>Peso: {dino.peso} kg</span>
              </div>
              
              {/* NUEVO: Botones de acción en cada fila */}
              <div style={{ display: 'flex', gap: '0.5rem' }}>
                <button 
                  onClick={() => handleEditar(dino)} 
                  style={{ padding: '0.4rem 0.8rem', backgroundColor: '#005599', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
                  Editar
                </button>
                <button 
                  onClick={() => handleEliminar(dino.id)} 
                  style={{ padding: '0.4rem 0.8rem', backgroundColor: '#990000', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
                  Eliminar
                </button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}