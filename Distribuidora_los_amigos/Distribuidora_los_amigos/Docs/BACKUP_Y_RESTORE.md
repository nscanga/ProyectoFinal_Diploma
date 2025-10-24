# ?? Sistema de Backup y Restore - Distribuidora Los Amigos

## ?? Resumen

El sistema incluye funcionalidad completa de **Backup (Respaldo)** y **Restore (Restauraci�n)** de todas las bases de datos del sistema.

## ?? Bases de Datos Incluidas

El sistema realiza operaciones sobre las siguientes bases de datos:

1. **DistribuidoraLosAmigos** - Base de datos principal del sistema
2. **Login** - Base de datos de usuarios y autenticaci�n
3. **Bitacora** - Base de datos de logs y auditor�a

## ?? Funcionalidad de Backup

### C�mo Crear un Backup

1. **Acceder al formulario**:
   - Men� ? Herramientas ? Generar Backup

2. **Seleccionar ubicaci�n**:
   - Hacer clic en "Crear BackUp"
   - Seleccionar la carpeta destino
   - Confirmar la operaci�n

3. **Resultado**:
   - Se crear�n 3 archivos .bak (uno por cada base de datos)
   - Los archivos incluir�n fecha y hora en el nombre
   - Se registrar� la operaci�n en los logs

### Archivos Generados

```
[Carpeta seleccionada]/
??? DistribuidoraLosAmigos_2024-01-15_14-30-00.bak
??? Login_2024-01-15_14-30-05.bak
??? Bitacora_2024-01-15_14-30-10.bak
```

### Rutas de Backup Configuradas

Las rutas predeterminadas se configuran en `App.config`:

```xml
<add key="BackupPath1" value="C:\SQLBackupTemp\"/>
<add key="BackupPath2" value="D:\SQLBackupTemp\"/>
<add key="BackupPath3" value="E:\SQLBackupTemp\"/>
```

## ?? Funcionalidad de Restore

### ?? ADVERTENCIA IMPORTANTE

La restauraci�n:
- **REEMPLAZA COMPLETAMENTE** todos los datos actuales
- **CIERRA** todas las conexiones activas a las bases de datos
- **NO SE PUEDE DESHACER** una vez completada
- Requiere que los archivos de backup sean v�lidos y compatibles

### C�mo Restaurar desde Backup

1. **?? Antes de comenzar**:
   - Crear un backup de la base de datos actual (por si acaso)
   - Cerrar todas las aplicaciones conectadas a las bases de datos
   - Asegurarse de tener los archivos .bak correctos

2. **Acceder al formulario**:
   - Men� ? Herramientas ? Restaurar Backup

3. **Proceso de restauraci�n**:
   - Hacer clic en "Restaurar Base de Datos"
   - Leer y aceptar las advertencias
   - Seleccionar la carpeta que contiene los archivos .bak
   - Verificar la lista de archivos detectados
   - Confirmar la operaci�n (se pedir� confirmaci�n 3 veces)
   - Esperar a que complete la restauraci�n
   - Reiniciar la aplicaci�n (recomendado)

### Archivos Requeridos

La carpeta seleccionada debe contener los archivos de backup:

```
[Carpeta de backup]/
??? DistribuidoraLosAmigos*.bak  (cualquier archivo que empiece con este nombre)
??? Login*.bak
??? Bitacora*.bak
```

El sistema busca autom�ticamente:
1. Primero: Archivo con nombre exacto (ej: `DistribuidoraLosAmigos.bak`)
2. Luego: Archivos que contengan el nombre de la base de datos (ej: `DistribuidoraLosAmigos_2024-01-15.bak`)

## ??? Arquitectura T�cnica

### Capas Implementadas

