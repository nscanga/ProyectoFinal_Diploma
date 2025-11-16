# ?? Documentación con Sandcastle - Distribuidora Los Amigos

## ?? Resumen

Este proyecto incluye una configuración completa de **Sandcastle Help File Builder** para generar documentación profesional de toda la aplicación.

## ? ¿Por qué Sandcastle y no DocFX?

- ?? **Sandcastle está optimizado para .NET Framework 4.7.2** (tu versión)
- ?? DocFX tuvo problemas extrayendo metadata de .NET Framework
- ?? Sandcastle es la herramienta oficial de Microsoft para .NET Framework
- ?? Genera formatos CHM (Windows Help) perfectos para aplicaciones de escritorio

---

## ?? Inicio Rápido (5 minutos)

### Opción A: Instalación Automática (Recomendada)

Abre **PowerShell como Administrador** y ejecuta:

```powershell
# Un solo comando hace todo:
.\InstalarSandcastle.ps1
```

Este script:
1. ? Instala Sandcastle (si no está instalado)
2. ? Habilita XML Documentation en proyectos
3. ? Compila la solución
4. ? Crea el proyecto .shfbproj
5. ? (Opcional) Genera la documentación

---

### Opción B: Instalación Manual Paso a Paso

```powershell
# 1. Instalar Sandcastle
choco install sandcastle -y

# 2. Habilitar XML Documentation
.\HabilitarXMLDocumentation.ps1

# 3. Crear proyecto Sandcastle
.\CrearProyectoSandcastle.ps1

# 4. Generar documentación
.\GenerarDocumentacionSandcastle.ps1

# 5. Ver resultado
.\VerDocumentacionSandcastle.ps1
```

---

## ?? Estructura de Archivos

```
Distribuidora_los_amigos/
?
??? ?? DOCUMENTACIÓN
?   ??? README-SANDCASTLE.md                    ? Este archivo
?   ??? INICIO-RAPIDO-SANDCASTLE.md            ? Guía rápida
?   ??? INSTALACION-SANDCASTLE.md              ? Guía completa detallada
?
??? ?? SCRIPTS DE AUTOMATIZACIÓN
?   ??? InstalarSandcastle.ps1                  ? Instalación todo-en-uno
?   ??? HabilitarXMLDocumentation.ps1           ? Habilitar XML docs
?   ??? CrearProyectoSandcastle.ps1             ? Crear .shfbproj
?   ??? GenerarDocumentacionSandcastle.ps1      ? Generar docs
?   ??? VerDocumentacionSandcastle.ps1          ? Ver resultado
?   ??? LimpiarDocumentacionSandcastle.ps1      ? Limpiar temporales
?
??? ?? PROYECTO SANDCASTLE
?   ??? Distribuidora_los_amigos.shfbproj      ? Configuración
?
??? ?? SALIDA (generada)
?   ??? Help/
?       ??? Distribuidora_los_amigos.chm       ? Windows Help
?       ??? index.html                          ? Sitio web
?       ??? ...                                 ? Páginas HTML
?
??? ??? TEMPORAL (generado)
    ??? Working/
        ??? *.log                               ? Logs de compilación
```

---

## ?? Documentación Generada

La documentación incluye:

### ?? API Reference Completa
- ? Todos los namespaces (BLL, DAL, DOMAIN, Service, UI)
- ? Todas las clases, interfaces, enums
- ? Métodos, propiedades, eventos
- ? Comentarios XML extraídos
- ? Parámetros y valores de retorno
- ? Excepciones documentadas
- ? Ejemplos de código
- ? Herencia de clases
- ? Implementaciones de interfaces

### ?? Formatos de Salida
- **CHM (Compiled HTML Help)**: Archivo único `.chm` para Windows
  - Búsqueda de texto completo
  - Índice alfabético
  - Tabla de contenidos
  - Perfecto para F1 contextual

- **HTML (Website)**: Sitio web estático
  - Responsive design
  - Navegación por árbol
  - Sintaxis resaltada
  - Ideal para intranet o hosting

---

## ??? Comandos Principales

### Generar Documentación
```powershell
.\GenerarDocumentacionSandcastle.ps1
```

### Ver Documentación
```powershell
# Abrir CHM
.\VerDocumentacionSandcastle.ps1 -Formato CHM

# Abrir HTML
.\VerDocumentacionSandcastle.ps1 -Formato HTML

# Auto (detecta formato disponible)
.\VerDocumentacionSandcastle.ps1
```

### Limpiar Temporales
```powershell
# Limpiar Help/ y Working/
.\LimpiarDocumentacionSandcastle.ps1

# Limpiar todo (incluye XML)
.\LimpiarDocumentacionSandcastle.ps1 -Full
```

### Editar Configuración
```powershell
# Abrir con Sandcastle GUI
& "C:\Program Files (x86)\EWSoftware\Sandcastle Help File Builder\SandcastleBuilderGUI.exe" Distribuidora_los_amigos.shfbproj

# O editar manualmente el .shfbproj (es XML)
```

---

## ?? Mejorar la Documentación

### Agregar Comentarios XML

Para que Sandcastle genere mejor documentación, agrega comentarios XML a tu código:

```csharp
/// <summary>
/// Servicio para gestionar operaciones de clientes.
/// </summary>
/// <remarks>
/// Implementa patrón Repository y Command con validaciones completas.
/// </remarks>
public class ClienteService
{
    /// <summary>
    /// Crea un nuevo cliente en el sistema.
    /// </summary>
    /// <param name="cliente">Datos del cliente a crear.</param>
    /// <returns>Cliente creado con ID asignado.</returns>
    /// <exception cref="ClienteException">
    /// Se lanza cuando el DNI ya existe o los datos son inválidos.
    /// </exception>
    /// <example>
    /// <code>
    /// var cliente = new Cliente 
    /// { 
    ///     Nombre = "Juan Pérez",
    ///     DNI = "12345678" 
    /// };
    /// var resultado = clienteService.CrearCliente(cliente);
    /// </code>
    /// </example>
    public Cliente CrearCliente(Cliente cliente)
    {
        // Implementación...
    }
}
```

### Tags XML Soportados

| Tag | Descripción |
|-----|-------------|
| `<summary>` | Descripción breve |
| `<remarks>` | Comentarios detallados |
| `<param name="">` | Descripción de parámetros |
| `<returns>` | Descripción del retorno |
| `<exception cref="">` | Excepciones que lanza |
| `<example>` | Ejemplo de uso |
| `<code>` | Bloque de código |
| `<see cref="">` | Referencia a otro tipo |
| `<seealso cref="">` | Ver también |

---

## ?? Personalizar la Documentación

### Editar .shfbproj

Puedes personalizar:
- Título y descripción
- Logo y footer
- Colores y estilos
- Contenido a incluir/excluir
- Idioma
- Formato de salida

```xml
<PropertyGroup>
  <HelpTitle>Mi Título Personalizado</HelpTitle>
  <CopyrightText>© 2024 Mi Empresa</CopyrightText>
  <HeaderText>Mi Encabezado</HeaderText>
  <FooterText>Mi Pie de Página</FooterText>
  <Language>es-ES</Language>
</PropertyGroup>
```

---

## ?? Proyectos Incluidos

El archivo `.shfbproj` está configurado para documentar:

| Proyecto | Tipo | Descripción |
|----------|------|-------------|
| **BLL** | DLL | Lógica de negocio y servicios |
| **DAL** | DLL | Acceso a datos y repositorios |
| **DOMAIN** | DLL | Entidades del dominio |
| **Service** | DLL | Servicios transversales |
| **UI** | EXE | Interfaz de usuario (Forms) |

---

## ?? Solución de Problemas

### Sandcastle no está instalado
```powershell
# Verificar
Test-Path "C:\Program Files (x86)\EWSoftware\Sandcastle Help File Builder\"

# Reinstalar
choco install sandcastle -y --force
```

### Archivos XML no generados
```powershell
# Habilitar en proyectos
.\HabilitarXMLDocumentation.ps1

# Recompilar
msbuild *.sln /t:Rebuild /p:Configuration=Release
```

### Error al compilar documentación
```powershell
# Ver log detallado
Get-Content Working\LastBuild.log

# Limpiar y regenerar
.\LimpiarDocumentacionSandcastle.ps1 -Full
.\GenerarDocumentacionSandcastle.ps1
```

### CHM bloqueado por Windows
```powershell
# Desbloquear
Unblock-File -Path "Help\Distribuidora_los_amigos.chm"
```

---

## ?? Integración con CI/CD

Para generar documentación automáticamente en builds:

```yaml
# Azure DevOps / GitHub Actions
- task: PowerShell@2
  inputs:
    targetType: 'filePath'
    filePath: 'GenerarDocumentacionSandcastle.ps1'
  displayName: 'Generar Documentación'

- task: PublishBuildArtifacts@1
  inputs:
    pathToPublish: 'Help'
    artifactName: 'Documentacion'
```

---

## ?? Recursos Adicionales

- **Documentación oficial SHFB**: https://ewsoftware.github.io/SHFB/html/
- **XML Comments Guide**: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
- **Sandcastle Releases**: https://github.com/EWSoftware/SHFB/releases

---

## ? Checklist

- [ ] Sandcastle instalado
- [ ] XML Documentation habilitado en proyectos
- [ ] Proyecto .shfbproj creado
- [ ] Solución compilada
- [ ] Documentación generada
- [ ] CHM funciona correctamente
- [ ] HTML muestra contenido
- [ ] Comentarios XML agregados (opcional pero recomendado)

---

## ?? Próximos Pasos Recomendados

1. ?? **Agregar comentarios XML** a clases principales
2. ?? **Personalizar** el .shfbproj (logo, colores)
3. ?? **Documentar** métodos públicos importantes
4. ?? **Publicar HTML** en intranet o servidor
5. ?? **Automatizar** generación en CI/CD

---

## ?? Consejos

- Genera la documentación en **configuración Release** para mejor rendimiento
- Usa **comentarios XML** con ejemplos de código para mayor claridad
- El formato **CHM es ideal para aplicaciones de escritorio**
- El formato **HTML es ideal para compartir en web/intranet**
- Revisa los **logs en Working/** si hay problemas
- Ejecuta **limpiezas periódicas** para ahorrar espacio

---

## ?? Soporte

Si tienes problemas:
1. Lee `INSTALACION-SANDCASTLE.md` (guía completa)
2. Revisa `INICIO-RAPIDO-SANDCASTLE.md` (pasos rápidos)
3. Consulta logs en `Working/LastBuild.log`
4. Verifica que todos los DLLs estén en `bin/Release`

---

**¡Documentación profesional lista en minutos! ??**

*Generado automáticamente para Distribuidora Los Amigos*
