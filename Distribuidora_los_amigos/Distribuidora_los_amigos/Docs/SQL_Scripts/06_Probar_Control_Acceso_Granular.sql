-- ============================================
-- Script para PROBAR el Control de Acceso Granular
-- ============================================
-- Crea roles de prueba con patentes específicas
-- y usuarios de prueba para validar el sistema
-- ============================================

USE [Login]
GO

-- ==================================================
-- PASO 1: Limpiar datos de prueba anteriores
-- ==================================================
PRINT '============================================'
PRINT 'LIMPIANDO DATOS DE PRUEBA ANTERIORES'
PRINT '============================================'

-- Eliminar usuarios de prueba
DELETE FROM Usuario_Familia WHERE IdUsuario IN (
    SELECT IdUsuario FROM Usuario WHERE UserName IN ('VendedorTest', 'GerenteTest', 'AuditorTest')
)

DELETE FROM Usuario WHERE UserName IN ('VendedorTest', 'GerenteTest', 'AuditorTest')

-- Eliminar familias de prueba
DELETE FROM Familia_Patente WHERE IdFamilia IN (
    SELECT IdFamilia FROM Familia WHERE Nombre IN ('Vendedor', 'Gerente', 'Auditor')
)

DELETE FROM Familia WHERE Nombre IN ('Vendedor', 'Gerente', 'Auditor')

PRINT '? Datos de prueba anteriores eliminados'
PRINT ''
GO

-- ==================================================
-- PASO 2: Crear Rol "Vendedor" (Solo operaciones básicas)
-- ==================================================
PRINT '============================================'
PRINT 'CREANDO ROL: VENDEDOR'
PRINT '============================================'

DECLARE @IdFamiliaVendedor UNIQUEIDENTIFIER = NEWID()

INSERT INTO Familia (IdFamilia, Nombre)
VALUES (@IdFamiliaVendedor, 'Vendedor')

-- Asignar solo patentes de visualización y creación básica
INSERT INTO Familia_Patente (IdFamilia, IdPatente)
SELECT @IdFamiliaVendedor, IdPatente
FROM Patente
WHERE Nombre IN (
    'Crear_cliente',
    'Mostrar_clientes',
    'VER_PRODUCTOS',
    'CREAR_PEDIDO',
    'MOSTRAR_PEDIDOS',
    'Mostrar_Stock'
)

PRINT '? Rol Vendedor creado con 6 patentes'
PRINT ''
GO

-- ==================================================
-- PASO 3: Crear Rol "Gerente" (Permisos medios)
-- ==================================================
PRINT '============================================'
PRINT 'CREANDO ROL: GERENTE'
PRINT '============================================'

DECLARE @IdFamiliaGerente UNIQUEIDENTIFIER = NEWID()

INSERT INTO Familia (IdFamilia, Nombre)
VALUES (@IdFamiliaGerente, 'Gerente')

-- Asignar patentes UI + algunas Control
INSERT INTO Familia_Patente (IdFamilia, IdPatente)
SELECT @IdFamiliaGerente, IdPatente
FROM Patente
WHERE Nombre IN (
    -- UI (Visualización)
    'Mostrar_clientes',
    'VER_PRODUCTOS',
    'MOSTRAR_PEDIDOS',
    'Mostrar_Stock',
    'Mostrar_Proveedores',
    'Ver_usuarios',
    'ReporteStockBajo',
    'ReporteProductosMasVendidos',
    -- Control (Gestión)
    'Crear_cliente',
    'CREAR_PEDIDO',
    'AGREGAR',
    'MODIFICAR',
    'Modificar_Proveedor',
    'Crear_Proveedor'
)

PRINT '? Rol Gerente creado con 14 patentes'
PRINT ''
GO

-- ==================================================
-- PASO 4: Crear Rol "Auditor" (Solo lectura total)
-- ==================================================
PRINT '============================================'
PRINT 'CREANDO ROL: AUDITOR'
PRINT '============================================'

DECLARE @IdFamiliaAuditor UNIQUEIDENTIFIER = NEWID()

INSERT INTO Familia (IdFamilia, Nombre)
VALUES (@IdFamiliaAuditor, 'Auditor')

-- Asignar SOLO patentes UI (todas las de visualización)
INSERT INTO Familia_Patente (IdFamilia, IdPatente)
SELECT @IdFamiliaAuditor, IdPatente
FROM Patente
WHERE TipoAcceso = 0 -- Solo UI

PRINT '? Rol Auditor creado con todas las patentes UI'
PRINT ''
GO

-- ==================================================
-- PASO 5: Crear usuarios de prueba
-- ==================================================
PRINT '============================================'
PRINT 'CREANDO USUARIOS DE PRUEBA'
PRINT '============================================'

-- Password: "123" encriptado en SHA-256
-- Puedes cambiar esto por el algoritmo que uses en tu aplicación
DECLARE @Password NVARCHAR(100) = 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3'

-- Usuario Vendedor
DECLARE @IdUsuarioVendedor UNIQUEIDENTIFIER = NEWID()
INSERT INTO Usuario (IdUsuario, UserName, Password, Email, Estado)
VALUES (@IdUsuarioVendedor, 'VendedorTest', @Password, 'vendedor@test.com', 1)

INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
SELECT @IdUsuarioVendedor, IdFamilia
FROM Familia WHERE Nombre = 'Vendedor'

PRINT '? Usuario VendedorTest creado (Password: 123)'

