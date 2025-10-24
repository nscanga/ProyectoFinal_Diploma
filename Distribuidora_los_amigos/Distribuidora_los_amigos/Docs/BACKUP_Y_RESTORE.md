# ?? Sistema de Backup y Restore - Distribuidora Los Amigos

## ?? Resumen

El sistema incluye funcionalidad completa de **Backup (Respaldo)** y **Restore (Restauración)** de todas las bases de datos del sistema.

## ?? Bases de Datos Incluidas

El sistema realiza operaciones sobre las siguientes bases de datos:

1. **DistribuidoraLosAmigos** - Base de datos principal del sistema
2. **Login** - Base de datos de usuarios y autenticación
3. **Bitacora** - Base de datos de logs y auditoría

## ?? Funcionalidad de Backup

### Cómo Crear un Backup

1. **Acceder al formulario**:
   - Menú ? Herramientas ? Generar Backup

2. **Seleccionar ubicación**:
   - Hacer clic en "Crear BackUp"
   - Seleccionar la carpeta destino
   - Confirmar la operación

3. **Resultado**:
   - Se crearán 3 archivos .bak (uno por cada base de datos)
   - Los archivos incluirán fecha y hora en el nombre
   - Se registrará la operación en los logs

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

La restauración:
- **REEMPLAZA COMPLETAMENTE** todos los datos actuales
- **CIERRA** todas las conexiones activas a las bases de datos
- **NO SE PUEDE DESHACER** una vez completada
- Requiere que los archivos de backup sean válidos y compatibles

### Cómo Restaurar desde Backup

1. **?? Antes de comenzar**:
   - Crear un backup de la base de datos actual (por si acaso)
   - Cerrar todas las aplicaciones conectadas a las bases de datos
   - Asegurarse de tener los archivos .bak correctos

2. **Acceder al formulario**:
   - Menú ? Herramientas ? Restaurar Backup

3. **Proceso de restauración**:
   - Hacer clic en "Restaurar Base de Datos"
   - Leer y aceptar las advertencias
   - Seleccionar la carpeta que contiene los archivos .bak
   - Verificar la lista de archivos detectados
   - Confirmar la operación (se pedirá confirmación 3 veces)
   - Esperar a que complete la restauración
   - Reiniciar la aplicación (recomendado)

### Archivos Requeridos

La carpeta seleccionada debe contener los archivos de backup:

```
[Carpeta de backup]/
??? DistribuidoraLosAmigos*.bak  (cualquier archivo que empiece con este nombre)
??? Login*.bak
??? Bitacora*.bak
```

El sistema busca automáticamente:
1. Primero: Archivo con nombre exacto (ej: `DistribuidoraLosAmigos.bak`)
2. Luego: Archivos que contengan el nombre de la base de datos (ej: `DistribuidoraLosAmigos_2024-01-15.bak`)

## ??? Arquitectura Técnica

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
                BackupRepository.BackupDatabase() (×3)
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
                BackupRepository.RestoreDatabase() (×3)
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

1. **Múltiples confirmaciones**: 3 diálogos de confirmación antes de proceder
2. **Modo SINGLE_USER**: Cierra todas las conexiones antes de restaurar
3. **Rollback inmediato**: Desconexión forzada de usuarios activos
4. **Modo MULTI_USER**: Restaura el acceso después de completar
5. **Logs detallados**: Registra todas las operaciones

## ?? Manejo de Errores

### Errores Comunes de Backup

| Error | Causa | Solución |
|-------|-------|----------|
| "Permiso denegado" | Sin permisos de escritura | Seleccionar otra carpeta o ejecutar como admin |
| "OPENROWSET no habilitado" | Función SQL deshabilitada | Contactar al administrador de SQL Server |
| "No se puede acceder al archivo" | Ruta inválida | Verificar que la ruta existe y es accesible |

### Errores Comunes de Restore

| Error | Causa | Solución |
|-------|-------|----------|
| "La base de datos está en uso" | Conexiones activas | Cerrar todas las aplicaciones conectadas |
| "Archivo de backup no encontrado" | Archivos .bak faltantes | Verificar que los 3 archivos existen |
| "No se puede abrir el archivo" | Permisos insuficientes | Verificar permisos de lectura |
| "Backup corrupto" | Archivo dañado | Usar un backup diferente |

