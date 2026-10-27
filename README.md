# HelpDesk v2.0 - UNEV

Sistema de gestión de tickets de soporte técnico y solicitudes académicas para la Universidad Nacional Evangélica (UNEV). Esta versión 2.0 representa una mejora integral respecto a la versión anterior, incorporando un nuevo diseño UI/UX, arquitectura basada en roles, interfaz interactiva con modales, métricas dinámicas en tiempo real y despliegue en la nube.

## 🔗 Enlace de Publicación en la Nube

* **URL Pública:** <https://dewinrh-001-site1.itempurl.com> *(Servidor MyASP.NET)*

## 👥 Credenciales de Prueba (Fase 4 y 7)

| Rol | Usuario / Correo | Contraseña | Permisos | 
| ----- | ----- | ----- | ----- | 
| **Administrador** | `admin@unev.edu.do` | `Admin123!` | Acceso total al sistema, gestión de usuarios, reportes globales y dashboard completo. | 
| **Tecnico** | `Tecnico@unev.edu.do` | `123**!` | Gestión de tickets de su área, reportes filtrados y dashboard operativo. | 
| **Usuario**  | `usuario@unev.edu.do` | `User123!` | Creación de tickets propios y seguimiento de solicitudes. | 

## 📋 Fase 1: Análisis Previo del Sistema

### 1. Descripción del Negocio

El sistema **HelpDesk v2.0** permite centralizar, categorizar y dar seguimiento eficiente a los incidentes y solicitudes de soporte técnico presentados por la comunidad universitaria (estudiantes, docentes y personal administrativo). Optimiza el tiempo de respuesta reduciendo el flujo de solicitudes informales.

### 2. Diagnóstico de la Versión 1.0

* **Interfaz y Usabilidad:** Ausencia de un sistema visual corporativo, formularios extensos con recarga completa de página.

* **Seguridad:** Autenticación básica sin control estricto de roles en controladores/acciones y contraseñas almacenadas de forma insegura.

* **Base de Datos:** Estructura con inconsistencias de nulos (`DBNull`) que provocaban fallas durante la ejecución de consultas LINQ.

* **Rendimiento:** Falta de reportes exportables y ausencia de un cuadro de mando con métricas ejecutivas.

### 3. Matriz de Mejoras Priorizadas

| Problema Detectado | Mejora Propuesta (v2.0) | Prioridad | 
| ----- | ----- | ----- | 
| Ausencia de control de acceso por nivel | Implementación de Roles (`[Authorize(Roles="...")`) y menú dinámico | Alta | 
| Recargas de pantalla al procesar registros | Operaciones CRUD mediante ventanas modales (Bootstrap 5) | Alta | 
| Errores de casting por valores vacíos en BD | Tratamiento de nulos en modelos C# y base de datos MySQL | Alta | 
| Interfaz sin identidad de marca | Paleta de colores con variables CSS (`theme.css`) | Media | 
| Falta de monitoreo visual de solicitudes | Dashboard interactivo con indicadores KPI y Chart.js | Media | 

### 4. Matriz de Roles y Permisos

| Módulo / Acción | Administrador | Tecnico | Usuario / Operador | 
| ----- | ----- | ----- | ----- | 
| **Gestión de Usuarios** | Ver / Crear / Editar / Eliminar | Sin Acceso | Sin Acceso | 
| **Crear / Consultar Tickets** | Ver Todos | Ver de su Área | Solo Propios | 
| **Gestión de Tickets (Modales)** | Crear / Editar / Eliminar | Crear / Editar | Solo Crear | 
| **Dashboard y Métricas (Chart.js)** | Vista Global | Vista de Área | Sin Acceso | 
| **Exportación de Reportes (PDF/Excel)** | Todos los Registros | Filtrados por Área | Sin Acceso | 

## 🛠️ Tecnologías Utilizadas

* **Lenguaje & Framework:** C# (.NET 8.0 / ASP.NET Core MVC)

* **Acceso a Datos:** Entity Framework Core

* **Proveedor BD:** Pomelo MySQL Provider (`Pomelo.EntityFrameworkCore.MySql`)

* **Base de Datos Relacional:** MySQL Server (Alojado en MyASP.NET / phpMyAdmin)

* **Frontend:** Bootstrap 5, HTML5, CSS3 (Variables CSS personalizadas), JavaScript

* **Gráficos & Visualización:** Chart.js

* **Generación de Reportes:** QuestPDF / ClosedXML

## 🚀 Instrucciones de Ejecución Local

1. **Clonar el repositorio:**

   ```
   git clone https://github.com/tu-usuario/HelpDesk-v2.0.git
   cd HelpDesk-v2.0
   
   ```

2. **Configurar la cadena de conexión:**
   Abre el archivo `appsettings.json` y reemplaza la cadena de conexión con tus credenciales de MySQL local:

   ```
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=db_helpdesk;User=root;Password=tu_password;"
     }
   }
   
   ```

3. **Restaurar paquetes y aplicar base de datos:**

   ```
   dotnet restore
   dotnet ef database update
   
   ```

   *(Opcional: Importar el archivo `script_database.sql` incluido en la raíz del repositorio en tu gestor MySQL).*

4. **Ejecutar la aplicación:**

   ```
   dotnet run
   
   ```

## 🤖 Declaración del Uso de Inteligencia Artificial (Normativa UNEV)

De acuerdo con las normativas de la asignatura **Programación IV (INF-210-01)**:

* **Herramientas de IA utilizadas:** Gemini 3 Flash / GitHub Copilot.

* **Fases apoyadas por IA:**

  * **Fase 6 (Base de Datos):** Diagnóstico y resolución de errores `InvalidCastException` derivado de valores `DBNull` en MySQL.

  * **Fase 7 (Publicación):** Depuración de errores en tiempo de ejecución al migrar de SQL Server local a MySQL en MyASP.NET.

* **Modificaciones realizadas:** Se ajustaron los modelos C# (`Ticket.cs`, `Usuario.cs`), se sanitizaron los datos directamente en MySQL mediante consultas SQL de actualización y se refinó la configuración en `Program.cs`.

## 👨‍💻 Autor

* **Asignatura:** Programación IV (INF-210-01)

* **Docente:** Ing. Carlos Artemio Escalante Lorenzo

* **Universidad:** Universidad Nacional Evangélica (UNEV)