-- Usuario Gerente
DECLARE @IdUsuarioGerente UNIQUEIDENTIFIER = NEWID()
INSERT INTO Usuario (IdUsuario, UserName, Password, Email, Estado)
VALUES (@IdUsuarioGerente, 'GerenteTest', @Password, 'gerente@test.com', 1)

INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
SELECT @IdUsuarioGerente, IdFamilia
FROM Familia WHERE Nombre = 'Gerente'

PRINT '? Usuario GerenteTest creado (Password: 123)'

-- Usuario Auditor
DECLARE @IdUsuarioAuditor UNIQUEIDENTIFIER = NEWID()
INSERT INTO Usuario (IdUsuario, UserName, Password, Email, Estado)
VALUES (@IdUsuarioAuditor, 'AuditorTest', @Password, 'auditor@test.com', 1)

INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
SELECT @IdUsuarioAuditor, IdFamilia
FROM Familia WHERE Nombre = 'Auditor'

PRINT '? Usuario AuditorTest creado (Password: 123)'
PRINT ''
GO

-- ==================================================
-- PASO 6: Verificar configuración
-- ==================================================
PRINT '============================================'
PRINT 'VERIFICACIÓN DE CONFIGURACIÓN'
PRINT '============================================'
PRINT ''

PRINT '?? RESUMEN DE ROLES Y PATENTES:'
PRINT '================================'
SELECT 
    f.Nombre AS Rol,
    COUNT(fp.IdPatente) AS TotalPatentes,
    SUM(CASE WHEN p.TipoAcceso = 0 THEN 1 ELSE 0 END) AS Patentes_UI,
    SUM(CASE WHEN p.TipoAcceso = 1 THEN 1 ELSE 0 END) AS Patentes_Control
FROM Familia f
LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
LEFT JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre IN ('Vendedor', 'Gerente', 'Auditor', 'Administrador', 'UI')
GROUP BY f.Nombre
ORDER BY COUNT(fp.IdPatente) DESC
GO

PRINT ''
PRINT '?? USUARIOS DE PRUEBA CREADOS:'
PRINT '================================'
SELECT 
    u.UserName AS Usuario,
    u.Email,
    f.Nombre AS Rol,
    CASE WHEN u.Estado = 1 THEN '? Habilitado' ELSE '? Deshabilitado' END AS Estado
FROM Usuario u
LEFT JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
LEFT JOIN Familia f ON uf.IdFamilia = f.IdFamilia
WHERE u.UserName IN ('VendedorTest', 'GerenteTest', 'AuditorTest')
ORDER BY u.UserName
GO

PRINT ''
PRINT '?? PATENTES DEL ROL "VENDEDOR":'
PRINT '================================'
SELECT p.Nombre AS Patente, p.TipoAcceso
FROM Familia f
INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Vendedor'
ORDER BY p.TipoAcceso, p.Nombre
GO

PRINT ''
PRINT '?? PATENTES DEL ROL "GERENTE":'
PRINT '================================'
SELECT p.Nombre AS Patente, p.TipoAcceso
FROM Familia f
INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Gerente'
ORDER BY p.TipoAcceso, p.Nombre
GO

PRINT ''
PRINT '?? PATENTES DEL ROL "AUDITOR":'
PRINT '================================'
SELECT p.Nombre AS Patente, p.TipoAcceso
FROM Familia f
INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Auditor'
ORDER BY p.TipoAcceso, p.Nombre
GO

-- ==================================================
-- INSTRUCCIONES DE PRUEBA
-- ==================================================
PRINT ''
PRINT '============================================'
PRINT '?? INSTRUCCIONES DE PRUEBA'
PRINT '============================================'
PRINT ''
PRINT '1?? PROBAR CON USUARIO VENDEDOR:'
PRINT '   Usuario: VendedorTest'
PRINT '   Password: 123'
PRINT '   Menús esperados:'
PRINT '   ? PEDIDOS (Crear, Mostrar)'
PRINT '   ? CLIENTE (Crear, Mostrar)'
PRINT '   ? PRODUCTOS (Ver Productos)'
PRINT '   ? STOCK (Mostrar Stock)'
PRINT '   ? PROVEEDORES (oculto)'
PRINT '   ? GESTIÓN DE USUARIOS (oculto)'
PRINT '   ? BACKUP Y RESTORE (oculto)'
PRINT '   ? REPORTES (oculto)'
PRINT ''
PRINT '2?? PROBAR CON USUARIO GERENTE:'
PRINT '   Usuario: GerenteTest'
PRINT '   Password: 123'
PRINT '   Menús esperados:'
PRINT '   ? PEDIDOS (Crear, Mostrar)'
PRINT '   ? CLIENTE (Crear, Mostrar)'
PRINT '   ? PRODUCTOS (Agregar, Modificar, Ver)'
PRINT '   ? STOCK (Mostrar)'
PRINT '   ? PROVEEDORES (Mostrar, Modificar, Crear)'
PRINT '   ? GESTIÓN DE USUARIOS (Ver usuarios)'
PRINT '   ? REPORTES (Stock Bajo, Más Vendidos)'
PRINT '   ? BACKUP Y RESTORE (oculto)'
PRINT ''
PRINT '3?? PROBAR CON USUARIO AUDITOR:'
PRINT '   Usuario: AuditorTest'
PRINT '   Password: 123'
PRINT '   Menús esperados:'
PRINT '   ? Todos los menús de VISUALIZACIÓN'
PRINT '   ? Ningún menú de MODIFICACIÓN/CREACIÓN'
PRINT '   ? BACKUP Y RESTORE (oculto)'
PRINT ''
PRINT '? Script de prueba ejecutado correctamente'
PRINT '============================================'
GO
