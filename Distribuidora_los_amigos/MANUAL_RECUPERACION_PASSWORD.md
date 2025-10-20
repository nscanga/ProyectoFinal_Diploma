# ?? Sistema de Recuperación de Contraseña - Manual de Uso

## ? **Funcionalidad Implementada**

He implementado completamente el sistema de recuperación de contraseña en tu aplicación. Ahora los usuarios pueden recuperar su contraseña fácilmente desde el formulario de login.

## ?? **Cómo Funciona**

### 1. **Activar la Recuperación**
- En el formulario de login, el usuario hace clic en el enlace **"Recuperar contraseña"**
- Se abre un nuevo formulario específico para la recuperación

### 2. **Proceso de Recuperación**
1. **Ingresar Usuario**: El usuario ingresa su nombre de usuario
2. **Enviar Código**: Se genera un código de 6 dígitos y se envía por email
3. **Validar Código**: El usuario ingresa el código recibido
4. **Nueva Contraseña**: El usuario ingresa su nueva contraseña (mínimo 6 caracteres)
5. **Confirmar**: Se valida que las contraseñas coincidan
6. **Completar**: La contraseña se actualiza en la base de datos

### 3. **Seguridad Implementada**
- ? **Expiración**: Los códigos expiran en 10 minutos
- ?? **Encriptación**: Las contraseñas se guardan hasheadas con MD5
- ? **Validación**: Se verifica que el usuario exista y tenga email
- ?? **Email válido**: Se valida el formato del email antes de enviar

## ?? **Componentes Implementados**

### **1. Formulario de Recuperación**
- **Archivo**: `Distribuidora_los_amigos\Forms\RecuperarPassword\RecuperarPasswordForm.cs`
- **Diseñador**: `RecuperarPasswordForm.Designer.cs`
- **Recursos**: `RecuperarPasswordForm.resx`

### **2. Servicio de Recuperación**
- **Archivo**: `Service\Facade\RecuperoPassService.cs`
- **Funciones**:
  - `GetUsuario(string username)`: Obtiene usuario completo
  - `GenerarYEnviarMailRecuperacion(string username)`: Genera y envía token
  - `ValidateRecoveryToken(string username, string token)`: Valida token
  - `ChangePassword(...)`: Cambia la contraseña

### **3. Lógica de Recuperación**
- **Archivo**: `Service\Logic\RecuperoPassLogic.cs`
- **Funciones**:
  - `GenerateRecoveryToken()`: Genera código de 6 dígitos
  - `GenerarMail(Usuario usuario)`: Envía email con token
  - `EsValidoRecoveryToken(...)`: Valida token y expiración
  - `ChangePassword(...)`: Actualiza contraseña encriptada

### **4. Repositorio de Usuario**
- **Archivo**: `Service\DAL\Implementations\SqlServer\UsuarioRepository.cs`
- **Métodos agregados**:
  - `UpdateUsuarioToken(Usuario usuario)`: Actualiza token y expiración
  - `UpdatePassword(Usuario usuario)`: Actualiza contraseña
  - `GetUsuarioCompletos(string username)`: Obtiene usuario con tokens

### **5. Modelo de Usuario**
- **Archivo**: `Service\DOMAIN\Usuario.cs`
- **Propiedades**:
  - `string RecoveryToken`: Token de recuperación
  - `DateTime TokenExpiration`: Fecha de expiración del token

## ?? **Configuración del Email**

El sistema usa Gmail SMTP. La configuración está en `EmailService.cs`:

```csharp
private static string smtpServer = "smtp.gmail.com";
private static string smtpUser = "distribuidoralosamigos@gmail.com";
private static string smtpPass = "pwknemritdinbhhy";
private static int smtpPort = 587;
```

## ?? **Interfaz de Usuario**

### **Formulario de Login**
- El enlace "Recuperar contraseña" ahora es clickeable
- Cursor cambia a "mano" al pasar por encima
- Soporte completo para múltiples idiomas

### **Formulario de Recuperación**
- **Panel 1**: Ingreso de usuario y envío de código
- **Panel 2**: Aparece después de enviar código con:
  - Campo para código de 6 dígitos
  - Campo para nueva contraseña
  - Campo para confirmar contraseña
  - Botón para cambiar contraseña

## ?? **Soporte Multiidioma**

Todos los mensajes están preparados para traducción:
- "Por favor, ingrese su nombre de usuario."
- "Se ha enviado un código de recuperación a su correo electrónico."
- "El token no es válido o ha expirado."
- "Contraseña cambiada exitosamente."

## ? **Características Adicionales**

### **1. Validaciones**
- Usuario debe existir
- Usuario debe tener email registrado
- Código debe ser válido y no haber expirado
- Contraseñas deben coincidir
- Contraseña mínima de 6 caracteres

### **2. Logging**
- Se registran todos los eventos importantes
- Errores se loguean automáticamente
- Integración completa con el sistema de bitácora

### **3. Experiencia de Usuario**
- Mensajes claros y específicos
- Botones se deshabilitan durante procesamiento
- Retroalimentación visual inmediata
- Formulario responsive y fácil de usar

## ?? **Cómo Probar**

1. **Ejecutar la aplicación**
2. **En el Login**: Hacer clic en "Recuperar contraseña"
3. **Ingresar usuario**: Por ejemplo "admin"
4. **Hacer clic en**: "Enviar código de recuperación"
5. **Revisar email**: Buscar el código de 6 dígitos
6. **Ingresar código** y nueva contraseña
7. **Completar proceso**

## ??? **Mantenimiento**

### **Base de Datos**
Asegúrate que la tabla `Usuario` tenga estas columnas:
- `RecoveryToken` (VARCHAR)
- `TokenExpiration` (DATETIME)

### **Email**
Si cambias las credenciales de email, actualiza:
- `smtpUser`
- `smtpPass`
- Habilita "Aplicaciones menos seguras" en Gmail o usa "Contraseñas de aplicación"

---

## ? **Resultado Final**

¡Tu sistema ahora tiene una funcionalidad de recuperación de contraseña completa, segura y profesional! 

Los usuarios pueden:
- ? Recuperar contraseñas olvidadas
- ? Recibir códigos por email
- ? Cambiar contraseñas de forma segura
- ? Todo con soporte multiidioma

?? **¡La implementación está lista y funcionando!**