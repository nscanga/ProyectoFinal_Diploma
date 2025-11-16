# ?? Guía de Instalación: Sandcastle Help File Builder (SHFB)

## ?? ¿Qué es Sandcastle?

**Sandcastle Help File Builder (SHFB)** es la herramienta profesional de Microsoft para generar documentación de API en proyectos .NET Framework. Genera archivos CHM, HTML, y más formatos.

---

## ? Ventajas de Sandcastle para tu Proyecto

- ?? **Perfecto para .NET Framework 4.7.2** (tu versión)
- ?? **Extrae XML comments** automáticamente
- ?? **Genera múltiples formatos**: CHM, HTML, PDF, Markdown
- ?? **Integrado con Visual Studio**
- ?? **Profesional y robusto**

---

## ?? Paso 1: Instalación de Sandcastle

### Opción A: Instalación con Chocolatey (Recomendada - Rápida)

```powershell
# 1. Abrir PowerShell como Administrador

# 2. Instalar Chocolatey (si no lo tienes)
Set-ExecutionPolicy Bypass -Scope Process -Force
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072
iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

# 3. Instalar Sandcastle Help File Builder
choco install sandcastle -y
```

### Opción B: Instalación Manual (Tradicional)

1. **Descargar Sandcastle Help File Builder:**
   - Ir a: https://github.com/EWSoftware/SHFB/releases
   - Descargar la última versión: `SHFBInstaller_vX.X.X.X.zip`

2. **Instalar:**
   - Ejecutar `SHFBInstaller.exe`
   - Seguir el asistente de instalación
   - Instalar en la ruta por defecto: `C:\Program Files (x86)\EWSoftware\Sandcastle Help File Builder\`

3. **Verificar instalación:**
   ```powershell
   Test-Path "C:\Program Files (x86)\EWSoftware\Sandcastle Help File Builder\SandcastleBuilderGUI.exe"
   ```

### Opción C: Instalación vía NuGet (Para integración CI/CD)

```powershell
# Instalar Sandcastle como herramienta global
dotnet tool install -g docfx
# Nota: Para Sandcastle usaremos MSBuild, no necesita tool global
```

---

## ?? Paso 2: Configurar XML Documentation en tus Proyectos

Sandcastle necesita que tus proyectos generen archivos XML con los comentarios de documentación.

### Script Automático para Habilitar XML Docs

Ejecuta este script que creé para ti:

```powershell
# Ver archivo: HabilitarXMLDocumentation.ps1
.\HabilitarXMLDocumentation.ps1
```

### Configuración Manual (Si prefieres)

Para cada proyecto (BLL, DAL, DOMAIN, Service, UI):

1. **Clic derecho en el proyecto ? Propiedades**
2. **Ir a Build ? XML documentation file**
3. **Marcar la casilla** y establecer ruta:
   ```
   bin\Debug\BLL.xml
   bin\Release\BLL.xml
   ```

O editar directamente cada `.csproj`:

```xml
<PropertyGroup>
  <DocumentationFile>bin\$(Configuration)\$(AssemblyName).xml</DocumentationFile>
</PropertyGroup>
```

---

## ??? Paso 3: Crear Proyecto Sandcastle

### Script Automático (Recomendado)

```powershell
# Ejecutar el script que crearé para ti
.\CrearProyectoSandcastle.ps1
```

Este script creará:
- `Distribuidora_los_amigos.shfbproj` - Proyecto principal
- Configuración para todos tus proyectos (BLL, DAL, DOMAIN, Service, UI)
- Referencias a assemblies y XML docs
- Configuración de salida HTML y CHM

---

## ?? Paso 4: Personalizar la Documentación

### Estructura que se generará:

```
Distribuidora_los_amigos/
??? Distribuidora_los_amigos.shfbproj    ? Proyecto Sandcastle
??? Documentation/                        ? Documentación adicional
?   ??? Conceptual/
?   ?   ??? Arquitectura.aml
?   ?   ??? ManejoExcepciones.aml
?   ?   ??? Validaciones.aml
?   ??? Images/
?       ??? (capturas de pantalla)
??? Help/                                 ? Salida generada
    ??? index.html                        ? Inicio web
    ??? Distribuidora_los_amigos.chm     ? Archivo CHM
    ??? (otros archivos HTML)
