# ?? Guía de Configuración Inicial - Distribuidora Los Amigos

## ?? Prerrequisitos

- SQL Server instalado y en ejecución
- .NET Framework 4.7.2 o superior
- Visual Studio 2019 o superior (para desarrollo)

## ?? Configuración Inicial

### 1. Configurar Conexión a Base de Datos

Editar el archivo `App.config` con tus datos de conexión:

```xml
<connectionStrings>
    <add name="MiConexion"
         connectionString="Data Source=TU_SERVIDOR,1433;Initial Catalog=DistribuidoraLosAmigos;User ID=TU_USUARIO;Password=TU_PASSWORD;"
         providerName="System.Data.SqlClient"/>
    <add name="LogDatabase"
         connectionString="Data Source=TU_SERVIDOR,1433;Initial Catalog=Bitacora;User ID=TU_USUARIO;Password=TU_PASSWORD;"
         providerName="System.Data.SqlClient"/>
    <add name="MiConexion2"
         connectionString="Data Source=TU_SERVIDOR,1433;Initial Catalog=Login;User ID=TU_USUARIO;Password=TU_PASSWORD;"
         providerName="System.Data.SqlClient"/>
</connectionStrings>
```

### 2. Crear las Bases de Datos

Ejecutar los scripts SQL en el siguiente orden:

```sql
-- 1. Crear las bases de datos
CREATE DATABASE DistribuidoraLosAmigos;
CREATE DATABASE Login;
CREATE DATABASE Bitacora;
```

Luego ejecutar los scripts de creación de tablas incluidos en el proyecto.

### 3. Primera Ejecución

Al ejecutar la aplicación por primera vez:

1. **El sistema detectará automáticamente** que no hay usuarios
2. **Mostrará un mensaje** indicando que creará un usuario administrador
3. **Creará el usuario** con las siguientes credenciales:

```
Usuario:    admin
Contraseña: Admin123!
Email:      admin@sistema.com
```

4. **¡IMPORTANTE!** Cambia esta contraseña inmediatamente después del primer inicio de sesión

## ?? Acceso Inicial

### Primer Login

1. Ejecutar la aplicación
2. Esperar el mensaje de creación del usuario admin (solo aparece la primera vez)
3. Hacer clic en "Aceptar"
4. Ingresar credenciales:
   - **Usuario:** `admin`
   - **Contraseña:** `Admin123!`
5. Hacer clic en "Iniciar Sesión"

### Después del Primer Login

1. **Cambiar contraseña del admin** (recomendado)
2. **Crear usuarios adicionales** para cada persona que usará el sistema
3. **Asignar roles apropiados** a cada usuario
4. **(Opcional) Deshabilitar** el usuario admin después de crear otros administradores

## ?? Gestión de Usuarios y Roles

### Crear Nuevos Usuarios

1. Ir a **Menú ? Usuarios ? Crear Usuario**
2. Ingresar:
   - Nombre de usuario
   - Contraseña
   - Email
3. Hacer clic en "Crear"

### Asignar Roles

1. Ir a **Menú ? Usuarios ? Asignar Rol**
2. Seleccionar el usuario
3. Seleccionar el rol (familia de permisos)
4. Hacer clic en "Asignar"

### Roles Disponibles (Ejemplo)

- **Administrador**: Acceso completo al sistema
- **Ventas**: Gestión de pedidos y clientes
- **Inventario**: Gestión de productos y stock
- **Consulta**: Solo lectura

## ?? Backup y Restore

### Crear Backup

1. Ir a **Menú ? Herramientas ? Generar Backup**
2. Seleccionar carpeta destino
3. Confirmar operación
4. Se crearán 3 archivos .bak (uno por cada base de datos)

### Restaurar desde Backup

?? **ADVERTENCIA**: La restauración reemplaza TODOS los datos actuales

1. **Preparación**:
   - Crear backup de seguridad de la BD actual
   - Cerrar todas las aplicaciones conectadas
   
2. **Proceso**:
   - Ir a **Menú ? Herramientas ? Restaurar Backup**
   - Seleccionar carpeta con archivos .bak
   - Confirmar operación (se pedirá 3 veces)
   - Esperar a que complete
   - **Reiniciar la aplicación** (obligatorio)

3. **Verificación**:
   - Probar login
   - Verificar datos
   - Confirmar funcionalidad

?? **Más información**: Ver [BACKUP_Y_RESTORE.md](BACKUP_Y_RESTORE.md)

## ??? Recuperación de Acceso

### Si olvidaste la contraseña del admin

