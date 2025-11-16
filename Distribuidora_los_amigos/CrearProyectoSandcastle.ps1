# =========================================================
# Script: Crear Proyecto Sandcastle Help File Builder
# Descripción: Genera archivo .shfbproj configurado
# =========================================================

Write-Host "???  Creando proyecto Sandcastle Help File Builder..." -ForegroundColor Cyan
Write-Host ""

# Verificar que Sandcastle esté instalado
$shfbPath = "C:\Program Files (x86)\EWSoftware\Sandcastle Help File Builder\SandcastleBuilderGUI.exe"
if (-not (Test-Path $shfbPath)) {
    Write-Host "? ERROR: Sandcastle Help File Builder no está instalado" -ForegroundColor Red
    Write-Host ""
    Write-Host "Instálalo con:" -ForegroundColor Yellow
    Write-Host "   choco install sandcastle -y" -ForegroundColor Gray
    Write-Host ""
    Write-Host "O descárgalo de:" -ForegroundColor Yellow
    Write-Host "   https://github.com/EWSoftware/SHFB/releases" -ForegroundColor Gray
    exit 1
}

Write-Host "? Sandcastle encontrado en: $shfbPath" -ForegroundColor Green
Write-Host ""

# Generar GUID único para el proyecto
$projectGuid = [guid]::NewGuid().ToString().ToUpper()

# Contenido del archivo .shfbproj
$shfbContent = @"
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <!-- Configuración básica -->
    <SHFBROOT Condition=" '`$(SHFBROOT)' == '' ">`$(MSBuildProgramFiles32)\EWSoftware\Sandcastle Help File Builder\</SHFBROOT>
    <Configuration Condition=" '`$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '`$(Platform)' == '' ">AnyCPU</Platform>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>$projectGuid</ProjectGuid>
    <SHFBSchemaVersion>2017.9.26.0</SHFBSchemaVersion>
    
    <!-- Información del proyecto -->
    <HelpTitle>Distribuidora Los Amigos - Documentación de Desarrollador</HelpTitle>
    <HelpFileVersion>1.0.0.0</HelpFileVersion>
    <NamingMethod>MemberName</NamingMethod>
    <RootNamespaceContainer>True</RootNamespaceContainer>
    <RootNamespaceTitle>Distribuidora Los Amigos API Reference</RootNamespaceTitle>
    <NamespaceSummaries>
      <NamespaceSummaryItem name="BLL" isDocumented="True">Capa de Lógica de Negocio - Contiene servicios y validaciones</NamespaceSummaryItem>
      <NamespaceSummaryItem name="DAL" isDocumented="True">Capa de Acceso a Datos - Implementaciones de repositorios</NamespaceSummaryItem>
      <NamespaceSummaryItem name="DOMAIN" isDocumented="True">Entidades del Dominio - Modelos de datos</NamespaceSummaryItem>
      <NamespaceSummaryItem name="Service" isDocumented="True">Servicios Transversales - Logging, seguridad, utilidades</NamespaceSummaryItem>
      <NamespaceSummaryItem name="BLL.Commands" isDocumented="True">Patrón Command - Comandos de operaciones de negocio</NamespaceSummaryItem>
      <NamespaceSummaryItem name="BLL.Exceptions" isDocumented="True">Excepciones de Negocio - Excepciones personalizadas</NamespaceSummaryItem>
      <NamespaceSummaryItem name="DAL.Implementations.SqlServer" isDocumented="True">Implementación SQL Server - Repositorios concretos</NamespaceSummaryItem>
    </NamespaceSummaries>
    
    <!-- Formatos de salida -->
    <HelpFileFormat>HtmlHelp1, Website</HelpFileFormat>
    <HtmlHelpName>Distribuidora_los_amigos</HtmlHelpName>
    <OutputPath>.\Help\</OutputPath>
    
    <!-- Idioma -->
    <Language>es-ES</Language>
    
    <!-- Estilo de presentación -->
    <PresentationStyle>VS2013</PresentationStyle>
    <SyntaxFilters>C#</SyntaxFilters>
    
    <!-- Opciones de compilación -->
    <BuildAssemblerVerbosity>OnlyWarningsAndErrors</BuildAssemblerVerbosity>
    <IndentHtml>False</IndentHtml>
    <FrameworkVersion>.NET Framework 4.7.2</FrameworkVersion>
    <KeepLogFile>True</KeepLogFile>
    <DisableCodeBlockComponent>False</DisableCodeBlockComponent>
    <CleanIntermediates>True</CleanIntermediates>
    <SaveComponentCacheCapacity>100</SaveComponentCacheCapacity>
    
    <!-- Opciones de contenido -->
    <ContentPlacement>AboveNamespaces</ContentPlacement>
    <MissingTags>Summary, Parameter, Returns, AutoDocumentCtors, TypeParameter, AutoDocumentDispose</MissingTags>
    <VisibleItems>InheritedMembers, InheritedFrameworkMembers, Protected, ProtectedInternalAsProtected, NonBrowsable</VisibleItems>
    <ApiFilter />
    <ComponentConfigurations />
    <HelpAttributes />
    <PlugInConfigurations />
    
    <!-- Configuración adicional -->
    <CopyrightText>Copyright %28c%29 2024 Distribuidora Los Amigos. Todos los derechos reservados.</CopyrightText>
    <FeedbackEMailAddress>soporte%40distribuidoralosamigos.com</FeedbackEMailAddress>
    <FeedbackEMailLinkText>Reportar problema</FeedbackEMailLinkText>
    <HeaderText>Sistema de Gestión - Distribuidora Los Amigos</HeaderText>
    <FooterText>Generado con Sandcastle Help File Builder</FooterText>
    
    <!-- Paths adicionales -->
    <WorkingPath>.\Working\</WorkingPath>
  </PropertyGroup>
  
  <!-- Configuraciones por plataforma -->
  <PropertyGroup Condition=" '`$(Configuration)|`$(Platform)' == 'Debug|AnyCPU' ">
  </PropertyGroup>
  <PropertyGroup Condition=" '`$(Configuration)|`$(Platform)' == 'Release|AnyCPU' ">
  </PropertyGroup>
  
  <!-- Referencias a proyectos (assemblies y XML) -->
  <ItemGroup>
    <!-- BLL -->
    <DocumentationSource sourceFile="BLL\bin\`$(Configuration)\BLL.dll" />
    <DocumentationSource sourceFile="BLL\bin\`$(Configuration)\BLL.xml" />
    
    <!-- DAL -->
    <DocumentationSource sourceFile="DAL\bin\`$(Configuration)\DAL.dll" />
    <DocumentationSource sourceFile="DAL\bin\`$(Configuration)\DAL.xml" />
    
    <!-- DOMAIN -->
    <DocumentationSource sourceFile="DOMAIN\bin\`$(Configuration)\DOMAIN.dll" />
    <DocumentationSource sourceFile="DOMAIN\bin\`$(Configuration)\DOMAIN.xml" />
    
    <!-- Service -->
    <DocumentationSource sourceFile="Service\bin\`$(Configuration)\Service.dll" />
    <DocumentationSource sourceFile="Service\bin\`$(Configuration)\Service.xml" />
    
    <!-- UI -->
    <DocumentationSource sourceFile="Distribuidora_los_amigos\bin\`$(Configuration)\Distribuidora_los_amigos.exe" />
    <DocumentationSource sourceFile="Distribuidora_los_amigos\bin\`$(Configuration)\Distribuidora_los_amigos.xml" />
  </ItemGroup>
  
  <!-- Contenido conceptual (documentación adicional en Markdown) -->
  <ItemGroup>
    <None Include="Docs\MANEJO_EXCEPCIONES.md">
      <ImagePath>.\images\</ImagePath>
    </None>
    <None Include="Docs\RESUMEN_RESILIENCIA.md">
      <ImagePath>.\images\</ImagePath>
    </None>
  </ItemGroup>
  
  <!-- Importar targets de Sandcastle -->
  <Import Project="`$(SHFBROOT)\SandcastleHelpFileBuilder.targets" />
