# ?? Plantillas Copy-Paste para Resiliencia

## ?? Para Servicios BLL

### 1. Agregar Usings al inicio del archivo:
```csharp
using BLL.Exceptions;
using BLL.Helpers;
```

### 2. Método ObtenerTodos() genérico:
```csharp
/// <summary>
/// Obtiene todos los [entidades] de la base de datos.
/// Si hay error de conexión, propaga la excepción para que la UI la maneje.
/// </summary>
public List<TEntidad> ObtenerTodos()
{
    try
    {
        return ExceptionMapper.ExecuteWithMapping(() =>
        {
            return _repository.GetAll();
        }, "Error al obtener [entidades]");
    }
    catch (DatabaseException dbEx)
    {
        if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
            dbEx.ErrorType == DatabaseErrorType.Timeout)
        {
            Console.WriteLine($"?? Error de conexión al obtener [entidades].");
            throw; // Propagar para que UI maneje
        }
        throw;
    }
}
```

### 3. Método Crear/Modificar genérico:
```csharp
/// <summary>
/// Crea/Modifica un(a) [entidad].
/// </summary>
public void Crear(TEntidad entidad)
{
    try
    {
        // Validaciones de negocio primero
        ValidarEntidad(entidad);
        
        // Luego ejecutar con mapping
        ExceptionMapper.ExecuteWithMapping(() =>
        {
            _repository.Add(entidad); // o Update()
        }, "Error al crear [entidad]");
    }
    catch (DatabaseException dbEx)
    {
        if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
        {
            Console.WriteLine($"? No se puede crear [entidad] sin conexión.");
        }
        throw; // Siempre propagar para que UI maneje
    }
}
```

---

## ??? Para Formularios de Listado

### 1. Agregar Usings al inicio:
```csharp
using BLL.Exceptions;
using Service.ManegerEx;
```

### 2. Método ObtenerUsuarioActual():
```csharp
/// <summary>
/// Obtiene el nombre del usuario actual de la sesión de forma segura.
/// </summary>
private string ObtenerUsuarioActual()
{
    try
    {
        return SesionService.UsuarioLogueado?.UserName ?? "Desconocido";
    }
    catch
    {
        return "Desconocido";
    }
}
```

### 3. Método CargarDatos() con manejo:
```csharp
/// <summary>
/// Carga los datos en el grid.
/// Si hay error de conexión, muestra mensaje pero permite que el formulario continúe.
/// </summary>
private void CargarDatos()
{
    try
    {
        List<TEntidad> listaDatos = _service.ObtenerTodos();
        
        // Si necesitas enriquecer los datos:
        var datosEnriquecidos = listaDatos.Select(d => new
        {
            // Propiedades necesarias...
            Original = d
        }).ToList();
        
        dataGridView.DataSource = datosEnriquecidos; // o listaDatos directamente
        
        if (listaDatos.Count == 0)
        {
            Console.WriteLine("?? No hay datos disponibles.");
        }
    }
    catch (DatabaseException dbEx)
    {
        string username = ObtenerUsuarioActual();
        ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
        dataGridView.DataSource = new List<object>();
        Console.WriteLine("? Error de conexión al cargar datos");
    }
    catch (Exception ex)
    {
        ErrorHandler.HandleGeneralException(ex);
        dataGridView.DataSource = new List<object>();
    }
}
```

### 4. Modificar Form_Load para no crashear:
```csharp
private void Form_Load(object sender, EventArgs e)
{
    if (this.MdiParent != null)
    {
        this.WindowState = FormWindowState.Maximized;
        this.Dock = DockStyle.Fill;
    }

    ConfigurarDataGridView(); // Si existe
    
    try
    {
        CargarDatos();
    }
    catch (Exception ex)
    {
        ErrorHandler.HandleGeneralException(ex);
        // No cerrar el form - permitir uso limitado
    }
}
```

---

## ?? Para Formularios de Creación/Modificación