**Opción 1: Usar recuperación de contraseña**
1. En la pantalla de login, hacer clic en "¿Olvidó su contraseña?"
2. Seguir el proceso de recuperación vía email

**Opción 2: Reset manual desde base de datos**
1. Abrir SQL Server Management Studio
2. Ejecutar el script: `Docs/SQL_Scripts/00_Crear_Usuario_Admin_Default.sql`
3. Descomentar la sección de "Resetear contraseña del admin"
4. Ejecutar

```sql
UPDATE Usuario
SET Password = 'e3afed0047b08059d0fada10f400c1e5'  -- Resetea a "Admin123!"
WHERE UserName = 'admin'
```

### Si el usuario admin fue deshabilitado

Ejecutar en SQL Server:

```sql
UPDATE Usuario
SET Estado = 1
WHERE UserName = 'admin'
```

## ?? Estructura de Directorios

El sistema requiere los siguientes directorios (se crean automáticamente):

```
C:\Logs\                          # Logs de la aplicación
C:\SQLBackupTemp\                 # Backups SQL
C:\DistribuidoraLosAmigos\Manual\ # Archivos de ayuda
```

## ?? Configuración Avanzada

### Cambiar rutas de logs

Editar en `App.config`:

```xml
<add key="LogFilePath" value="C:\TuRuta\app.log"/>
<add key="PathLogError" value="C:\TuRuta\error.log"/>
<add key="PathLogInfo" value="C:\TuRuta\info.log"/>
```

### Cambiar idioma por defecto

Editar en `App.config`:

```xml
<add key="Idioma" value="es-ES"/>  <!-- es-ES, en-US, pt-PT -->
```

### Configurar backups

Editar en `App.config`:

```xml
<add key="BackupPath1" value="D:\MisBackups\"/>
<add key="BackupPath2" value="E:\MisBackups\"/>
<add key="Database1Name" value="DistribuidoraLosAmigos"/>
<add key="Database2Name" value="Login"/>
<add key="Database3Name" value="Bitacora"/>
```

## ? Verificación de Instalación

### Checklist de verificación:

- [ ] SQL Server instalado y funcionando
- [ ] Bases de datos creadas (DistribuidoraLosAmigos, Login, Bitacora)
- [ ] Strings de conexión configuradas en App.config
- [ ] Aplicación ejecuta sin errores
- [ ] Usuario admin creado automáticamente
- [ ] Login exitoso con credenciales por defecto
- [ ] Contraseña del admin cambiada
- [ ] Usuario adicional creado y probado
- [ ] Roles asignados correctamente
- [ ] Permisos funcionando según lo esperado
- [ ] Backup creado exitosamente
- [ ] Restore probado en ambiente de prueba

## ?? Troubleshooting

### Error: "No se puede conectar a la base de datos"
- Verificar que SQL Server esté ejecutándose
- Revisar el string de conexión en App.config
- Verificar credenciales de SQL Server

### Error: "El usuario admin no puede iniciar sesión"
- Verificar que la contraseña sea exactamente: `Admin123!` (sensible a mayúsculas)
- Revisar logs en `C:\Logs\error.log`
- Intentar reset de contraseña desde SQL

### Error: "No tengo permisos para ver ciertos menús"
- Verificar que el usuario tenga un rol asignado
- Verificar que el rol tenga las patentes necesarias
- Contactar al administrador del sistema

### Error: "Fallo al crear backup"
- Verificar permisos de escritura en la carpeta destino
- Verificar que SQL Server tenga acceso a la ruta
- Revisar logs para detalles específicos

### Error: "Fallo al restaurar backup"
- Cerrar TODAS las conexiones a las bases de datos
- Verificar que los archivos .bak sean válidos
- Ejecutar como administrador si es necesario
- Revisar script manual: `SQL_Scripts/01_Restore_Manual_Emergency.sql`

## ?? Soporte

Para problemas adicionales:
1. Revisar logs en `C:\Logs\`
2. Consultar documentación en `Docs/`
3. Revisar issues en el repositorio del proyecto
4. Usar script de emergencia de restore si es necesario

## ?? Documentación Adicional

- [Usuario Administrador por Defecto](USUARIO_ADMINISTRADOR_DEFAULT.md)
- [Sistema de Backup y Restore](BACKUP_Y_RESTORE.md)
- [Scripts SQL de Emergencia](SQL_Scripts/)
- Manual de Usuario (disponible en la aplicación, tecla F1)

---

**Última actualización**: 2024
**Versión del Sistema**: 1.0
