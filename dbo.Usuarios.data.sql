-- Limpiar registros previos si deseas reiniciar desde cero (opcional)
-- DELETE FROM [dbo].[Usuarios];

-- Asegurar que se puedan insertar IDs explícitos
SET IDENTITY_INSERT [dbo].[Usuarios] ON;

-- 1. CLIENTE (Estudiante)
INSERT INTO [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Matricula], [Correo], [ContrasenaHash], [Telefono], [EsActivo], [FechaRegistro], [TipoUsuario], [Carrera], [AreaEspecialidad]) 
VALUES (3, N'Dewin', N'Rivera', N'2024-3300026', N'd.rivera@unev.edu.do', N'123456', NULL, 1, N'2026-09-30 22:25:41', N'Cliente', N'Ingeniería en Sistemas', NULL);

-- 2. TÉCNICO (Soporte Operativo)
INSERT INTO [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Matricula], [Correo], [ContrasenaHash], [Telefono], [EsActivo], [FechaRegistro], [TipoUsuario], [Carrera], [AreaEspecialidad]) 
VALUES (5, N'Carlos', N'Técnico', N'2024-999999', N'tecnico@unev.edu.do', N'123456', NULL, 1, N'2026-10-10 00:15:50', N'Tecnico', NULL, N'Soporte Técnico');

-- 3. ADMINISTRADOR (Acceso Total: Usuarios, Reportes, Tickets)
INSERT INTO [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Matricula], [Correo], [ContrasenaHash], [Telefono], [EsActivo], [FechaRegistro], [TipoUsuario], [Carrera], [AreaEspecialidad]) 
VALUES (6, N'Soporte', N'Admin', N'admin', N'admin@unev.edu.do', N'240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', NULL, 1, N'2026-10-16 18:20:33', N'Administrador', NULL, N'Administración del Sistema');

-- 4. MAESTRO (Docente)
INSERT INTO [dbo].[Usuarios] ([IdUsuario], [Nombre], [Apellido], [Matricula], [Correo], [ContrasenaHash], [Telefono], [EsActivo], [FechaRegistro], [TipoUsuario], [Carrera], [AreaEspecialidad]) 
VALUES (7, N'Profesor', N'Maestro', N'maestro', N'maestro@unev.edu.do', N'123456', NULL, 1, N'2026-10-16 19:00:00', N'Maestro', NULL, N'Facultad de Ingeniería');

-- Desactivar el insert explícito de identidades
SET IDENTITY_INSERT [dbo].[Usuarios] OFF;