### 1. Método btnGuardar_Click con manejo completo:
```csharp
private void btnGuardar_Click(object sender, EventArgs e)
{
    try
    {
        // Validaciones locales primero
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            MessageBox.Show("El nombre es requerido", "Validación", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        // Crear/modificar entidad
        var entidad = new TEntidad
        {
            // Propiedades...
        };
        
        _service.Crear(entidad); // o Modificar()
        
        MessageBox.Show("[Entidad] guardada exitosamente", "Éxito", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.DialogResult = DialogResult.OK;
        this.Close();
    }
    catch (DatabaseException dbEx)
    {
        string username = ObtenerUsuarioActual();
        ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
        
        if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
        {
            MessageBox.Show(
                "No se puede guardar sin conexión a la base de datos.\n" +
                "Por favor, verifique la conexión e intente nuevamente.",
                "Error de Conexión",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        // NO CERRAR EL FORMULARIO - El usuario puede reintentar
    }
    catch (BusinessException bizEx)
    {
        // Errores de validación de negocio
        MessageBox.Show(bizEx.Message, "Error de Validación", 
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
    catch (Exception ex)
    {
        ErrorHandler.HandleGeneralException(ex);
    }
}
```

### 2. Cargar ComboBoxes con manejo:
```csharp
private void CargarComboClientes()
{
    try
    {
        var clientes = _clienteService.ObtenerTodosLosClientes();
        cmbClientes.DataSource = clientes;
        cmbClientes.DisplayMember = "Nombre";
        cmbClientes.ValueMember = "IdCliente";
    }
    catch (DatabaseException dbEx)
    {
        ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
        cmbClientes.Enabled = false;
        lblAdvertencia.Text = "?? No se pueden cargar clientes sin conexión";
        lblAdvertencia.Visible = true;
    }
    catch (Exception ex)
    {
        ErrorHandler.HandleGeneralException(ex);
        cmbClientes.Enabled = false;
    }
}
```

---

## ?? Para Reportes

### Método GenerarReporte con manejo:
```csharp
private void GenerarReporte()
{
    try
    {
        var datos = _service.ObtenerDatosReporte();
        
        if (datos == null || datos.Count == 0)
        {
            MessageBox.Show("No hay datos para el reporte", "Información", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        dgvReporte.DataSource = datos;
        // Generar PDF, etc.
    }
    catch (DatabaseException dbEx)
    {
        ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
        dgvReporte.DataSource = new List<object>();
        lblInfo.Text = "?? No se puede generar el reporte sin conexión";
        lblInfo.ForeColor = Color.Orange;
        lblInfo.Visible = true;
    }
    catch (Exception ex)
    {
        ErrorHandler.HandleGeneralException(ex);
    }
}
```

---

## ?? Checklist por Archivo

### Servicios BLL:
```
? 1. Agregar using BLL.Exceptions;
? 2. Agregar using BLL.Helpers;
? 3. Envolver métodos de lectura con ExceptionMapper.ExecuteWithMapping()
? 4. Agregar catch DatabaseException con verificación de tipo de error
? 5. Agregar Console.WriteLine() para debugging
? 6. Compilar y verificar sin errores
? 7. Probar con SQL Server detenido
```

### Formularios:
```
? 1. Agregar using BLL.Exceptions;
? 2. Agregar using Service.ManegerEx;
? 3. Agregar método ObtenerUsuarioActual()
? 4. Modificar CargarDatos() con try/catch completo
? 5. Modificar Form_Load con try/catch
? 6. Modificar btnGuardar_Click con try/catch completo
? 7. Compilar y verificar sin errores
? 8. Probar con SQL Server detenido
? 9. Verificar que muestra mensaje correcto
? 10. Verificar que NO crashea
```

---

## ?? Ejemplo Completo: ClienteService

```csharp
using System;
using System.Collections.Generic;
using DAL.Contratcs;
using DAL.Factory;
using DOMAIN;
using BLL.Exceptions;        // ?? AGREGAR
using BLL.Helpers;           // ?? AGREGAR

namespace BLL
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService()
        {
            _clienteRepository = FactoryDAL.SqlClienteRepository;
        }

        /// <summary>
        /// Obtiene todos los clientes de la base de datos.
        /// Si hay error de conexión, propaga la excepción para que la UI la maneje.
        /// </summary>
        public List<Cliente> ObtenerTodosLosClientes()
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _clienteRepository.GetAll();
                }, "Error al obtener clientes");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"?? Error de conexión al obtener clientes.");
                    throw; // Propagar para que UI maneje
                }
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        public void CrearCliente(Cliente cliente)
        {
            try
            {
                // Validaciones primero
                ValidarCliente(cliente);
                
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _clienteRepository.Add(cliente);
                }, "Error al crear cliente");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"? No se puede crear cliente sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Modifica un cliente existente.
        /// </summary>
        public void ModificarCliente(Cliente cliente)
        {
            try
            {
                ValidarCliente(cliente);
                
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _clienteRepository.Update(cliente);
                }, "Error al modificar cliente");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"? No se puede modificar cliente sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Elimina un cliente por su ID.
        /// </summary>
        public void EliminarCliente(Guid idCliente)
        {
            try
            {
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _clienteRepository.Remove(idCliente);
                }, "Error al eliminar cliente");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"? No se puede eliminar cliente sin conexión.");
                }
                throw;
            }
        }

        private void ValidarCliente(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));
                
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new ClienteException("El nombre del cliente es requerido");
                
            if (string.IsNullOrWhiteSpace(cliente.Cuit))
                throw new ClienteException("El CUIT es requerido");
        }
    }
}
```