```
???????????????????????????????????????
?         UI Layer (Forms)            ?
?  ??? BackUpForm.cs                  ?
?  ??? RestoreForm.cs                 ?
???????????????????????????????????????
               ?
???????????????????????????????????????
?      Service Layer (Facade)         ?
?     BackupService.cs                ?
?  ??? ExecuteBackup()                ?
?  ??? ExecuteRestore()               ?
???????????????????????????????????????
               ?
???????????????????????????????????????
?      Business Logic Layer           ?
?      BackUpLogic.cs                 ?
?  ??? PerformBackup()                ?
?  ??? PerformRestore()               ?
?  ??? FindBackupFile()               ?
???????????????????????????????????????
               ?
???????????????????????????????????????
?      Data Access Layer              ?
?      BackupRepository.cs            ?
?  ??? BackupDatabase()               ?
?  ??? RestoreDatabase()              ?
???????????????????????????????????????
```

### Flujo de Backup

```
Usuario ? BackUpForm ? BackupService.ExecuteBackup()
                            ?
                    BackUpLogic.PerformBackup()
                            ?
                BackupRepository.BackupDatabase() (�3)
                            ?
                  SQL Server (BACKUP DATABASE)
                            ?
                Archivos .bak guardados
```

### Flujo de Restore

```
Usuario ? RestoreForm ? BackupService.ExecuteRestore()
                            ?
                    BackUpLogic.PerformRestore()
                            ?
                BackupRepository.RestoreDatabase() (�3)
                            ?
            SQL Server (SET SINGLE_USER + RESTORE + SET MULTI_USER)
                            ?
                Bases de datos restauradas
```

## ?? Consideraciones de Seguridad

### Permisos Requeridos

1. **Usuario de SQL Server**:
   - Permisos de `BACKUP DATABASE` para crear backups
   - Permisos de `RESTORE DATABASE` para restaurar
   - Permisos de `ALTER DATABASE` para cambiar modos de usuario

2. **Usuario de Windows**:
   - Lectura en la carpeta de origen (para restore)
   - Escritura en la carpeta de destino (para backup)

### Proceso de Restore Seguro

El restore implementa las siguientes medidas de seguridad:

1. **M�ltiples confirmaciones**: 3 di�logos de confirmaci�n antes de proceder
2. **Modo SINGLE_USER**: Cierra todas las conexiones antes de restaurar
3. **Rollback inmediato**: Desconexi�n forzada de usuarios activos
4. **Modo MULTI_USER**: Restaura el acceso despu�s de completar
5. **Logs detallados**: Registra todas las operaciones

## ?? Manejo de Errores

### Errores Comunes de Backup

| Error | Causa | Soluci�n |
|-------|-------|----------|
| "Permiso denegado" | Sin permisos de escritura | Seleccionar otra carpeta o ejecutar como admin |
| "OPENROWSET no habilitado" | Funci�n SQL deshabilitada | Contactar al administrador de SQL Server |
| "No se puede acceder al archivo" | Ruta inv�lida | Verificar que la ruta existe y es accesible |

### Errores Comunes de Restore

| Error | Causa | Soluci�n |
|-------|-------|----------|
| "La base de datos est� en uso" | Conexiones activas | Cerrar todas las aplicaciones conectadas |
| "Archivo de backup no encontrado" | Archivos .bak faltantes | Verificar que los 3 archivos existen |
| "No se puede abrir el archivo" | Permisos insuficientes | Verificar permisos de lectura |
| "Backup corrupto" | Archivo da�ado | Usar un backup diferente |

## ?? Troubleshooting

### Problema: "No se encontr� el archivo de backup para la base de datos X"

**Causas posibles**:
- El archivo .bak no existe en la carpeta seleccionada
- El nombre del archivo no coincide con el nombre de la base de datos

**Soluci�n**:
1. Verificar que la carpeta contenga todos los archivos necesarios
2. Asegurarse de que los nombres de los archivos contengan el nombre de la base de datos
3. Ejemplo v�lido: `DistribuidoraLosAmigos_2024-01-15.bak`

### Problema: "Error: La base de datos est� en uso"

**Causas**:
- Otras instancias de la aplicaci�n est�n conectadas
- Otro usuario est� usando la base de datos
- Conexiones hu�rfanas en SQL Server

