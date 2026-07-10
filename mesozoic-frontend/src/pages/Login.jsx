import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { useAuthStore } from '../store/authStore';
import fondoDino from '../assets/dinos/fondo-login.jpeg';
import { useLocation } from 'wouter';
import api from '../services/api';

const loginSchema = z.object({
  userName: z.string().min(1, { message: "El nombre de usuario es obligatorio" }),
  password: z.string().min(6, { message: "La contraseña debe tener al menos 6 caracteres" })
});

export default function Login() {
  const login = useAuthStore((state) => state.login);
  const [, setLocation] = useLocation();
  const { register, handleSubmit, formState: { errors } } = useForm({ resolver: zodResolver(loginSchema) });

  const onSubmit = async (data) => {
    try {
      const response = await api.post('/auth/login', { userName: data.userName, password: data.password });
      const { token, user } = response.data;
      let extractedRole = user?.roles?.[0] || "User";
      login(token, extractedRole);
      alert("¡Sesión iniciada con éxito!");
      setLocation(extractedRole === "Admin" ? "/admin" : "/dinosaurios");
    } catch (error) {
      alert("Error al conectar con el servidor.");
    }
  };

  return (
    <div style={{
    margin: 0,
        padding: 0,
        width: "100%",
        height: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        backgroundImage: `url(${fondoDino})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat",
        position: "absolute",
        top: 0,
        left: 0
    }}>
      <div style={{
        width: "100%", maxWidth: "400px", padding: "2rem", borderRadius: "6px",
        border: "2px solid #3b5768", backgroundColor: "rgba(225, 214, 194, 0.8)"
      }}>
        <h2 style={{ textAlign: "center", color: "#3b5768", marginBottom: "1.5rem" }}>🦖 Acceso al Sistema</h2>
        <form onSubmit={handleSubmit(onSubmit)} style={{ display: "flex", flexDirection: "column", gap: "1.2rem" }}>
          <div>
            <label style={{ color: "#3b5768", fontWeight: "bold" }}>Nombre de Usuario</label>
            <input {...register("userName")} style={{ width: "100%", padding: "10px", marginTop: "5px", border: "1px solid #3b5768", background: "transparent", color: "#3b5768" }} />
          </div>
          <div>
            <label style={{ color: "#3b5768", fontWeight: "bold" }}>Contraseña</label>
            <input type="password" {...register("password")} style={{ width: "100%", padding: "10px", marginTop: "5px", border: "1px solid #3b5768", background: "transparent", color: "#3b5768" }} />
          </div>
          <button type="submit" style={{ padding: "12px", border: "2px solid #3b5768", background: "transparent", color: "#3b5768", cursor: "pointer", fontWeight: "bold" }}>
            Iniciar Sesión
          </button>
        </form>
      </div>
    </div>
  );
}