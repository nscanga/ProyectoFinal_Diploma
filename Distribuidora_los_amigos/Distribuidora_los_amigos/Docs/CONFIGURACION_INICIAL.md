# ?? Gu�a de Configuraci�n Inicial - Distribuidora Los Amigos

## ?? Prerrequisitos

- SQL Server instalado y en ejecuci�n
- .NET Framework 4.7.2 o superior
- Visual Studio 2019 o superior (para desarrollo)

## ?? Configuraci�n Inicial

### 1. Configurar Conexi�n a Base de Datos

Editar el archivo `App.config` con tus datos de conexi�n:

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

Luego ejecutar los scripts de creaci�n de tablas incluidos en el proyecto.

### 3. Primera Ejecuci�n

Al ejecutar la aplicaci�n por primera vez:

1. **El sistema detectar� autom�ticamente** que no hay usuarios
2. **Mostrar� un mensaje** indicando que crear� un usuario administrador
3. **Crear� el usuario** con las siguientes credenciales:

```
Usuario:    admin
Contrase�a: Admin123!
Email:      admin@sistema.com
```

4. **�IMPORTANTE!** Cambia esta contrase�a inmediatamente despu�s del primer inicio de sesi�n

## ?? Acceso Inicial

### Primer Login

1. Ejecutar la aplicaci�n
2. Esperar el mensaje de creaci�n del usuario admin (solo aparece la primera vez)
3. Hacer clic en "Aceptar"
4. Ingresar credenciales:
   - **Usuario:** `admin`
   - **Contrase�a:** `Admin123!`
5. Hacer clic en "Iniciar Sesi�n"

### Despu�s del Primer Login

1. **Cambiar contrase�a del admin** (recomendado)
2. **Crear usuarios adicionales** para cada persona que usar� el sistema
3. **Asignar roles apropiados** a cada usuario
4. **(Opcional) Deshabilitar** el usuario admin despu�s de crear otros administradores

## ?? Gesti�n de Usuarios y Roles

### Crear Nuevos Usuarios

1. Ir a **Men� ? Usuarios ? Crear Usuario**
2. Ingresar:
   - Nombre de usuario
   - Contrase�a
   - Email
3. Hacer clic en "Crear"

### Asignar Roles

1. Ir a **Men� ? Usuarios ? Asignar Rol**
2. Seleccionar el usuario
3. Seleccionar el rol (familia de permisos)
4. Hacer clic en "Asignar"

### Roles Disponibles (Ejemplo)

- **Administrador**: Acceso completo al sistema
- **Ventas**: Gesti�n de pedidos y clientes
- **Inventario**: Gesti�n de productos y stock
- **Consulta**: Solo lectura

## ?? Backup y Restore

### Crear Backup

1. Ir a **Men� ? Herramientas ? Generar Backup**
2. Seleccionar carpeta destino
3. Confirmar operaci�n
4. Se crear�n 3 archivos .bak (uno por cada base de datos)

### Restaurar desde Backup

?? **ADVERTENCIA**: La restauraci�n reemplaza TODOS los datos actuales

1. **Preparaci�n**:
   - Crear backup de seguridad de la BD actual
   - Cerrar todas las aplicaciones conectadas
   
2. **Proceso**:
   - Ir a **Men� ? Herramientas ? Restaurar Backup**
   - Seleccionar carpeta con archivos .bak
   - Confirmar operaci�n (se pedir� 3 veces)
   - Esperar a que complete
   - **Reiniciar la aplicaci�n** (obligatorio)

3. **Verificaci�n**:
   - Probar login
   - Verificar datos
   - Confirmar funcionalidad

?? **M�s informaci�n**: Ver [BACKUP_Y_RESTORE.md](BACKUP_Y_RESTORE.md)

## ??? Recuperaci�n de Acceso

### Si olvidaste la contrase�a del admin

**Opci�n 1: Usar recuperaci�n de contrase�a**
1. En la pantalla de login, hacer clic en "�Olvid� su contrase�a?"
2. Seguir el proceso de recuperaci�n v�a email

**Opci�n 2: Reset manual desde base de datos**
1. Abrir SQL Server Management Studio
2. Ejecutar el script: `Docs/SQL_Scripts/00_Crear_Usuario_Admin_Default.sql`
3. Descomentar la secci�n de "Resetear contrase�a del admin"
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

El sistema requiere los siguientes directorios (se crean autom�ticamente):

```
C:\Logs\                          # Logs de la aplicaci�n
C:\SQLBackupTemp\                 # Backups SQL
C:\DistribuidoraLosAmigos\Manual\ # Archivos de ayuda
```

## ?? Configuraci�n Avanzada

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

## ? Verificaci�n de Instalaci�n

### Checklist de verificaci�n:

- [ ] SQL Server instalado y funcionando
- [ ] Bases de datos creadas (DistribuidoraLosAmigos, Login, Bitacora)
- [ ] Strings de conexi�n configuradas en App.config
- [ ] Aplicaci�n ejecuta sin errores
- [ ] Usuario admin creado autom�ticamente
- [ ] Login exitoso con credenciales por defecto
- [ ] Contrase�a del admin cambiada
- [ ] Usuario adicional creado y probado
- [ ] Roles asignados correctamente
- [ ] Permisos funcionando seg�n lo esperado
- [ ] Backup creado exitosamente
- [ ] Restore probado en ambiente de prueba

## ?? Troubleshooting

### Error: "No se puede conectar a la base de datos"
- Verificar que SQL Server est� ejecut�ndose
- Revisar el string de conexi�n en App.config
- Verificar credenciales de SQL Server

### Error: "El usuario admin no puede iniciar sesi�n"
- Verificar que la contrase�a sea exactamente: `Admin123!` (sensible a may�sculas)
- Revisar logs en `C:\Logs\error.log`
- Intentar reset de contrase�a desde SQL

### Error: "No tengo permisos para ver ciertos men�s"
- Verificar que el usuario tenga un rol asignado
- Verificar que el rol tenga las patentes necesarias
- Contactar al administrador del sistema

### Error: "Fallo al crear backup"
- Verificar permisos de escritura en la carpeta destino
- Verificar que SQL Server tenga acceso a la ruta
- Revisar logs para detalles espec�ficos

### Error: "Fallo al restaurar backup"
- Cerrar TODAS las conexiones a las bases de datos
- Verificar que los archivos .bak sean v�lidos
- Ejecutar como administrador si es necesario
- Revisar script manual: `SQL_Scripts/01_Restore_Manual_Emergency.sql`

## ?? Soporte

Para problemas adicionales:
1. Revisar logs en `C:\Logs\`
2. Consultar documentaci�n en `Docs/`
3. Revisar issues en el repositorio del proyecto
4. Usar script de emergencia de restore si es necesario

## ?? Documentaci�n Adicional

- [Usuario Administrador por Defecto](USUARIO_ADMINISTRADOR_DEFAULT.md)
- [Sistema de Backup y Restore](BACKUP_Y_RESTORE.md)
- [Scripts SQL de Emergencia](SQL_Scripts/)
- Manual de Usuario (disponible en la aplicaci�n, tecla F1)

---

**�ltima actualizaci�n**: 2024
**Versi�n del Sistema**: 1.0
