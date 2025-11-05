# ?? Guía de Implementación de Ayuda Contextual (F1)

## ?? Resumen Ejecutivo

El sistema de ayuda contextual permite a los usuarios acceder a la documentación presionando **F1** en cualquier formulario. La ayuda se muestra en archivos **CHM** (Compiled HTML Help) que cambian automáticamente según el idioma configurado.

---

## ??? Arquitectura del Sistema de Ayuda

### **Componentes Principales**

```
?? Service/
??? DAL/Implementations/SqlServer/
?   ??? ManualRepository.cs          ? Gestiona archivos CHM y TopicIDs
??? Facade/
?   ??? ManualService.cs             ? Fachada que expone métodos de ayuda
?
?? Distribuidora_los_amigos/
??? Properties/
?   ??? App.config                   ? Configuración de rutas de archivos CHM
??? Forms/
    ??? [Todos los formularios]      ? Implementan KeyDown para F1
```

---

## ?? Configuración en App.config

```xml
<appSettings>
    <!-- Manuales de ayuda -->
    <add key="HelpFilePath_es-ES" value="C:\DistribuidoraLosAmigos\Manual\help_es.chm"/>
    <add key="HelpFilePath_en-US" value="C:\DistribuidoraLosAmigos\Manual\help_us.chm"/>
    <add key="HelpFilePath_Default" value="C:\DistribuidoraLosAmigos\Manual\help_en.chm"/>
</appSettings>
```

**Importante:** 
- Los archivos CHM deben estar en las rutas especificadas
- Se selecciona automáticamente según el idioma del usuario
- Si no existe el archivo para el idioma actual, usa el Default

---

## ?? Mapeo de Formularios y TopicIDs

| Formulario | Método de Ayuda | TopicID |
|------------|----------------|---------|
| **General** |
| MainForm | `AbrirAyudaMain()` | 20 |
| **Login y Seguridad** |
| LoginForm | `AbrirAyudaLogin()` | 31 |
| CambiarPasswordForm | `AbrirAyudaCambiarPass()` | 33 |
| RecuperarPasswordForm | `AbrirAyudaRecuperoPass()` | 32 |
| **Gestión de Usuarios** |
| CrearUsuarioForm | `AbrirAyudaCrearUsuario()` | 23 |
| ModificarUsuarioForm | `AbrirAyudaModUsuario()` | 28 |
| MostrarUsuariosForm | `AbrirAyudaMostrarUsuario()` | 29 |
| **Roles y Permisos** |
| AsignarRolForm | `AbrirAyudaAsignarRol()` | 24 |
| CrearRolForm | `AbrirAyudaCrearRol()` | 26 |
| CrearPatenteForm | `AbrirAyudaCrearPatente()` | 27 |
| **Backup y Restore** |
| BackUpForm | `AbrirAyudaBackUp()` | 22 |
| RestoreForm | `AbrirAyudaRestore()` | 30 |
| **Bitácora** |
| BitacoraForm | `AbrirAyudaBitacora()` | 25 |
| **Clientes** |
| CrearClienteForm | `AbrirAyudaCrearCliente()` | 40 |
| ModificarClienteForm | `AbrirAyudaModificarCliente()` | 41 |
| MostrarClientesForm | `AbrirAyudaMostrarClientes()` | 42 |
| **Productos** |
| CrearProductoForm | `AbrirAyudaCrearProducto()` | 50 |
| ModificarProductoForm | `AbrirAyudaModificarProducto()` | 51 |
| MostrarProductosForm | `AbrirAyudaMostrarProductos()` | 52 |
| EliminarProductoForm | `AbrirAyudaEliminarProducto()` | 53 |
| **Proveedores** |
| CrearProveedorForm | `AbrirAyudaCrearProveedor()` | 60 |
| ModificarProveedorForm | `AbrirAyudaModificarProveedor()` | 61 |
| MostrarProveedoresForm | `AbrirAyudaMostrarProveedores()` | 62 |
| **Stock** |
| MostrarStockForm | `AbrirAyudaMostrarStock()` | 70 |
| ModificarStockForm | `AbrirAyudaModificarStock()` | 71 |
| **Pedidos** |
| CrearPedidoForm | `AbrirAyudaCrearPedido()` | 80 |
| ModificarPedidoForm | `AbrirAyudaModificarPedido()` | 81 |
| MostrarPedidosForm | `AbrirAyudaMostrarPedidos()` | 82 |
| MostrarDetallePedidoForm | `AbrirAyudaDetallePedido()` | 83 |
| **Reportes** |
| ReporteStockBajoForm | `AbrirAyudaReporteStockBajo()` | 90 |
| ReporteProductosMasVendidosForm | `AbrirAyudaReporteProductosMasVendidos()` | 91 |

