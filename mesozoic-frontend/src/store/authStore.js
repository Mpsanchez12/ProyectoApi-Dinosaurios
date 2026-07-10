import { create } from 'zustand';
import { persist } from 'zustand/middleware';

export const useAuthStore = create(
  persist(
    (set) => ({
      // Estado inicial: nadie está logueado al entrar
      token: null,
      role: null,
      isAuthenticated: false,

      // Acción para cuando el usuario se loguea con éxito
      login: (token, role) => set({
        token: token,
        role: role,
        isAuthenticated: true
      }),

      // Acción para cuando el usuario cierra sesión
      logout: () => set({
        token: null,
        role: null,
        isAuthenticated: false
      })
    }),
    {
      name: 'mesozoic-auth-storage', // Nombre del "cajón" donde se guarda en el navegador
    }
  )
);