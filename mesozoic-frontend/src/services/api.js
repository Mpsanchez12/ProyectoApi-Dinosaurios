import axios from 'axios';
import { useAuthStore } from '../store/authStore';

// 1. Creamos una instancia de Axios con la URL base de tu backend
const api = axios.create({
  baseURL: 'https://localhost:7181/api', // CAMBIAR POR EL PUERTO DE TU API DE C#
  headers: {
    'Content-Type': 'application/json',
  },
});

// 2. Interceptor de peticiones (El Bonus Track de la rúbrica)
api.interceptors.request.use(
  (config) => {
    // Vamos a buscar el token directamente al estado global de Zustand
    const token = useAuthStore.getState().token;
    
    // Si hay un token guardado, se lo inyectamos a la cabecera de la petición
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default api;