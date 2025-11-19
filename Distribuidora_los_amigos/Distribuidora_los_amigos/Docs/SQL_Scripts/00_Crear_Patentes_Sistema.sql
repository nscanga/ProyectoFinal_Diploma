-- =============================================
-- Script: Crear Patentes del Sistema
-- Base de datos: Login
-- Descripción: Crea las 22 patentes estándar del sistema
--              DEBE ejecutarse ANTES de crear usuarios
--              Este es el PRIMER script a ejecutar en una BD nueva
-- =============================================

USE [Login]
GO

PRINT '============================================'
PRINT 'CREAR PATENTES DEL SISTEMA'
PRINT '============================================'
PRINT ''

-- Verificar si ya existen patentes
DECLARE @PatentesExistentes INT
SELECT @PatentesExistentes = COUNT(*) FROM Patente

IF @PatentesExistentes > 0
BEGIN
    PRINT '??  ADVERTENCIA: Ya existen ' + CAST(@PatentesExistentes AS VARCHAR(10)) + ' patentes en el sistema'
    PRINT '?? Este script solo insertará las patentes que NO existan'
    PRINT ''
END
ELSE
BEGIN
    PRINT '? Sistema limpio - Se crearán todas las patentes'
    PRINT ''
END

-- ==================================================
-- PATENTES DE TIPO UI (Visualización) - TipoAcceso = 0
-- ==================================================
PRINT '?? Creando Patentes de Visualización (UI)...'
PRINT '============================================'

-- Ver_usuarios
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Ver_usuarios')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Ver_usuarios', 'Ver_usuarios', 0)
    PRINT '? Ver_usuarios'
END
ELSE
    PRINT '??  Ver_usuarios ya existe'

-- VER_PRODUCTOS
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'VER_PRODUCTOS')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'VER_PRODUCTOS', 'VER_PRODUCTOS', 0)
    PRINT '? VER_PRODUCTOS'
END
ELSE
    PRINT '??  VER_PRODUCTOS ya existe'

-- Mostrar_Stock
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Mostrar_Stock')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Mostrar_Stock', 'Mostrar_Stock', 0)
    PRINT '? Mostrar_Stock'
END
ELSE
    PRINT '??  Mostrar_Stock ya existe'

-- Mostrar_clientes
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Mostrar_clientes')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Mostrar_clientes', 'Mostrar_clientes', 0)
    PRINT '? Mostrar_clientes'
END
ELSE
    PRINT '??  Mostrar_clientes ya existe'

-- Mostrar_Proveedores
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Mostrar_Proveedores')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Mostrar_Proveedores', 'Mostrar_Proveedores', 0)
    PRINT '? Mostrar_Proveedores'
END
ELSE
    PRINT '??  Mostrar_Proveedores ya existe'

-- MOSTRAR_PEDIDOS
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'MOSTRAR_PEDIDOS')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'MOSTRAR_PEDIDOS', 'MOSTRAR_PEDIDOS', 0)
    PRINT '? MOSTRAR_PEDIDOS'
END
ELSE
    PRINT '??  MOSTRAR_PEDIDOS ya existe'

-- ReporteStockBajo
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'ReporteStockBajo')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'ReporteStockBajo', 'ReporteStockBajo', 0)
    PRINT '? ReporteStockBajo'
END
ELSE
    PRINT '??  ReporteStockBajo ya existe'

-- ReporteProductosMasVendidos
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'ReporteProductosMasVendidos')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'ReporteProductosMasVendidos', 'ReporteProductosMasVendidos', 0)
    PRINT '? ReporteProductosMasVendidos'
END
ELSE
    PRINT '??  ReporteProductosMasVendidos ya existe'

PRINT ''

-- ==================================================
-- PATENTES DE TIPO CONTROL (Administración) - TipoAcceso = 1
-- ==================================================
PRINT '?? Creando Patentes de Administración (Control)...'
PRINT '=================================================='

-- Crear_usuario
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Crear_usuario')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Crear_usuario', 'Crear_usuario', 1)
    PRINT '? Crear_usuario'
END
ELSE
    PRINT '??  Crear_usuario ya existe'

-- MODIFICAR
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'MODIFICAR')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'MODIFICAR', 'MODIFICAR', 1)
    PRINT '? MODIFICAR'
END
ELSE
    PRINT '??  MODIFICAR ya existe'

-- Crear_rol
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Crear_rol')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Crear_rol', 'Crear_rol', 1)
    PRINT '? Crear_rol'
END
ELSE
    PRINT '??  Crear_rol ya existe'

-- Asignar_rol
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Asignar_rol')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Asignar_rol', 'Asignar_rol', 1)
    PRINT '? Asignar_rol'
END
ELSE
    PRINT '??  Asignar_rol ya existe'