```

---

## ?? Paso 5: Configurar el Proyecto SHFB

### Configuración Básica del .shfbproj

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <!-- Configuración básica -->
    <SHFBROOT Condition=" '$(SHFBROOT)' == '' ">$(MSBuildProgramFiles32)\EWSoftware\Sandcastle Help File Builder\</SHFBROOT>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>{TU-GUID-AQUI}</ProjectGuid>
    
    <!-- Información del proyecto -->
    <HelpTitle>Distribuidora Los Amigos - Documentación de Desarrollador</HelpTitle>
    <HelpFileVersion>1.0.0.0</HelpFileVersion>
    <RootNamespaceContainer>True</RootNamespaceContainer>
    <RootNamespaceTitle>Distribuidora Los Amigos API</RootNamespaceTitle>
    
    <!-- Formatos de salida -->
    <HelpFileFormat>HtmlHelp1, Website</HelpFileFormat>
    <OutputPath>.\Help\</OutputPath>
    
    <!-- Idioma -->
    <Language>es-ES</Language>
    
    <!-- Estilo -->
    <PresentationStyle>VS2013</PresentationStyle>
    <SyntaxFilters>C#</SyntaxFilters>
    
    <!-- Configuración de compilación -->
    <BuildAssemblerVerbosity>OnlyWarningsAndErrors</BuildAssemblerVerbosity>
    <HelpFileVersion>1.0.0.0</HelpFileVersion>
    <IndentHtml>False</IndentHtml>
    <FrameworkVersion>.NET Framework 4.7.2</FrameworkVersion>
    <KeepLogFile>True</KeepLogFile>
    <DisableCodeBlockComponent>False</DisableCodeBlockComponent>
    <CleanIntermediates>True</CleanIntermediates>
  </PropertyGroup>
  
  <!-- Referencias a proyectos -->
  <ItemGroup>
    <DocumentationSource sourceFile="BLL\bin\Debug\BLL.dll" />
    <DocumentationSource sourceFile="BLL\bin\Debug\BLL.xml" />
    <DocumentationSource sourceFile="DAL\bin\Debug\DAL.dll" />
    <DocumentationSource sourceFile="DAL\bin\Debug\DAL.xml" />
    <DocumentationSource sourceFile="DOMAIN\bin\Debug\DOMAIN.dll" />
    <DocumentationSource sourceFile="DOMAIN\bin\Debug\DOMAIN.xml" />
    <DocumentationSource sourceFile="Service\bin\Debug\Service.dll" />
    <DocumentationSource sourceFile="Service\bin\Debug\Service.xml" />
    <DocumentationSource sourceFile="Distribuidora_los_amigos\bin\Debug\Distribuidora_los_amigos.exe" />
    <DocumentationSource sourceFile="Distribuidora_los_amigos\bin\Debug\Distribuidora_los_amigos.xml" />
  </ItemGroup>
  
  <!-- Importar targets de Sandcastle -->
  <Import Project="$(SHFBROOT)\SandcastleHelpFileBuilder.targets" />
</Project>
```

---

## ?? Paso 6: Generar la Documentación

### Opción A: Desde Visual Studio

1. Abrir `Distribuidora_los_amigos.shfbproj` en Visual Studio
2. Presionar **F6** o **Build ? Build Solution**
3. La documentación se generará en `Help/`

### Opción B: Desde Línea de Comandos

```powershell
# Generar documentación
.\GenerarDocumentacionSandcastle.ps1
```

### Opción C: Con MSBuild Directo

```powershell
# Compilar primero tu solución
msbuild Distribuidora_los_amigos.sln /p:Configuration=Release

# Generar documentación
msbuild Distribuidora_los_amigos.shfbproj /p:Configuration=Release
```

---

## ?? Paso 7: Ver la Documentación Generada

### Ver CHM (Windows Help)

```powershell
# Abrir archivo CHM
Start-Process ".\Help\Distribuidora_los_amigos.chm"
```

### Ver HTML (Website)

```powershell
# Abrir en navegador
Start-Process ".\Help\index.html"
```

---

## ?? Paso 8: Agregar Comentarios XML a tu Código

Para que Sandcastle genere buena documentación, agrega comentarios XML:

### Ejemplo: Clase