**Soluci�n**:
1. Cerrar todas las instancias de la aplicaci�n
2. Verificar en SQL Server Management Studio:
```sql
-- Ver conexiones activas
SELECT * FROM sys.dm_exec_sessions 
WHERE database_id = DB_ID('DistribuidoraLosAmigos')

-- Forzar cierre de conexiones (usar con precauci�n)
ALTER DATABASE DistribuidoraLosAmigos SET SINGLE_USER WITH ROLLBACK IMMEDIATE
ALTER DATABASE DistribuidoraLosAmigos SET MULTI_USER
```

### Problema: "Error despu�s de la restauraci�n"

**S�ntomas**:
- La aplicaci�n no puede conectarse
- Errores de autenticaci�n
- Datos inconsistentes

**Soluci�n**:
1. **Reiniciar la aplicaci�n** (obligatorio despu�s de restore)
2. Verificar cadenas de conexi�n en `App.config`
3. Verificar que las 3 bases de datos fueron restauradas correctamente
4. Revisar logs en `C:\Logs\error.log`

## ?? Logs y Auditor�a

Todas las operaciones de backup y restore se registran en:

```
C:\Logs\
??? app.log      - Log general de la aplicaci�n
??? error.log    - Errores detallados
??? info.log     - Operaciones informativas
```

### Ejemplo de Log de Backup

```
[INFO] 2024-01-15 14:30:00 - Backup ejecutado manualmente por el usuario en: C:\Backups
[INFO] 2024-01-15 14:30:15 - Backup completado exitosamente en: C:\Backups
```

### Ejemplo de Log de Restore

```
[INFO] 2024-01-15 15:45:00 - Formulario de restauraci�n de base de datos abierto
[INFO] 2024-01-15 15:45:30 - Restauraci�n ejecutada exitosamente desde: C:\Backups
[INFO] 2024-01-15 15:45:45 - Restauraci�n completada exitosamente desde: C:\Backups
```

## ?? Mejores Pr�cticas

### Backups Regulares

1. **Frecuencia recomendada**:
   - Diario: Para sistemas en producci�n con cambios frecuentes
   - Semanal: Para sistemas con cambios moderados
   - Antes de actualizaciones: Siempre

2. **Retenci�n**:
   - Mantener al menos los �ltimos 7 backups diarios
   - Mantener un backup mensual por 1 a�o
   - Etiquetar backups importantes (ej: fin de mes, fin de a�o)

3. **Almacenamiento**:
   - Guardar backups en ubicaci�n diferente al servidor
   - Considerar almacenamiento en la nube o disco externo
   - Probar peri�dicamente que los backups se pueden restaurar

### Antes de Restaurar

? **Checklist pre-restauraci�n**:
- [ ] Crear backup de la base de datos actual
- [ ] Cerrar todas las instancias de la aplicaci�n
- [ ] Notificar a todos los usuarios
- [ ] Verificar que los archivos .bak existen y son v�lidos
- [ ] Verificar permisos de SQL Server
- [ ] Tener plan de rollback si algo falla

### Despu�s de Restaurar

? **Checklist post-restauraci�n**:
- [ ] Reiniciar la aplicaci�n
- [ ] Verificar que puede hacer login
- [ ] Verificar que los datos son correctos
- [ ] Verificar que todos los m�dulos funcionan
- [ ] Notificar a los usuarios que el sistema est� disponible
- [ ] Documentar la operaci�n realizada

## ?? Automatizaci�n (Futuro)

### Posibles Mejoras

1. **Backups Autom�ticos Programados**:
   - Programar backups diarios/semanales
   - Notificaciones por email
   - Limpieza autom�tica de backups antiguos

2. **Verificaci�n de Integridad**:
   - Validar archivos de backup despu�s de crearlos
   - Probar restauraci�n en ambiente de prueba

3. **Compresi�n Mejorada**:
   - Aplicar compresi�n adicional a los archivos .bak
   - Encriptaci�n de backups sensibles

4. **Restore Selectivo**:
   - Permitir restaurar solo una base de datos espec�fica
   - Restaurar a una base de datos con nombre diferente

---

**�ltima actualizaci�n**: 2024
**Versi�n**: 1.0