## ?? Troubleshooting

### Problema: "No se encontró el archivo de backup para la base de datos X"

**Causas posibles**:
- El archivo .bak no existe en la carpeta seleccionada
- El nombre del archivo no coincide con el nombre de la base de datos

**Solución**:
1. Verificar que la carpeta contenga todos los archivos necesarios
2. Asegurarse de que los nombres de los archivos contengan el nombre de la base de datos
3. Ejemplo válido: `DistribuidoraLosAmigos_2024-01-15.bak`

### Problema: "Error: La base de datos está en uso"

**Causas**:
- Otras instancias de la aplicación están conectadas
- Otro usuario está usando la base de datos
- Conexiones huérfanas en SQL Server

**Solución**:
1. Cerrar todas las instancias de la aplicación
2. Verificar en SQL Server Management Studio:
```sql
-- Ver conexiones activas
SELECT * FROM sys.dm_exec_sessions 
WHERE database_id = DB_ID('DistribuidoraLosAmigos')

-- Forzar cierre de conexiones (usar con precaución)
ALTER DATABASE DistribuidoraLosAmigos SET SINGLE_USER WITH ROLLBACK IMMEDIATE
ALTER DATABASE DistribuidoraLosAmigos SET MULTI_USER
```

### Problema: "Error después de la restauración"

**Síntomas**:
- La aplicación no puede conectarse
- Errores de autenticación
- Datos inconsistentes

**Solución**:
1. **Reiniciar la aplicación** (obligatorio después de restore)
2. Verificar cadenas de conexión en `App.config`
3. Verificar que las 3 bases de datos fueron restauradas correctamente
4. Revisar logs en `C:\Logs\error.log`

## ?? Logs y Auditoría

Todas las operaciones de backup y restore se registran en:

```
C:\Logs\
??? app.log      - Log general de la aplicación
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
[INFO] 2024-01-15 15:45:00 - Formulario de restauración de base de datos abierto
[INFO] 2024-01-15 15:45:30 - Restauración ejecutada exitosamente desde: C:\Backups
[INFO] 2024-01-15 15:45:45 - Restauración completada exitosamente desde: C:\Backups
```

## ?? Mejores Prácticas

### Backups Regulares

1. **Frecuencia recomendada**:
   - Diario: Para sistemas en producción con cambios frecuentes
   - Semanal: Para sistemas con cambios moderados
   - Antes de actualizaciones: Siempre

2. **Retención**:
   - Mantener al menos los últimos 7 backups diarios
   - Mantener un backup mensual por 1 año
   - Etiquetar backups importantes (ej: fin de mes, fin de año)

3. **Almacenamiento**:
   - Guardar backups en ubicación diferente al servidor
   - Considerar almacenamiento en la nube o disco externo
   - Probar periódicamente que los backups se pueden restaurar

### Antes de Restaurar

? **Checklist pre-restauración**:
- [ ] Crear backup de la base de datos actual
- [ ] Cerrar todas las instancias de la aplicación
- [ ] Notificar a todos los usuarios
- [ ] Verificar que los archivos .bak existen y son válidos
- [ ] Verificar permisos de SQL Server
- [ ] Tener plan de rollback si algo falla

### Después de Restaurar

? **Checklist post-restauración**:
- [ ] Reiniciar la aplicación
- [ ] Verificar que puede hacer login
- [ ] Verificar que los datos son correctos
- [ ] Verificar que todos los módulos funcionan
- [ ] Notificar a los usuarios que el sistema está disponible
- [ ] Documentar la operación realizada

## ?? Automatización (Futuro)

### Posibles Mejoras

1. **Backups Automáticos Programados**:
   - Programar backups diarios/semanales
   - Notificaciones por email
   - Limpieza automática de backups antiguos

2. **Verificación de Integridad**:
   - Validar archivos de backup después de crearlos
   - Probar restauración en ambiente de prueba

3. **Compresión Mejorada**:
   - Aplicar compresión adicional a los archivos .bak
   - Encriptación de backups sensibles

4. **Restore Selectivo**:
   - Permitir restaurar solo una base de datos específica
   - Restaurar a una base de datos con nombre diferente

---

**Última actualización**: 2024
**Versión**: 1.0
