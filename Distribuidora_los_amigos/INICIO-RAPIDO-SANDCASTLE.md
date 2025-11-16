# ?? INICIO RÁPIDO - Sandcastle Help File Builder

## ? Instalación y Generación en 5 Minutos

### Paso 1: Instalar Sandcastle (2 minutos)

Abre **PowerShell como Administrador** y ejecuta:

```powershell
# Opción A: Con Chocolatey (recomendado)
choco install sandcastle -y

# Opción B: Descarga manual
# Ir a: https://github.com/EWSoftware/SHFB/releases
# Descargar y ejecutar el instalador
```

---

### Paso 2: Configurar Proyectos (1 minuto)

```powershell
# Habilitar XML Documentation en todos los proyectos
.\HabilitarXMLDocumentation.ps1
```

---

### Paso 3: Crear Proyecto Sandcastle (30 segundos)

```powershell
# Crear archivo .shfbproj configurado
.\CrearProyectoSandcastle.ps1
```

---

### Paso 4: Generar Documentación (2 minutos)

```powershell
# Compilar y generar documentación
.\GenerarDocumentacionSandcastle.ps1
```

---

### Paso 5: Ver Resultado

```powershell
# Abrir documentación generada
.\VerDocumentacionSandcastle.ps1
```

---

## ?? Comandos Completos en Secuencia

Copia y pega todo esto en PowerShell (ejecutar como Administrador):

```powershell
# 1. Instalar Sandcastle
choco install sandcastle -y

# 2. Cambiar a directorio del proyecto
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos"

# 3. Configurar proyectos
.\HabilitarXMLDocumentation.ps1

# 4. Crear proyecto Sandcastle
.\CrearProyectoSandcastle.ps1

# 5. Generar documentación
.\GenerarDocumentacionSandcastle.ps1

# 6. Ver resultado
.\VerDocumentacionSandcastle.ps1
```

---

## ?? Estructura de Archivos Generados

```
Distribuidora_los_amigos/
?
??? Distribuidora_los_amigos.shfbproj    ? Proyecto Sandcastle
??? Help/                                 ? Documentación generada
?   ??? Distribuidora_los_amigos.chm     ? Windows Help (CHM)
?   ??? index.html                        ? Sitio web HTML
?   ??? ...                               ? Páginas HTML individuales
?
??? Working/                              ? Archivos temporales
?   ??? *.log                             ? Logs de compilación
?
??? Scripts/
    ??? HabilitarXMLDocumentation.ps1
    ??? CrearProyectoSandcastle.ps1
    ??? GenerarDocumentacionSandcastle.ps1
    ??? VerDocumentacionSandcastle.ps1
    ??? LimpiarDocumentacionSandcastle.ps1
```

---

## ?? Qué Incluye la Documentación

? **API Reference completa:**
- Todos los namespaces (BLL, DAL, DOMAIN, Service, UI)
- Todas las clases públicas
- Todos los métodos y propiedades
- Comentarios XML extraídos

? **Formatos generados:**
- ?? **CHM** (Windows Help) - Para F1 contextual
- ?? **HTML** (Website) - Para intranet o hosting

? **Características:**
- Búsqueda de texto completo
- Navegación por namespaces
- Sintaxis de código resaltada
- Links entre tipos relacionados
- Herencia de clases visualizada

---

## ?? Comandos Útiles

### Regenerar Documentación
```powershell
.\GenerarDocumentacionSandcastle.ps1
```

### Ver CHM
```powershell
.\VerDocumentacionSandcastle.ps1 -Formato CHM
```

### Ver HTML
```powershell
.\VerDocumentacionSandcastle.ps1 -Formato HTML
```

### Limpiar Archivos Temporales
```powershell
.\LimpiarDocumentacionSandcastle.ps1
```

### Limpieza Completa (incluye XML)
```powershell
.\LimpiarDocumentacionSandcastle.ps1 -Full
```

---

## ?? Editar Configuración

Para personalizar la documentación, edita el archivo:

```
Distribuidora_los_amigos.shfbproj
```

Puedes abrirlo con:
- **Sandcastle GUI**: Interfaz gráfica completa
- **Visual Studio**: Como proyecto MSBuild
- **Editor de texto**: Es XML estándar

```powershell
# Abrir en Sandcastle GUI
& "C:\Program Files (x86)\EWSoftware\Sandcastle Help File Builder\SandcastleBuilderGUI.exe" Distribuidora_los_amigos.shfbproj
```

---

## ?? Mejorar Documentación con XML Comments

Para que la documentación sea más completa, agrega comentarios XML a tu código:

```csharp
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
/// var cliente = new Cliente { Nombre = "Juan", DNI = "12345678" };
/// var resultado = clienteService.CrearCliente(cliente);
/// </code>
/// </example>
public Cliente CrearCliente(Cliente cliente)
{
    // ...
}
```

---

## ?? Solución de Problemas

### Error: "SHFB no encontrado"
```powershell
# Verificar instalación
Test-Path "C:\Program Files (x86)\EWSoftware\Sandcastle Help File Builder\"

# Reinstalar
choco install sandcastle -y --force
```

### Error: "XML files not found"
```powershell
# Verificar que XML docs estén habilitados
.\HabilitarXMLDocumentation.ps1

# Recompilar solución
msbuild Distribuidora_los_amigos.sln /t:Rebuild /p:Configuration=Release
```

### CHM no abre (Bloqueado por Windows)
```powershell
# Desbloquear archivo
Unblock-File -Path "Help\Distribuidora_los_amigos.chm"
```

---

## ?? Recursos Adicionales

- **Documentación SHFB**: https://ewsoftware.github.io/SHFB/html/
- **XML Comments Guide**: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
- **Guía Completa**: Ver `INSTALACION-SANDCASTLE.md`

---

## ? Checklist de Verificación

- [ ] Sandcastle instalado correctamente
- [ ] XML Documentation habilitado en proyectos
- [ ] Archivo .shfbproj creado
- [ ] Solución compilada en Release
- [ ] Documentación generada sin errores
- [ ] CHM abre correctamente
- [ ] HTML muestra contenido completo

---

## ?? Próximos Pasos

1. **Agregar comentarios XML** a clases y métodos principales
2. **Personalizar** el proyecto .shfbproj (logo, footer, etc.)
3. **Agregar documentación conceptual** (guías, tutoriales)
4. **Configurar CI/CD** para generación automática
5. **Publicar HTML** en intranet o SharePoint

---

**¡Ya tienes documentación profesional de tu aplicación! ??**
