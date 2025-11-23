# ?? Solución de Problemas de Permisos en SQL Server

## ?? Problema: "Operating system error 5 (Acceso denegado)"

Este error ocurre cuando SQL Server no puede acceder a un archivo porque:
1. El archivo está en una carpeta protegida (ej: `C:\Users\...`)
2. La cuenta de servicio de SQL Server no tiene permisos
3. El archivo está bloqueado por otro proceso

## ? Solución Automática Implementada

La aplicación ahora **resuelve automáticamente** este problema:

### Para BACKUP
1. Crea el backup en la carpeta de SQL Server
2. Lee el archivo usando `OPENROWSET`
3. Lo guarda en la carpeta que seleccionaste
4. Limpia el archivo temporal

### Para RESTORE
1. Lee el archivo .bak de tu carpeta
2. Lo transfiere a la carpeta de SQL Server
3. Ejecuta el restore desde ahí
4. Limpia el archivo temporal

## ?? Cómo Funciona la Transferencia Automática

### Método 1: Ole Automation (Preferido)

```sql
-- Se habilita temporalmente
EXEC sp_configure 'Ole Automation Procedures', 1;

-- Se usa ADODB.Stream para copiar el archivo
DECLARE @ObjectToken INT;
EXEC sp_OACreate 'ADODB.Stream', @ObjectToken OUTPUT;
-- ... copia el archivo ...

-- Se deshabilita inmediatamente después
EXEC sp_configure 'Ole Automation Procedures', 0;
```

**Ventajas:**
- ? Más rápido
- ? No requiere xp_cmdshell
- ? Más seguro
- ? Funciona en la mayoría de los casos

### Método 2: PowerShell (Fallback)

Si el método 1 falla, se usa PowerShell:

```sql
-- Se habilita xp_cmdshell temporalmente
EXEC sp_configure 'xp_cmdshell', 1;

-- Se ejecuta PowerShell para copiar
EXEC xp_cmdshell 'powershell.exe -File script.ps1'

-- Se deshabilita inmediatamente
EXEC sp_configure 'xp_cmdshell', 0;
```

**Ventajas:**
- ? Funciona cuando Ole Automation falla
- ? Compatible con Windows
- ? Se limpia automáticamente

## ?? Flujo de Seguridad

```
Inicio del Restore
    ?
Verificar archivo existe
    ?
Conectar a SQL Server (master)
    ?
Intentar Método 1: Ole Automation
    ?? ? Éxito ? Continuar con restore
    ?? ? Fallo ? Intentar Método 2
            ?
        Intentar Método 2: PowerShell
            ?? ? Éxito ? Continuar con restore
            ?? ? Fallo ? Mostrar error al usuario
    ?
Poner DB en modo SINGLE_USER
    ?
Ejecutar RESTORE
    ?
Volver DB a modo MULTI_USER
    ?
Limpiar archivos temporales
    ?
Deshabilitar características habilitadas
    ?
Fin
```

## ?? Configuración de SQL Server

### Verificar Estado de Ole Automation

```sql
-- Ver configuración actual
EXEC sp_configure 'Ole Automation Procedures';
GO

-- Resultado esperado:
-- Si está en 0 (deshabilitado) ? La app lo habilita temporalmente
-- Si está en 1 (habilitado) ? La app lo usa directamente
```

### Verificar Estado de xp_cmdshell

```sql
-- Ver configuración actual
EXEC sp_configure 'xp_cmdshell';
GO

-- Resultado esperado:
-- Si está en 0 (deshabilitado) ? La app lo habilita si es necesario
-- Si está en 1 (habilitado) ? La app lo usa si método 1 falla
```

### ?? Importante: Seguridad

La aplicación:
- ? Solo habilita estas características **temporalmente**
- ? Las **deshabilita inmediatamente** después de usarlas
- ? **No deja** configuraciones inseguras habilitadas
- ? Usa **try-finally** para garantizar limpieza

## ??? Troubleshooting

### Error: "Ole Automation Procedures cannot be used"

**Causa:** Ole Automation está deshabilitado y no se pudo habilitar.

**Solución automática:** El sistema usará PowerShell automáticamente.

**Solución manual (si ambos fallan):**
```sql
-- Habilitar manualmente
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'Ole Automation Procedures', 1;
RECONFIGURE;

-- Luego volver a intentar el restore
```

### Error: "xp_cmdshell is disabled"

**Causa:** xp_cmdshell está deshabilitado y no se pudo habilitar.

**Solución:** El sistema intentó ambos métodos. Opciones:

**Opción 1: Habilitar xp_cmdshell manualmente**
```sql
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'xp_cmdshell', 1;
RECONFIGURE;
```

**Opción 2: Copiar manualmente el archivo**
```powershell
# Desde PowerShell como administrador
Copy-Item "C:\Users\Nicolas\Documents\Backup\archivo.bak" `
          "C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\Backup\"
