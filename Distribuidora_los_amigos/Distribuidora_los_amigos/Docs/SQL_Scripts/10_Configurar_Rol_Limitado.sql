-- ============================================
-- CONFIGURAR ROL LIMITADO (Ejemplo: Vendedor)
-- ============================================
-- Este script configura un rol con permisos LIMITADOS
-- Puedes usarlo como plantilla para crear roles específicos
-- ============================================

USE [Login]
GO

-- ?? CONFIGURACIÓN
DECLARE @NombreRol NVARCHAR(100) = 'Vendedor'

PRINT '============================================'
PRINT 'CONFIGURANDO ROL: ' + @NombreRol
PRINT '============================================'
PRINT ''

-- ==================================================
-- PASO 1: Crear el rol si no existe
-- ==================================================
PRINT '1?? Verificando/Creando rol...'

IF NOT EXISTS (SELECT 1 FROM Familia WHERE Nombre = @NombreRol)
BEGIN
    INSERT INTO Familia (IdFamilia, Nombre)
    VALUES (NEWID(), @NombreRol)
    PRINT '? Rol "' + @NombreRol + '" creado'
END
ELSE
BEGIN
    PRINT '? Rol "' + @NombreRol + '" ya existe'
END
PRINT ''
GO

-- ==================================================
-- PASO 2: LIMPIAR patentes existentes del rol
-- ==================================================
DECLARE @NombreRol NVARCHAR(100) = 'Vendedor'

PRINT '2?? Limpiando patentes antiguas...'

DELETE FROM Familia_Patente
WHERE IdFamilia = (SELECT IdFamilia FROM Familia WHERE Nombre = @NombreRol)

PRINT '? Patentes antiguas eliminadas'
PRINT ''
GO

-- ==================================================
-- PASO 3: Asignar SOLO las patentes necesarias
-- ==================================================
DECLARE @NombreRol NVARCHAR(100) = 'Vendedor'

PRINT '3?? Asignando patentes específicas...'

DECLARE @IdRol UNIQUEIDENTIFIER
SELECT @IdRol = IdFamilia FROM Familia WHERE Nombre = @NombreRol

-- ?? DEFINIR QUÉ PATENTES NECESITA ESTE ROL
-- Modifica esta lista según las necesidades del rol

DECLARE @PatentesNecesarias TABLE (NombrePatente NVARCHAR(100))

-- Para un VENDEDOR típico:
INSERT INTO @PatentesNecesarias VALUES
    ('CREAR_PEDIDO'),           -- Puede crear pedidos
    ('MOSTRAR_PEDIDOS'),        -- Puede ver pedidos
    ('Crear_cliente'),          -- Puede crear clientes
    ('Mostrar_clientes'),       -- Puede ver clientes
    ('VER_PRODUCTOS'),          -- Puede consultar productos
    ('Mostrar_Stock')           -- Puede consultar stock

-- NO incluye:
-- - Crear_usuario, Ver_usuarios, etc. (gestión de usuarios)
-- - Generar_Backup, RestaurarBackup (backup)
-- - Crear_rol, Crear_patente (administración)
-- - MODIFICAR, ELIMINAR (modificaciones peligrosas)

-- Asignar las patentes
DECLARE @NombrePatente NVARCHAR(100)
DECLARE @IdPatente UNIQUEIDENTIFIER
DECLARE @PatentesAsignadas INT = 0

DECLARE patente_cursor CURSOR FOR
    SELECT NombrePatente FROM @PatentesNecesarias

OPEN patente_cursor
FETCH NEXT FROM patente_cursor INTO @NombrePatente

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Buscar la patente
    SELECT @IdPatente = IdPatente 
    FROM Patente 
    WHERE Nombre = @NombrePatente
    
    IF @IdPatente IS NOT NULL
    BEGIN
        -- Asignar si no existe
        IF NOT EXISTS (
            SELECT 1 FROM Familia_Patente 
            WHERE IdFamilia = @IdRol AND IdPatente = @IdPatente
        )
        BEGIN
            INSERT INTO Familia_Patente (IdFamiliaPatente, IdFamilia, IdPatente)
            VALUES (NEWID(), @IdRol, @IdPatente)
            
            PRINT '  ? Asignada: ' + @NombrePatente
            SET @PatentesAsignadas = @PatentesAsignadas + 1
        END
    END
    ELSE
    BEGIN
        PRINT '  ??  No encontrada: ' + @NombrePatente
    END
    
    FETCH NEXT FROM patente_cursor INTO @NombrePatente
END

CLOSE patente_cursor
DEALLOCATE patente_cursor

PRINT ''
PRINT 'Total de patentes asignadas: ' + CAST(@PatentesAsignadas AS VARCHAR(10))
PRINT ''
GO

-- ==================================================
-- PASO 4: Verificar configuración
-- ==================================================
DECLARE @NombreRol NVARCHAR(100) = 'Vendedor'

PRINT '4?? Verificando configuración final...'
PRINT ''

SELECT 
    p.Nombre AS Patente,
    CASE p.TipoAcceso
        WHEN 0 THEN 'UI'
        WHEN 1 THEN 'Control'
        WHEN 2 THEN 'UseCases'
    END AS TipoAcceso
FROM Familia f
INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = @NombreRol
ORDER BY p.TipoAcceso, p.Nombre

PRINT ''
PRINT '============================================'
PRINT '? Configuración completada'
PRINT '============================================'
PRINT ''
PRINT 'MENÚS QUE VERÁ ESTE ROL:'
PRINT '- PEDIDOS ? Crear Pedido, Mostrar Pedidos'
PRINT '- CLIENTE ? Crear Cliente, Mostrar Clientes'
PRINT '- PRODUCTOS ? Ver Productos'
PRINT '- STOCK ? Mostrar Stock'
PRINT ''
PRINT 'MENÚS QUE NO VERÁ:'
PRINT '- GESTIÓN DE USUARIOS ?'
PRINT '- BACKUP Y RESTORE ?'
PRINT '- REPORTES ? (si no tiene las patentes)'
PRINT '- Modificar/Eliminar Productos ?'
PRINT ''
GO

-- ==================================================
-- EJEMPLO: Asignar usuario a este rol
-- ==================================================
/*
-- Descomenta este bloque para asignar un usuario

DECLARE @NombreUsuario NVARCHAR(100) = 'vendedor1'
DECLARE @NombreRol NVARCHAR(100) = 'Vendedor'

DECLARE @IdUsuario UNIQUEIDENTIFIER
DECLARE @IdRol UNIQUEIDENTIFIER

SELECT @IdUsuario = IdUsuario FROM Usuario WHERE UserName = @NombreUsuario
SELECT @IdRol = IdFamilia FROM Familia WHERE Nombre = @NombreRol

IF @IdUsuario IS NOT NULL AND @IdRol IS NOT NULL
BEGIN
    -- Eliminar roles anteriores
    DELETE FROM Usuario_Familia WHERE IdUsuario = @IdUsuario
    
    -- Asignar nuevo rol
    INSERT INTO Usuario_Familia (IdUsuarioFamilia, IdUsuario, IdFamilia)
    VALUES (NEWID(), @IdUsuario, @IdRol)
    
    PRINT '? Usuario "' + @NombreUsuario + '" asignado al rol "' + @NombreRol + '"'
END
ELSE
BEGIN
    PRINT '? Error: Usuario o rol no encontrado'
END
*/
GO