---

## ?? Implementación en Formularios

### **Patrón de Implementación Completo**

```csharp
using Service.Facade;
using Services.Facade;
using System;
using System.Windows.Forms;

namespace Distribuidora_los_amigos.Forms.TuModulo
{
    public partial class TuFormulario : Form, IIdiomaObserver
    {
        /// <summary>
        /// Constructor del formulario
        /// </summary>
        public TuFormulario()
        {
            InitializeComponent();
            
            // ? PASO 1: Habilitar captura de teclas
            this.KeyPreview = true;
            
            // ? PASO 2: Suscribir evento KeyDown
            this.KeyDown += TuFormulario_KeyDown;
            
            // Configuración de idiomas (si aplica)
            IdiomaService.Subscribe(this);
            IdiomaService.TranslateForm(this);
        }

        /// <summary>
        /// ? PASO 3: Implementar el manejador de F1
        /// </summary>
        private void TuFormulario_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    
                    // ? PASO 4: Llamar al método de ayuda correspondiente
                    manualService.AbrirAyuda[NombreDelMetodo]();
                    
                    // ? PASO 5: Prevenir propagación del evento
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        // ... resto del código del formulario
    }
}
```

---

## ?? Ejemplos Concretos

### **Ejemplo 1: CrearClienteForm**

```csharp
public CrearClienteForm()
{
    InitializeComponent();
    this.KeyPreview = true;
    this.KeyDown += CrearClienteForm_KeyDown;
    
    IdiomaService.Subscribe(this);
    IdiomaService.TranslateForm(this);
}

private void CrearClienteForm_KeyDown(object sender, KeyEventArgs e)
{
    try
    {
        if (e.KeyCode == Keys.F1)
        {
            ManualService manualService = new ManualService();
            manualService.AbrirAyudaCrearCliente(); // ? TopicID 40
            e.Handled = true;
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        LoggerService.WriteException(ex);
    }
}
```

### **Ejemplo 2: MostrarProductosForm**

```csharp
public MostrarProductosForm()
{
    InitializeComponent();
    this.KeyPreview = true;
    this.KeyDown += MostrarProductosForm_KeyDown;
    
    IdiomaService.Subscribe(this);
    IdiomaService.TranslateForm(this);
}

private void MostrarProductosForm_KeyDown(object sender, KeyEventArgs e)
{
    try
    {
        if (e.KeyCode == Keys.F1)
        {
            ManualService manualService = new ManualService();
            manualService.AbrirAyudaMostrarProductos(); // ? TopicID 52
            e.Handled = true;
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        LoggerService.WriteException(ex);
    }
}
```

---

## ? Checklist de Implementación

Para cada formulario que necesite ayuda F1:

- [ ] **1. Agregar `using Service.Facade;`** en la parte superior del archivo
- [ ] **2. En el constructor:**
  - [ ] Agregar `this.KeyPreview = true;`
  - [ ] Agregar `this.KeyDown += [NombreFormulario]_KeyDown;`
- [ ] **3. Crear el método `[NombreFormulario]_KeyDown`**
- [ ] **4. Dentro del método:**
  - [ ] Verificar `if (e.KeyCode == Keys.F1)`
  - [ ] Crear instancia de `ManualService`
  - [ ] Llamar al método de ayuda correspondiente
  - [ ] Establecer `e.Handled = true;`
  - [ ] Manejar excepciones con try-catch
- [ ] **5. Verificar que el archivo CHM existe** en las rutas configuradas
- [ ] **6. Probar presionando F1** en el formulario

---

## ?? Errores Comunes y Soluciones

### **1. "No se abre la ayuda al presionar F1"**
**Causa:** `KeyPreview` no está habilitado o el evento no está suscrito.
```csharp
// ? Solución
this.KeyPreview = true;
this.KeyDown += MiFormulario_KeyDown;
```

### **2. "Archivo CHM no encontrado"**
**Causa:** La ruta en `App.config` es incorrecta o el archivo no existe.
```csharp
// ? Verificar ruta
<add key="HelpFilePath_es-ES" value="C:\DistribuidoraLosAmigos\Manual\help_es.chm"/>
```

### **3. "El formulario hijo no captura F1"**
**Causa:** En formularios MDI, el padre puede interceptar la tecla.
```csharp
// ? Solución: Asegurarse de que CADA formulario tenga KeyPreview
this.KeyPreview = true; // En el formulario hijo
```

