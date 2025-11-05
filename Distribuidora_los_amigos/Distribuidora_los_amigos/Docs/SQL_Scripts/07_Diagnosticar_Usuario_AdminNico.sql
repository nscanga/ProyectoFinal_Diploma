-- ============================================
-- Script de DIAGNÓSTICO para usuario adminNico
-- ============================================
-- Verifica por qué el usuario solo ve "BÚSQUEDA"
-- ============================================

USE [Login]
GO

PRINT '============================================'
PRINT 'DIAGNÓSTICO: Usuario adminNico'
PRINT '============================================'
PRINT ''

-- ==================================================
-- PASO 1: Verificar que el usuario existe
-- ==================================================
PRINT '1️⃣ VERIFICAR EXISTENCIA DEL USUARIO'
PRINT '===================================='

IF EXISTS (SELECT 1 FROM Usuario WHERE UserName = 'adminNico')
BEGIN
    PRINT '✅ Usuario adminNico existe'
    
    SELECT 
        IdUsuario,
        UserName,
        Email,
        CASE WHEN Estado = 1 THEN 'Habilitado' ELSE 'Deshabilitado' END AS Estado
    FROM Usuario
    WHERE UserName = 'adminNico'
END
ELSE
BEGIN
    PRINT '❌ Usuario adminNico NO EXISTE'
    PRINT ''
    PRINT 'SOLUCIÓN: Crear el usuario o verificar el nombre exacto'
END
PRINT ''
GO

-- ==================================================
-- PASO 2: Verificar familia asignada
-- ==================================================
PRINT '2️⃣ VERIFICAR FAMILIA ASIGNADA'
PRINT '===================================='

SELECT 
    u.UserName AS Usuario,
    f.Nombre AS Familia,
    f.IdFamilia
FROM Usuario u
LEFT JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
LEFT JOIN Familia f ON uf.IdFamilia = f.IdFamilia
WHERE u.UserName = 'adminNico'

IF NOT EXISTS (
    SELECT 1 
    FROM Usuario u
    INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
    WHERE u.UserName = 'adminNico'
)
BEGIN
    PRINT ''
    PRINT '❌ El usuario NO TIENE familia asignada'
    PRINT ''
    PRINT 'SOLUCIÓN:'
    PRINT 'DECLARE @IdUsuario UNIQUEIDENTIFIER'
    PRINT 'DECLARE @IdFamilia UNIQUEIDENTIFIER'
    PRINT 'SELECT @IdUsuario = IdUsuario FROM Usuario WHERE UserName = ''adminNico'''
    PRINT 'SELECT @IdFamilia = IdFamilia FROM Familia WHERE Nombre = ''Administrador'''
    PRINT 'INSERT INTO Usuario_Familia (IdUsuarioFamilia, IdUsuario, IdFamilia)'
    PRINT 'VALUES (NEWID(), @IdUsuario, @IdFamilia)'
END
ELSE
BEGIN
    PRINT '✅ Usuario tiene familia asignada'
END
PRINT ''
GO

-- ==================================================
-- PASO 3: Verificar patentes de la familia Administrador
-- ==================================================
PRINT '3️⃣ VERIFICAR PATENTES DE LA FAMILIA "Administrador"'
PRINT '===================================='

SELECT 
    f.Nombre AS Familia,
    COUNT(DISTINCT p.IdPatente) AS TotalPatentes,
    SUM(CASE WHEN p.TipoAcceso = 0 THEN 1 ELSE 0 END) AS Patentes_UI,
    SUM(CASE WHEN p.TipoAcceso = 1 THEN 1 ELSE 0 END) AS Patentes_Control
FROM Familia f
LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
LEFT JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Administrador'
GROUP BY f.Nombre

DECLARE @CantidadPatentesAdmin INT
SELECT @CantidadPatentesAdmin = COUNT(DISTINCT p.IdPatente)
FROM Familia f
LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
LEFT JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Administrador'

IF @CantidadPatentesAdmin = 0
BEGIN
    PRINT ''
    PRINT '❌ La familia Administrador NO TIENE PATENTES asignadas'
    PRINT ''
    PRINT 'SOLUCIÓN: Asignar todas las patentes a Administrador'
    PRINT 'Ver script: 08_Asignar_Todas_Patentes_Administrador.sql'
END
ELSE IF @CantidadPatentesAdmin < 20
BEGIN
    PRINT ''
    PRINT '⚠️  La familia Administrador tiene POCAS patentes (' + CAST(@CantidadPatentesAdmin AS VARCHAR(10)) + ')'
    PRINT 'Se esperan al menos 22 patentes'
END
ELSE
BEGIN
    PRINT ''
    PRINT '✅ La familia Administrador tiene suficientes patentes'
END
PRINT ''
GO

-- ==================================================
-- PASO 4: Listar patentes específicas de Administrador
-- ==================================================
PRINT '4️⃣ PATENTES ASIGNADAS A "Administrador"'
PRINT '===================================='

SELECT 
    p.Nombre AS Patente,
    p.DataKey,
    CASE p.TipoAcceso
        WHEN 0 THEN 'UI'
        WHEN 1 THEN 'Control'
        WHEN 2 THEN 'UseCases'
    END AS TipoAcceso
FROM Familia f
INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Administrador'
ORDER BY p.TipoAcceso, p.Nombre

PRINT ''
GO

-- ==================================================
-- PASO 5: Verificar patentes que faltan
-- ==================================================
PRINT '5️⃣ PATENTES QUE FALTAN EN "Administrador"'
PRINT '===================================='