-- CREAR_PEDIDO
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'CREAR_PEDIDO')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'CREAR_PEDIDO', 'CREAR_PEDIDO', 1)
    PRINT '? CREAR_PEDIDO'
END
ELSE
    PRINT '??  CREAR_PEDIDO ya existe'

-- AGREGAR
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'AGREGAR')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'AGREGAR', 'AGREGAR', 1)
    PRINT '? AGREGAR'
END
ELSE
    PRINT '??  AGREGAR ya existe'

-- Modificar_Usuario
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Modificar_Usuario')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Modificar_Usuario', 'Modificar_Usuario', 1)
    PRINT '? Modificar_Usuario'
END
ELSE
    PRINT '??  Modificar_Usuario ya existe'

-- Modificar_Proveedor
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Modificar_Proveedor')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Modificar_Proveedor', 'Modificar_Proveedor', 1)
    PRINT '? Modificar_Proveedor'
END
ELSE
    PRINT '??  Modificar_Proveedor ya existe'

-- Generar_Backup
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Generar_Backup')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Generar_Backup', 'Generar_Backup', 1)
    PRINT '? Generar_Backup'
END
ELSE
    PRINT '??  Generar_Backup ya existe'

-- RestaurarBackup
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'RestaurarBackup')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'RestaurarBackup', 'RestaurarBackup', 1)
    PRINT '? RestaurarBackup'
END
ELSE
    PRINT '??  RestaurarBackup ya existe'

-- Crear_patente
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Crear_patente')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Crear_patente', 'Crear_patente', 1)
    PRINT '? Crear_patente'
END
ELSE
    PRINT '??  Crear_patente ya existe'

-- ELIMINAR
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'ELIMINAR')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'ELIMINAR', 'ELIMINAR', 1)
    PRINT '? ELIMINAR'
END
ELSE
    PRINT '??  ELIMINAR ya existe'

-- Crear_Proveedor
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Crear_Proveedor')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Crear_Proveedor', 'Crear_Proveedor', 1)
    PRINT '? Crear_Proveedor'
END
ELSE
    PRINT '??  Crear_Proveedor ya existe'

-- Crear_cliente
IF NOT EXISTS (SELECT 1 FROM Patente WHERE Nombre = 'Crear_cliente')
BEGIN
    INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
    VALUES (NEWID(), 'Crear_cliente', 'Crear_cliente', 1)
    PRINT '? Crear_cliente'
END
ELSE
    PRINT '??  Crear_cliente ya existe'

PRINT ''

-- ==================================================
-- RESUMEN FINAL
-- ==================================================
PRINT '============================================'
PRINT '? PROCESO COMPLETADO'
PRINT '============================================'
PRINT ''

DECLARE @TotalPatentesFinales INT
SELECT @TotalPatentesFinales = COUNT(*) FROM Patente

PRINT '?? RESUMEN:'
PRINT '   Total de patentes en el sistema: ' + CAST(@TotalPatentesFinales AS VARCHAR(10))
PRINT ''

SELECT 
    COUNT(*) AS Total,
    SUM(CASE WHEN TipoAcceso = 0 THEN 1 ELSE 0 END) AS UI_Visualizacion,
    SUM(CASE WHEN TipoAcceso = 1 THEN 1 ELSE 0 END) AS Control_Administracion,
    SUM(CASE WHEN TipoAcceso = 2 THEN 1 ELSE 0 END) AS UseCases_Especiales
FROM Patente

PRINT ''
PRINT '?? Patentes creadas:'
SELECT 
    ROW_NUMBER() OVER (ORDER BY TipoAcceso, Nombre) AS '#',
    Nombre,
    CASE TipoAcceso
        WHEN 0 THEN 'UI (Visualización)'
        WHEN 1 THEN 'Control (Administración)'
        WHEN 2 THEN 'UseCases (Especiales)'
    END AS Tipo
FROM Patente
ORDER BY TipoAcceso, Nombre

PRINT ''
PRINT '============================================'
PRINT '?? PRÓXIMOS PASOS:'
PRINT '============================================'
PRINT ''
PRINT '1?? Ahora puede ejecutar la aplicación'
PRINT '   Al no tener usuarios, se creará automáticamente el usuario admin'
PRINT '   con TODAS estas patentes asignadas'
PRINT ''
PRINT '2?? O puede ejecutar el script:'
PRINT '   00_Crear_Usuario_Admin_Default.sql'
PRINT '   Para crear manualmente el usuario admin'
PRINT ''
PRINT '3?? Luego ejecutar:'
PRINT '   11_Asignar_Rol_Admin_A_Usuario_Existente.sql'
PRINT '   Para asignar todas las patentes al admin'
PRINT ''
PRINT '============================================'
GO
