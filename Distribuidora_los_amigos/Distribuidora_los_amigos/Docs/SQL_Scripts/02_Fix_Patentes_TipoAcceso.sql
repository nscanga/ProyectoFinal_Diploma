-- ============================================
-- Script para corregir TipoAcceso de Patentes
-- ============================================
-- Problema: Todas las patentes tienen TipoAcceso = 1 (Control)
-- Solución: Actualizar según el propósito de cada patente
--
-- Valores del Enum TipoAcceso:
-- 0 = UI (Interfaz de Usuario - controles visuales)
-- 1 = Control (Acceso administrativo/gestión)
-- 2 = UseCases (Casos de uso específicos)
-- ============================================

USE [Login]
GO

-- ==================================================
-- PASO 1: Ver estado actual de las patentes
-- ==================================================
PRINT '============================================'
PRINT 'ESTADO ACTUAL DE PATENTES'
PRINT '============================================'

SELECT 
    Nombre,
    DataKey,
    TipoAcceso,
    CASE TipoAcceso
        WHEN 0 THEN 'UI'
        WHEN 1 THEN 'Control'
        WHEN 2 THEN 'UseCases'
        ELSE 'Desconocido'
    END AS TipoAccesoNombre
FROM [dbo].[Patente]
ORDER BY Nombre
GO

-- ==================================================
-- PASO 2: Actualizar patentes de UI (lectura/visualización)
-- ==================================================
PRINT ''
PRINT '============================================'
PRINT 'ACTUALIZANDO PATENTES DE TIPO UI'
PRINT '============================================'

-- Patentes de visualización/consulta -> TipoAcceso = 0 (UI)
UPDATE [dbo].[Patente]
SET TipoAcceso = 0
WHERE DataKey IN (
    'Ver_usuarios',
    'VER_PRODUCTOS',
    'Mostrar_Stock',
    'Mostrar_clientes',
    'Mostrar_Proveedores',
    'MOSTRAR_PEDIDOS',
    'ReporteStockBajo',
    'ReporteProductosMasVendidos'
)

PRINT '? Patentes UI actualizadas'
GO

-- ==================================================
-- PASO 3: Actualizar patentes de Control (gestión/administración)
-- ==================================================
PRINT ''
PRINT '============================================'
PRINT 'ACTUALIZANDO PATENTES DE TIPO CONTROL'
PRINT '============================================'

-- Patentes administrativas -> TipoAcceso = 1 (Control)
UPDATE [dbo].[Patente]
SET TipoAcceso = 1
WHERE DataKey IN (
    'Crear_usuario',
    'MODIFICAR',
    'Crear_rol',
    'Asignar_rol',
    'CREAR_PEDIDO',
    'AGREGAR',
    'Modificar_Usuario',
    'Modificar_Proveedor',
    'General_Backup',
    'RestaurarBackup',
    'Crear_patente',
    'ELIMINAR',
    'Crear_Proveedor',
    'Crear_cliente'
)

PRINT '? Patentes Control actualizadas'
GO

-- ==================================================
-- PASO 4: Verificar cambios
-- ==================================================
PRINT ''
PRINT '============================================'
PRINT 'ESTADO DESPUÉS DE LA ACTUALIZACIÓN'
PRINT '============================================'

SELECT 
    Nombre,
    DataKey,
    TipoAcceso,
    CASE TipoAcceso
        WHEN 0 THEN 'UI (Visualización)'
        WHEN 1 THEN 'Control (Administración)'
        WHEN 2 THEN 'UseCases'
        ELSE 'Desconocido'
    END AS TipoAccesoNombre
FROM [dbo].[Patente]
ORDER BY TipoAcceso, Nombre
GO

-- ==================================================
-- PASO 5: Verificar familias y sus patentes
-- ==================================================
PRINT ''
PRINT '============================================'
PRINT 'FAMILIAS Y SUS PATENTES'
PRINT '============================================'

SELECT 
    f.Nombre AS Familia,
    p.Nombre AS Patente,
    CASE p.TipoAcceso
        WHEN 0 THEN 'UI'
        WHEN 1 THEN 'Control'
        WHEN 2 THEN 'UseCases'
    END AS TipoAcceso
FROM [dbo].[Familia] f
INNER JOIN [dbo].[Familia_Patente] fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN [dbo].[Patente] p ON fp.IdPatente = p.IdPatente
ORDER BY f.Nombre, p.TipoAcceso, p.Nombre
GO

PRINT ''
PRINT '? Script completado exitosamente'
PRINT '============================================'
GO