```

Luego ejecutar desde la aplicación o SQL Management Studio:
```sql
RESTORE DATABASE [DistribuidoraLosAmigos]
FROM DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\Backup\archivo.bak'
WITH REPLACE, RECOVERY;
```

### Error: "Access is denied" al copiar archivo

**Causa:** La cuenta del usuario que ejecuta la aplicación no tiene permisos.

**Solución:**

**Opción 1: Ejecutar la aplicación como Administrador**
- Click derecho en el ejecutable
- Seleccionar "Ejecutar como administrador"
- Intentar restore nuevamente

**Opción 2: Dar permisos a la carpeta de SQL Server**
```powershell
# Desde PowerShell como administrador
$sqlBackupFolder = "C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\Backup"

# Dar permisos de escritura al usuario actual
$acl = Get-Acl $sqlBackupFolder
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule(
    $env:USERNAME, "FullControl", "Allow"
)
$acl.SetAccessRule($accessRule)
Set-Acl $sqlBackupFolder $acl
```

### Error: "Database is in use"

**Causa:** Otras conexiones están usando la base de datos.

**Solución automática:** La aplicación ejecuta:
```sql
ALTER DATABASE [nombre] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
```

**Solución manual (si falla):**
```sql
-- Verificar conexiones activas
SELECT session_id, login_name, host_name, program_name
FROM sys.dm_exec_sessions
WHERE database_id = DB_ID('DistribuidoraLosAmigos');

-- Cerrar conexiones específicas
KILL 52; -- Reemplazar con session_id real

-- Intentar restore nuevamente
```

## ?? Carpetas de SQL Server por Defecto

### Windows

| Versión | Carpeta de Backup |
|---------|-------------------|
| SQL Server 2019 | `C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\Backup` |
| SQL Server 2017 | `C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\Backup` |
| SQL Server 2016 | `C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS\MSSQL\Backup` |
| SQL Server 2014 | `C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\Backup` |

### Linux

```
/var/opt/mssql/data
```

## ?? Verificar Carpeta de Backup de SQL Server

```sql
-- Obtener la carpeta de backup configurada
EXEC master.dbo.xp_instance_regread 
    N'HKEY_LOCAL_MACHINE',
    N'Software\Microsoft\MSSQLServer\MSSQLServer',
    N'BackupDirectory';
```

## ?? Mejores Prácticas

### Para Usuarios

1. **No requiere acción especial** - Todo es automático
2. Puedes seleccionar cualquier carpeta para backup/restore
3. La aplicación maneja los permisos automáticamente

### Para Administradores

1. **No deshabilitar Ole Automation** permanentemente
2. **No deshabilitar xp_cmdshell** permanentemente (la app lo hace)
3. Verificar que SQL Server tiene permisos en su carpeta de backup
4. Mantener espacio libre en disco para backups

### Para Desarrolladores

El código ya implementa:
- ? Detección automática de método disponible
- ? Fallback entre múltiples métodos
- ? Limpieza automática de configuraciones
- ? Manejo de errores detallado
- ? Logging de operaciones

## ?? Logs

Todas las operaciones se registran en:

```
C:\Logs\
??? app.log      - Log general
??? error.log    - Errores detallados
??? info.log     - Operaciones informativas
```

### Ejemplo de log de restore exitoso:

```
[INFO] 2024-11-21 15:49:00 - Iniciando restore de base de datos
[INFO] 2024-11-21 15:49:01 - Transferiendo archivo a servidor SQL
[INFO] 2024-11-21 15:49:02 - Usando método: Ole Automation
[INFO] 2024-11-21 15:49:15 - Archivo transferido exitosamente
[INFO] 2024-11-21 15:49:16 - Ejecutando RESTORE DATABASE
[INFO] 2024-11-21 15:49:45 - Restore completado exitosamente
[INFO] 2024-11-21 15:49:46 - Limpiando archivos temporales
```

### Ejemplo de log con fallback:

```
[INFO] 2024-11-21 15:49:00 - Iniciando restore de base de datos
[INFO] 2024-11-21 15:49:01 - Transferiendo archivo a servidor SQL
[WARNING] 2024-11-21 15:49:02 - Ole Automation falló, usando PowerShell
[INFO] 2024-11-21 15:49:15 - Archivo transferido exitosamente
[INFO] 2024-11-21 15:49:16 - Ejecutando RESTORE DATABASE
[INFO] 2024-11-21 15:49:45 - Restore completado exitosamente
```

## ?? Contacto y Soporte

Si después de seguir estos pasos aún tienes problemas:

1. Revisar logs en `C:\Logs\error.log`
2. Verificar versión de SQL Server
3. Confirmar que tienes permisos de administrador
4. Intentar restore desde SQL Management Studio manualmente

---

**Nota:** Esta solución fue diseñada específicamente para SQL Server Express Edition, pero funciona en todas las ediciones de SQL Server.
