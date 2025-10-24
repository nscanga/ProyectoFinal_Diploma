-- =============================================
-- Script: Restore Manual de Base de Datos
-- Descripción: Script de emergencia para restaurar bases de datos manualmente
-- Uso: Para casos donde la aplicación no puede ejecutar el restore
-- =============================================

-- ?? ADVERTENCIA: Este script REEMPLAZARÁ completamente las bases de datos actuales
-- ?? Asegúrese de tener un backup actual antes de ejecutar

-- =============================================
-- PASO 1: Configurar variables
-- =============================================
-- Modifique estas rutas según su configuración
DECLARE @BackupPath1 NVARCHAR(500) = N'C:\Backups\DistribuidoraLosAmigos.bak'
DECLARE @BackupPath2 NVARCHAR(500) = N'C:\Backups\Login.bak'
DECLARE @BackupPath3 NVARCHAR(500) = N'C:\Backups\Bitacora.bak'

-- =============================================
-- PASO 2: Verificar que los archivos existen
-- =============================================
PRINT '============================================='
PRINT 'Verificando archivos de backup...'
PRINT '============================================='

-- Verificar archivo 1
EXEC master.dbo.xp_fileexist @BackupPath1
-- Verificar archivo 2
EXEC master.dbo.xp_fileexist @BackupPath2
-- Verificar archivo 3
EXEC master.dbo.xp_fileexist @BackupPath3

PRINT 'Si los archivos existen, verá 1 en la primera columna'
PRINT ''

-- =============================================
-- PASO 3: Listar contenido de los backups
-- =============================================
PRINT '============================================='
PRINT 'Contenido de los backups:'
PRINT '============================================='

-- Backup 1
PRINT '--- DistribuidoraLosAmigos ---'
RESTORE HEADERONLY FROM DISK = @BackupPath1

-- Backup 2
PRINT '--- Login ---'
RESTORE HEADERONLY FROM DISK = @BackupPath2

-- Backup 3
PRINT '--- Bitacora ---'
RESTORE HEADERONLY FROM DISK = @BackupPath3

PRINT ''
PRINT '?? PAUSA: Revise la información anterior antes de continuar'
PRINT '?? Para continuar, ejecute el siguiente bloque'
PRINT ''

-- =============================================
-- PASO 4: RESTAURAR - DistribuidoraLosAmigos
-- =============================================
-- ?? DESCOMENTAR PARA EJECUTAR
/*
USE master;
GO

PRINT '============================================='
PRINT 'Restaurando DistribuidoraLosAmigos...'
PRINT '============================================='

-- Cerrar todas las conexiones
ALTER DATABASE [DistribuidoraLosAmigos] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- Restaurar la base de datos
RESTORE DATABASE [DistribuidoraLosAmigos]
FROM DISK = N'C:\Backups\DistribuidoraLosAmigos.bak'
WITH REPLACE, RECOVERY, STATS = 10;
GO

-- Volver a modo multiusuario
ALTER DATABASE [DistribuidoraLosAmigos] SET MULTI_USER;
GO

PRINT '? DistribuidoraLosAmigos restaurada correctamente'
PRINT ''
*/

-- =============================================
-- PASO 5: RESTAURAR - Login
-- =============================================
-- ?? DESCOMENTAR PARA EJECUTAR
/*
USE master;
GO

PRINT '============================================='
PRINT 'Restaurando Login...'
PRINT '============================================='

-- Cerrar todas las conexiones
ALTER DATABASE [Login] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- Restaurar la base de datos
RESTORE DATABASE [Login]
FROM DISK = N'C:\Backups\Login.bak'
WITH REPLACE, RECOVERY, STATS = 10;
GO

-- Volver a modo multiusuario
ALTER DATABASE [Login] SET MULTI_USER;
GO

PRINT '? Login restaurada correctamente'
PRINT ''
*/