SELECT 
    p.Nombre AS Patente_Faltante,
    p.DataKey,
    CASE p.TipoAcceso
        WHEN 0 THEN 'UI'
        WHEN 1 THEN 'Control'
        WHEN 2 THEN 'UseCases'
    END AS TipoAcceso
FROM Patente p
WHERE p.IdPatente NOT IN (
    SELECT fp.IdPatente
    FROM Familia f
    INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
    WHERE f.Nombre = 'Administrador'
)
ORDER BY p.TipoAcceso, p.Nombre

PRINT ''
GO

-- ==================================================
-- PASO 6: Verificar patentes esperadas para el menú
-- ==================================================
PRINT '6️⃣ VERIFICAR PATENTES CRÍTICAS (Las que necesita para los menús)'
PRINT '===================================='

DECLARE @PatentesEsperadas TABLE (Patente NVARCHAR(100))

INSERT INTO @PatentesEsperadas VALUES
    ('CREAR_PEDIDO'),
    ('MOSTRAR_PEDIDOS'),
    ('Crear_cliente'),
    ('Mostrar_clientes'),
    ('AGREGAR'),
    ('MODIFICAR'),
    ('ELIMINAR'),
    ('VER_PRODUCTOS'),
    ('Mostrar_Stock'),
    ('Mostrar_Proveedores'),
    ('Modificar_Proveedor'),
    ('Crear_Proveedor'),
    ('Crear_usuario'),
    ('Ver_usuarios'),
    ('Asignar_rol'),
    ('Modificar_Usuario'),
    ('Crear_rol'),
    ('Crear_patente'),
    ('Generar_Backup'),
    ('RestaurarBackup'),
    ('ReporteStockBajo'),
    ('ReporteProductosMasVendidos')

SELECT 
    pe.Patente,
    CASE 
        WHEN EXISTS (
            SELECT 1 
            FROM Familia f
            INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
            INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
            WHERE f.Nombre = 'Administrador' 
            AND p.Nombre = pe.Patente
        ) THEN '✅ Asignada'
        ELSE '❌ FALTANTE'
    END AS Estado
FROM @PatentesEsperadas pe
ORDER BY 
    CASE 
        WHEN EXISTS (
            SELECT 1 
            FROM Familia f
            INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
            INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
            WHERE f.Nombre = 'Administrador' 
            AND p.Nombre = pe.Patente
        ) THEN 0
        ELSE 1
    END,
    pe.Patente

PRINT ''
GO

-- ==================================================
-- PASO 7: Resumen y recomendación
-- ==================================================
PRINT '7️⃣ RESUMEN Y DIAGNÓSTICO'
PRINT '===================================='

DECLARE @TieneUsuario BIT = 0
DECLARE @TieneFamilia BIT = 0
DECLARE @CantidadPatentes INT = 0

-- Verificar usuario
IF EXISTS (SELECT 1 FROM Usuario WHERE UserName = 'adminNico')
    SET @TieneUsuario = 1

-- Verificar familia
IF EXISTS (
    SELECT 1 
    FROM Usuario u
    INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
    INNER JOIN Familia f ON uf.IdFamilia = f.IdFamilia
    WHERE u.UserName = 'adminNico' AND f.Nombre = 'Administrador'
)
    SET @TieneFamilia = 1

-- Contar patentes
SELECT @CantidadPatentes = COUNT(DISTINCT p.IdPatente)
FROM Familia f
INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Administrador'

PRINT ''
PRINT 'Estado del usuario adminNico:'
PRINT '=============================='
IF @TieneUsuario = 1
    PRINT '✅ Usuario existe'
ELSE
    PRINT '❌ Usuario NO existe'

IF @TieneFamilia = 1
    PRINT '✅ Tiene familia "Administrador" asignada'
ELSE
    PRINT '❌ NO tiene familia "Administrador" asignada'

PRINT 'Patentes en familia Administrador: ' + CAST(@CantidadPatentes AS VARCHAR(10))

PRINT ''
PRINT 'DIAGNÓSTICO:'
PRINT '============'

IF @TieneUsuario = 0
BEGIN
    PRINT '❌ PROBLEMA: El usuario adminNico no existe en la base de datos'
    PRINT '📝 SOLUCIÓN: Crear el usuario o verificar el nombre exacto'
END
ELSE IF @TieneFamilia = 0
BEGIN
    PRINT '❌ PROBLEMA: El usuario existe pero NO tiene la familia Administrador'
    PRINT '📝 SOLUCIÓN: Ejecutar el script 08_Asignar_Todas_Patentes_Administrador.sql'
END
ELSE IF @CantidadPatentes < 20
BEGIN
    PRINT '❌ PROBLEMA: La familia Administrador tiene POCAS patentes (' + CAST(@CantidadPatentes AS VARCHAR(10)) + ' de 22)'
    PRINT '📝 SOLUCIÓN: Ejecutar el script 08_Asignar_Todas_Patentes_Administrador.sql'
    PRINT ''
    PRINT '⚠️  Por eso solo ve "BÚSQUEDA" (que no requiere patente específica)'
END
ELSE
BEGIN
    PRINT '✅ Todo parece correcto en la base de datos'
    PRINT ''
    PRINT 'POSIBLES CAUSAS:'
    PRINT '1. Problema con el código C# al recuperar patentes'
    PRINT '2. Cache de sesión (cerrar y volver a iniciar sesión)'
    PRINT '3. Error en el mapeo de patentes en main.cs'
END

PRINT ''
PRINT '============================================'
PRINT '✅ Diagnóstico completado'
PRINT '============================================'
GO