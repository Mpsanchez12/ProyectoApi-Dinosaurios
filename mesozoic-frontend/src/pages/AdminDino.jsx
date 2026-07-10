import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import api from '../services/api';

const dinoSchema = z.object({
  nombre: z.string().min(2, "Mínimo 2 caracteres"),
  periodo: z.string().min(3, "Indica el periodo (ej: Triásico)"),
  descripcion: z.string().max(200, "Máximo 200 caracteres")
});

export default function AdminDino() {
  const { register, handleSubmit, formState: { errors } } = useForm({
    resolver: zodResolver(dinoSchema)
  });

  const onSubmit = async (data) => {
    try {
      await api.post('/Dinosaurios', data);
      alert("¡Dino registrado en la era Mesozoica!");
    } catch (err) {
      alert("Error al guardar el dino");
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input {...register("nombre")} placeholder="Nombre del Dino" />
      {errors.nombre && <p>{errors.nombre.message}</p>}
      <button type="submit">Guardar</button>
    </form>
  );
}