---

## ?? Ejemplo Completo: MostrarClientesForm

```csharp
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BLL;
using BLL.Exceptions;        // ?? AGREGAR
using DOMAIN;
using Service.Facade;
using Service.ManegerEx;     // ?? AGREGAR

namespace Distribuidora_los_amigos.Forms.Clientes
{
    public partial class MostrarClientesForm : Form
    {
        private readonly ClienteService _clienteService;

        public MostrarClientesForm()
        {
            InitializeComponent();
            _clienteService = new ClienteService();
        }

        private void MostrarClientesForm_Load(object sender, EventArgs e)
        {
            if (this.MdiParent != null)
            {
                this.WindowState = FormWindowState.Maximized;
                this.Dock = DockStyle.Fill;
            }

            try
            {
                CargarClientes();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
                // No cerrar el form - permitir uso limitado
            }
        }

        /// <summary>
        /// Carga los clientes en el grid.
        /// Si hay error de conexión, muestra mensaje pero permite que el formulario continúe.
        /// </summary>
        private void CargarClientes()
        {
            try
            {
                List<Cliente> listaClientes = _clienteService.ObtenerTodosLosClientes();
                dataGridViewClientes.DataSource = listaClientes;
                
                if (listaClientes.Count == 0)
                {
                    Console.WriteLine("?? No hay clientes disponibles.");
                }
            }
            catch (DatabaseException dbEx)
            {
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                dataGridViewClientes.DataSource = new List<Cliente>();
                Console.WriteLine("? Error de conexión al cargar clientes");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
                dataGridViewClientes.DataSource = new List<Cliente>();
            }
        }

        /// <summary>
        /// Obtiene el nombre del usuario actual de la sesión de forma segura.
        /// </summary>
        private string ObtenerUsuarioActual()
        {
            try
            {
                return SesionService.UsuarioLogueado?.UserName ?? "Desconocido";
            }
            catch
            {
                return "Desconocido";
            }
        }

        private void buttonCrear_Click(object sender, EventArgs e)
        {
            CrearClienteForm form = new CrearClienteForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                CargarClientes(); // Recargar
            }
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            if (dataGridViewClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente", "Advertencia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cliente clienteSeleccionado = (Cliente)dataGridViewClientes.SelectedRows[0].DataBoundItem;
            ModificarClienteForm form = new ModificarClienteForm(clienteSeleccionado);
            if (form.ShowDialog() == DialogResult.OK)
            {
                CargarClientes();
            }
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente", "Advertencia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cliente clienteSeleccionado = (Cliente)dataGridViewClientes.SelectedRows[0].DataBoundItem;
            
            DialogResult confirmacion = MessageBox.Show(
                $"¿Está seguro de eliminar al cliente {clienteSeleccionado.Nombre}?", 
                "Confirmación", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    _clienteService.EliminarCliente(clienteSeleccionado.IdCliente);
                    MessageBox.Show("Cliente eliminado exitosamente", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarClientes();
                }
                catch (DatabaseException dbEx)
                {
                    ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleGeneralException(ex);
                }
            }
        }
    }
}
```

---

## ? Listo para Usar

Estos ejemplos están listos para copiar y pegar. Solo necesitas:
1. Reemplazar `TEntidad` por tu entidad (Cliente, Producto, etc.)
2. Reemplazar nombres de variables según tu código
3. Ajustar validaciones según tu lógica de negocio

**Siguiente paso:** Comenzar con `ClienteService.cs` + `MostrarClientesForm.cs`
