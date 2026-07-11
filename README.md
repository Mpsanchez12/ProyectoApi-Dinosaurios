# 🦕 Legado Mesozoico Argentino

Proyecto fullstack de gestión paleontológica de dinosaurios argentinos.  
Desarrollado por **Candela Caraballo** y **María Pilar Sánchez**.

---

## 📁 Estructura del proyecto

```
proyecto-api/
├── DinoArgentoApi/        → Backend (ASP.NET Core C#)
└── mesozoic-frontend/     → Frontend (React + Vite)
```

---

## ⚙️ Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) con SQL Server LocalDB
- [Git](https://git-scm.com/)

---

## 🔧 Cómo correr el Backend

### 1. Abrir el proyecto
Abrí el archivo `DinoArgentoApi/DinoArgentoApi.sln` con Visual Studio.

### 2. Configurar la base de datos
El archivo `appsettings.json` ya tiene la cadena de conexión configurada para LocalDB:
```json
"ConnectionStrings": {
  "devConnection": "Server=(localdb)\\mssqllocaldb;Database=DinosArchiveDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Correr las migraciones
En la **Consola del Administrador de Paquetes** (Herramientas → NuGet → Consola):
```
Update-Database -Context AppDbContext
```

### 4. Correr la API
Presioná **F5** o el botón de Play en Visual Studio.

La API queda disponible en:
```
https://localhost:7181/api
```

Swagger disponible en:
```
https://localhost:7181/swagger
```

---

## 🎨 Cómo correr el Frontend

### 1. Instalar dependencias
Abrí una terminal en la carpeta `mesozoic-frontend/` y corré:
```bash
npm install
```

### 2. Verificar la URL del backend
En `src/services/api.js` asegurate que el puerto coincida con el de tu API:
```js
baseURL: 'https://localhost:7181/api'
```

### 3. Correr el frontend
```bash
npm run dev
```

El frontend queda disponible en:
```
http://localhost:5173
```

---

## 🔐 Credenciales de prueba

Para probar la app, primero registrá un usuario desde `/register` o usá la API de Swagger.

Para crear un usuario **Admin**, usá el endpoint:
```
PUT /api/auth/update-roles/{userId}
```

---

## ✅ Funcionalidades implementadas

- **Autenticación** con JWT + roles (Admin / User)
- **Listado** de dinosaurios argentinos con búsqueda y filtro
- **Vista de detalle** de cada dinosaurio (lazy loading)
- **Panel Admin** con estadísticas (lazy loading)
- **CRUD** de dinosaurios (solo Admin)
- **Zustand** para estado global de autenticación
- **React Hook Form + Zod** para validación de formularios
- **Axios** con interceptor que inyecta el token automáticamente
- **Wouter** para ruteo del lado del cliente
- **React.lazy + Suspense** en DinosaurioDetail y AdminPanel

---

## 🛠️ Tecnologías utilizadas

### Backend
- ASP.NET Core 10 Web API
- Entity Framework Core 10
- SQL Server LocalDB
- JWT Bearer Authentication
- AutoMapper
- BCrypt.Net
- Swagger (Swashbuckle)

### Frontend
- React 18 + Vite
- Wouter (ruteo)
- Zustand (estado global)
- Axios (HTTP)
- React Hook Form + Zod (formularios y validación)
- CSS inline (estilos temáticos)

---

## 📌 Endpoints principales de la API

| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| POST | `/api/auth/register` | Registrar usuario | No |
| POST | `/api/auth/login` | Iniciar sesión | No |
| POST | `/api/auth/logout` | Cerrar sesión | Sí |
| GET | `/api/dinosaurios` | Listar dinosaurios | Sí |
| GET | `/api/dinosaurios/{id}` | Detalle dinosaurio | Sí |
| POST | `/api/dinosaurios` | Crear dinosaurio | Admin |
| PUT | `/api/dinosaurios/{id}` | Editar dinosaurio | Admin |
| DELETE | `/api/dinosaurios/{id}` | Eliminar dinosaurio | Admin |
| GET | `/api/periodos` | Listar períodos | No |
| GET | `/api/dietas` | Listar dietas | No |

---

## 👥 Integrantes 
Maria Candela Caraballo
Sanchez Maria Del Pilar

- Candela Caraballo
- María Pilar Sánchez
-