</Project>
"@

# Guardar archivo .shfbproj
$outputFile = "Distribuidora_los_amigos.shfbproj"
$shfbContent | Out-File -FilePath $outputFile -Encoding UTF8

Write-Host "? Archivo creado: $outputFile" -ForegroundColor Green
Write-Host ""
Write-Host "?? Configuración generada:" -ForegroundColor Cyan
Write-Host "   - Proyectos incluidos: BLL, DAL, DOMAIN, Service, UI" -ForegroundColor Gray
Write-Host "   - Formatos de salida: CHM y HTML" -ForegroundColor Gray
Write-Host "   - Idioma: Español (es-ES)" -ForegroundColor Gray
Write-Host "   - Framework: .NET Framework 4.7.2" -ForegroundColor Gray
Write-Host "   - Estilo: VS2013" -ForegroundColor Gray
Write-Host ""

# Crear carpeta Help si no existe
if (-not (Test-Path "Help")) {
    New-Item -ItemType Directory -Path "Help" | Out-Null
    Write-Host "?? Carpeta 'Help' creada para salida de documentación" -ForegroundColor Green
}

# Crear carpeta Working si no existe
if (-not (Test-Path "Working")) {
    New-Item -ItemType Directory -Path "Working" | Out-Null
    Write-Host "?? Carpeta 'Working' creada para archivos temporales" -ForegroundColor Green
}

Write-Host ""
Write-Host "================================" -ForegroundColor Cyan
Write-Host "? Proyecto Sandcastle creado exitosamente" -ForegroundColor Green
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? Próximos pasos:" -ForegroundColor Yellow
Write-Host "   1. Compilar la solución en Release:" -ForegroundColor White
Write-Host "      msbuild Distribuidora_los_amigos.sln /t:Rebuild /p:Configuration=Release" -ForegroundColor Gray
Write-Host ""
Write-Host "   2. Generar documentación:" -ForegroundColor White
Write-Host "      .\GenerarDocumentacionSandcastle.ps1" -ForegroundColor Gray
Write-Host ""
Write-Host "   O abrir el proyecto en Sandcastle GUI:" -ForegroundColor White
Write-Host "      & '$shfbPath' '$outputFile'" -ForegroundColor Gray
Write-Host ""