```csharp
/// <summary>
/// Servicio para gestionar operaciones de clientes.
/// Proporciona métodos CRUD completos con validaciones de negocio.
/// </summary>
/// <remarks>
/// Este servicio implementa el patrón Repository y Command.
/// Todas las operaciones están transaccionadas y validadas.
/// </remarks>
public class ClienteService
{
    /// <summary>
    /// Crea un nuevo cliente en el sistema.
    /// </summary>
    /// <param name="cliente">Entidad cliente con los datos a registrar.</param>
    /// <returns>El cliente creado con su ID asignado.</returns>
    /// <exception cref="ClienteException">
    /// Se lanza cuando:
    /// - El DNI ya existe en el sistema
    /// - Los datos son inválidos
    /// - El nombre está vacío
    /// </exception>
    /// <example>
    /// <code>
    /// var cliente = new Cliente 
    /// { 
    ///     Nombre = "Juan Pérez",
    ///     DNI = "12345678",
    ///     Direccion = "Calle Falsa 123"
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

### Tags XML Importantes

| Tag | Uso |
|-----|-----|
| `<summary>` | Descripción breve de clase/método |
| `<remarks>` | Comentarios adicionales detallados |
| `<param name="x">` | Descripción de parámetros |
| `<returns>` | Descripción del valor retornado |
| `<exception cref="T">` | Excepciones que puede lanzar |
| `<example>` | Ejemplos de uso |
| `<code>` | Bloques de código |
| `<see cref="T">` | Referencias a otros tipos |
| `<seealso cref="T">` | Ver también |

---

## ?? Paso 9: Contenido Conceptual (Opcional pero Recomendado)

Puedes agregar páginas adicionales de documentación conceptual:

### Crear archivo MAML (Microsoft Assistance Markup Language)

```xml
<!-- Documentation/Conceptual/Arquitectura.aml -->
<?xml version="1.0" encoding="utf-8"?>
<topic id="arquitectura-guid" revisionNumber="1">
  <developerConceptualDocument xmlns="http://ddue.schemas.microsoft.com/authoring/2003/5">
    <introduction>
      <para>Arquitectura del Sistema Distribuidora Los Amigos</para>
    </introduction>
    
    <section>
      <title>Capas de la Aplicación</title>
      <content>
        <para>El sistema está organizado en las siguientes capas:</para>
        <list class="bullet">
          <listItem><para><legacyBold>UI:</legacyBold> Capa de presentación (Windows Forms)</para></listItem>
          <listItem><para><legacyBold>BLL:</legacyBold> Lógica de negocio</para></listItem>
          <listItem><para><legacyBold>DAL:</legacyBold> Acceso a datos</para></listItem>
          <listItem><para><legacyBold>DOMAIN:</legacyBold> Entidades del dominio</para></listItem>
          <listItem><para><legacyBold>Service:</legacyBold> Servicios transversales</para></listItem>
        </list>
      </content>
    </section>
  </developerConceptualDocument>
</topic>
```

---

## ?? Paso 10: Scripts de Automatización

Voy a crear estos scripts para ti:

1. **HabilitarXMLDocumentation.ps1** - Habilita XML docs en todos los proyectos
2. **CrearProyectoSandcastle.ps1** - Crea el .shfbproj configurado
3. **GenerarDocumentacionSandcastle.ps1** - Genera la documentación
4. **VerDocumentacionSandcastle.ps1** - Abre la documentación generada
5. **LimpiarDocumentacionSandcastle.ps1** - Limpia archivos temporales

---

## ?? Checklist de Instalación

- [ ] Sandcastle Help File Builder instalado
- [ ] XML Documentation habilitada en todos los proyectos
- [ ] Proyecto .shfbproj creado
- [ ] Solución compilada en Release
- [ ] Documentación generada exitosamente
- [ ] CHM generado y abre correctamente
- [ ] HTML generado y muestra contenido
- [ ] Comentarios XML agregados a clases principales
- [ ] Documentación conceptual agregada (opcional)

---

## ?? Solución de Problemas

### Error: "SHFB no encontrado"
```powershell
# Verificar instalación
Test-Path "C:\Program Files (x86)\EWSoftware\Sandcastle Help File Builder\"
# Reinstalar si es necesario
choco install sandcastle -y --force
```

### Error: "XML documentation file not found"
```powershell
# Rebuild solución
msbuild Distribuidora_los_amigos.sln /t:Rebuild /p:Configuration=Release
# Verificar archivos XML generados
Get-ChildItem -Recurse -Filter "*.xml" | Where-Object { $_.Directory.Name -eq "Debug" -or $_.Directory.Name -eq "Release" }
```

### Error: "Assembly could not be loaded"
```powershell
# Copiar dependencias a carpeta de output
# Verificar que todos los DLLs estén en bin\Debug o bin\Release
```

### CHM no abre (Error: "Cannot display page")
```powershell
# Desbloquear archivo CHM
Unblock-File -Path ".\Help\Distribuidora_los_amigos.chm"
```

---

## ?? Recursos Adicionales

- **Documentación oficial SHFB:** https://ewsoftware.github.io/SHFB/html/
- **XML Comments Guide:** https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
- **MAML Guide:** https://ewsoftware.github.io/MAMLGuide/html/

---

## ?? Próximos Pasos

Una vez instalado Sandcastle, ejecuta:

```powershell
# 1. Habilitar XML Documentation
.\HabilitarXMLDocumentation.ps1

# 2. Crear proyecto Sandcastle
.\CrearProyectoSandcastle.ps1

# 3. Generar documentación
.\GenerarDocumentacionSandcastle.ps1

# 4. Ver resultado
.\VerDocumentacionSandcastle.ps1
```

---

**¡Listo! Tendrás documentación profesional de toda tu aplicación.**