-- =============================================
-- PASO 6: RESTAURAR - Bitacora
-- =============================================
-- ?? DESCOMENTAR PARA EJECUTAR
/*
USE master;
GO

PRINT '============================================='
PRINT 'Restaurando Bitacora...'
PRINT '============================================='

-- Cerrar todas las conexiones
ALTER DATABASE [Bitacora] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- Restaurar la base de datos
RESTORE DATABASE [Bitacora]
FROM DISK = N'C:\Backups\Bitacora.bak'
WITH REPLACE, RECOVERY, STATS = 10;
GO

-- Volver a modo multiusuario
ALTER DATABASE [Bitacora] SET MULTI_USER;
GO

PRINT '? Bitacora restaurada correctamente'
PRINT ''
*/

-- =============================================
-- PASO 7: Verificar las bases de datos restauradas
-- =============================================
-- ?? DESCOMENTAR DESPUÉS DE RESTAURAR
/*
PRINT '============================================='
PRINT 'Verificando bases de datos restauradas...'
PRINT '============================================='

-- Verificar DistribuidoraLosAmigos
SELECT 
    name AS 'Base de Datos',
    state_desc AS 'Estado',
    user_access_desc AS 'Acceso',
    recovery_model_desc AS 'Modelo de Recuperación',
    create_date AS 'Fecha Creación'
FROM sys.databases
WHERE name IN ('DistribuidoraLosAmigos', 'Login', 'Bitacora')

PRINT ''
PRINT '? Todas las bases de datos restauradas correctamente'
PRINT '?? IMPORTANTE: Reinicie la aplicación para que tome los cambios'
*/

-- =============================================
-- SCRIPTS DE EMERGENCIA
-- =============================================

-- Script 1: Forzar cierre de todas las conexiones a una base de datos
/*
USE master;
GO

DECLARE @DatabaseName NVARCHAR(128) = 'DistribuidoraLosAmigos'

DECLARE @SQL VARCHAR(MAX) = ''
SELECT @SQL = @SQL + 'KILL ' + CONVERT(VARCHAR(10), session_id) + ';'
FROM sys.dm_exec_sessions
WHERE database_id = DB_ID(@DatabaseName)
AND session_id <> @@SPID

EXEC(@SQL)

PRINT 'Todas las conexiones cerradas'
*/

-- Script 2: Verificar integridad de una base de datos después del restore
/*
USE DistribuidoraLosAmigos;
GO

DBCC CHECKDB WITH NO_INFOMSGS;

PRINT '? Verificación de integridad completada'
*/

-- Script 3: Listar usuarios conectados a las bases de datos
/*
SELECT 
    DB_NAME(database_id) AS 'Base de Datos',
    COUNT(*) AS 'Conexiones Activas',
    login_name AS 'Usuario'
FROM sys.dm_exec_sessions
WHERE database_id IN (DB_ID('DistribuidoraLosAmigos'), DB_ID('Login'), DB_ID('Bitacora'))
GROUP BY DB_NAME(database_id), login_name
ORDER BY DB_NAME(database_id), COUNT(*) DESC
*/

-- =============================================
-- NOTAS IMPORTANTES
-- =============================================
-- 1. Este script debe ejecutarse en SQL Server Management Studio
-- 2. Requiere permisos de sysadmin o dbcreator
-- 3. Cierre TODAS las aplicaciones antes de ejecutar el restore
-- 4. Los backups deben ser compatibles con la versión de SQL Server
-- 5. Las rutas de los archivos .bak deben ser accesibles por SQL Server
-- 6. Después del restore, REINICIE la aplicación
-- 7. Si algo sale mal, tenga un backup de emergencia disponible
-- =============================================

PRINT ''
PRINT '============================================='
PRINT 'Script de Restore Manual cargado'
PRINT '============================================='
PRINT 'Para ejecutar:'
PRINT '1. Verifique los archivos (ejecute PASO 2 y 3)'
PRINT '2. Descomente y ejecute los bloques de RESTORE'
PRINT '3. Verifique el resultado (PASO 7)'
PRINT '4. Reinicie la aplicación'
PRINT ''
PRINT '?? PRECAUCIÓN: Esta operación no se puede deshacer'
PRINT '============================================='
GO