### **4. "Error: El tipo o el espacio de nombres 'ManualService' no se pudo encontrar"**
**Causa:** Falta el `using` correspondiente.
```csharp
// ? Agregar al inicio del archivo
using Service.Facade;
```

---

## ?? Buenas Prácticas

### **1. Consistencia en Mensajes de Error**
```csharp
catch (Exception ex)
{
    MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
    LoggerService.WriteException(ex);
}
```

### **2. Prevenir Propagación de Eventos**
```csharp
if (e.KeyCode == Keys.F1)
{
    manualService.AbrirAyudaXXX();
    e.Handled = true; // ? IMPORTANTE: Evita que Windows procese F1
}
```

### **3. Documentación del Método**
```csharp
/// <summary>
/// Muestra la ayuda del formulario cuando se presiona F1.
/// </summary>
/// <param name="sender">Origen del evento.</param>
/// <param name="e">Argumentos del evento.</param>
private void MiFormulario_KeyDown(object sender, KeyEventArgs e)
{
    // ...
}
```

---

## ?? Flujo de Ejecución

```
Usuario presiona F1 en el formulario
           ?
FormularioXYZ_KeyDown detecta Keys.F1
           ?
Crea instancia de ManualService
           ?
ManualService lee idioma actual del usuario
           ?
ManualRepository carga ruta del archivo CHM según idioma
           ?
Help.ShowHelp abre el archivo CHM en el TopicID específico
           ?
Windows muestra la ayuda contextual
```

---

## ?? Estado de Implementación

### ? **Ya Implementados:**
- ? MainForm (main.cs)
- ? LoginForm
- ? BackUpForm
- ? RestoreForm
- ? CrearUsuarioForm
- ? CrearRolForm
- ? CrearPatenteForm

### ?? **Pendientes de Implementar:**

**Gestión de Usuarios:**
- [ ] ModificarUsuarioForm
- [ ] MostrarUsuariosForm
- [ ] AsignarRolForm

**Clientes:**
- [ ] CrearClienteForm
- [ ] ModificarClienteForm
- [ ] MostrarClientesForm

**Productos:**
- [ ] CrearProductoForm
- [ ] ModificarProductoForm
- [ ] MostrarProductosForm
- [ ] EliminarProductoForm

**Proveedores:**
- [ ] CrearProveedorForm
- [ ] ModificarProveedorForm
- [ ] MostrarProveedoresForm

**Stock:**
- [ ] MostrarStockForm
- [ ] ModificarStockForm

**Pedidos:**
- [ ] CrearPedidoForm
- [ ] ModificarPedidoForm
- [ ] MostrarPedidosForm
- [ ] MostrarDetallePedidoForm

**Reportes:**
- [ ] ReporteStockBajoForm
- [ ] ReporteProductosMasVendidosForm

**Otros:**
- [ ] RecuperarPasswordForm
- [ ] BitacoraForm (si existe)

---

## ??? Script de Implementación Rápida

Para implementar F1 en un nuevo formulario, sigue estos pasos:

### **1. Modificar el Constructor:**
```csharp
public MiFormulario()
{
    InitializeComponent();
    
    // ? Agregar estas 2 líneas
    this.KeyPreview = true;
    this.KeyDown += MiFormulario_KeyDown;
    
    // ... resto del código existente
}
```

### **2. Agregar el Método KeyDown:**
```csharp
/// <summary>
/// Muestra la ayuda del formulario cuando se presiona F1.
/// </summary>
private void MiFormulario_KeyDown(object sender, KeyEventArgs e)
{
    try
    {
        if (e.KeyCode == Keys.F1)
        {
            ManualService manualService = new ManualService();
            manualService.AbrirAyuda[METODO_CORRESPONDIENTE]();
            e.Handled = true;
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        LoggerService.WriteException(ex);
    }
}
```

### **3. Agregar using si es necesario:**
```csharp
using Service.Facade;
```

---

## ?? Recursos Adicionales

- **ManualRepository.cs:** `Service/DAL/Implementations/SqlServer/ManualRepository.cs`
- **ManualService.cs:** `Service/Facade/ManualService.cs`
- **App.config:** `Distribuidora_los_amigos/Properties/App.config`

---

## ?? Próximos Pasos

1. **Implementar F1 en todos los formularios pendientes** usando esta guía
2. **Crear archivos CHM** con la documentación para cada TopicID
3. **Probar en cada idioma** (es-ES, en-US, pt-PT)
4. **Documentar el contenido de cada TopicID** en los archivos CHM

---

? **Con esta implementación, los usuarios podrán obtener ayuda contextual en cualquier formulario presionando F1.**
