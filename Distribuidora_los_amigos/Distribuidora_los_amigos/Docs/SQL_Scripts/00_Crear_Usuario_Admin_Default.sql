-- =============================================
-- Script: Crear Usuario Administrador por Defecto
-- Base de datos: Login
-- Descripción: Crea el usuario administrador cuando se requiere acceso de emergencia
-- =============================================

USE [Login]
GO

-- Verificar si el usuario admin ya existe
IF NOT EXISTS (SELECT 1 FROM Usuario WHERE UserName = 'admin')
BEGIN
    PRINT 'Creando usuario administrador por defecto...'
    
    -- Crear el usuario admin con contraseña hasheada MD5
    -- Contraseña en texto plano: Admin123!
    -- Hash MD5: e3afed0047b08059d0fada10f400c1e5
    INSERT INTO Usuario (IdUsuario, UserName, Password, Estado, Email, Lenguaje)
    VALUES (
        NEWID(),
        'admin',
        'e3afed0047b08059d0fada10f400c1e5',  -- Hash MD5 de "Admin123!"
        1,  -- Estado: Habilitado
        'admin@sistema.com',
        'es-ES'
    )
    
    PRINT 'Usuario administrador creado exitosamente.'
    PRINT 'Usuario: admin'
    PRINT 'Contraseña: Admin123!'
    PRINT '?? IMPORTANTE: Cambie esta contraseña inmediatamente después del primer inicio de sesión.'
END
ELSE
BEGIN
    PRINT 'El usuario admin ya existe en el sistema.'
END
GO

-- Verificar que el usuario fue creado correctamente
SELECT 
    IdUsuario,
    UserName,
    Email,
    Estado,
    Lenguaje,
    'Creado correctamente' AS Estado_Creacion
FROM Usuario 
WHERE UserName = 'admin'
GO

-- =============================================
-- NOTAS IMPORTANTES:
-- =============================================
-- 1. Este script es para uso de emergencia o instalación manual
-- 2. La aplicación crea automáticamente este usuario si no existe ningún usuario
-- 3. La contraseña debe ser cambiada inmediatamente después del primer uso
-- 4. Hash MD5 calculado: MD5("Admin123!") = e3afed0047b08059d0fada10f400c1e5
-- =============================================

-- =============================================
-- Script alternativo: Resetear contraseña del admin
-- =============================================
-- Descomentar y ejecutar si necesitas resetear la contraseña del admin
/*
UPDATE Usuario
SET Password = 'e3afed0047b08059d0fada10f400c1e5'  -- Resetea a "Admin123!"
WHERE UserName = 'admin'

PRINT 'Contraseña del usuario admin reseteada a: Admin123!'
*/
GO

-- =============================================
-- Script: Habilitar usuario admin si fue deshabilitado
-- =============================================
-- Descomentar y ejecutar si el admin fue deshabilitado y necesitas acceso
/*
UPDATE Usuario
SET Estado = 1
WHERE UserName = 'admin'

PRINT 'Usuario admin habilitado correctamente.'
*/
GO

-- =============================================
-- Script: Verificar estado actual del usuario admin
-- =============================================
SELECT 
    UserName AS 'Usuario',
    Email AS 'Email',
    CASE Estado 
        WHEN 1 THEN 'Habilitado' 
        WHEN 0 THEN 'Deshabilitado'
        ELSE 'Desconocido'
    END AS 'Estado',
    Lenguaje AS 'Idioma'
FROM Usuario 
WHERE UserName = 'admin'
GO
