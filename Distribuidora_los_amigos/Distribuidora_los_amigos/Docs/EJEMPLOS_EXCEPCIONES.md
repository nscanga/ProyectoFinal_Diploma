# Ejemplos Prácticos de Manejo de Excepciones

## ?? Ejemplos de Implementación en BLL

### Ejemplo 1: Servicio Básico con Manejo de Excepciones

```csharp
using BLL.Exceptions;
using BLL.Helpers;
using DAL;
using DOMAIN;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class PedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService()
        {
            _pedidoRepository = FactoryDAL.SqlPedidoRepository;
        }

        /// <summary>
        /// Obtiene todos los pedidos con manejo de excepciones.
        /// </summary>
        public List<Pedido> ObtenerTodosLosPedidos()
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _pedidoRepository.GetAll();
                }, "Error al obtener la lista de pedidos");
            }
            catch (DatabaseException dbEx)
            {
                // Si es un error recuperable, devolver lista vacía
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    // Log del error (ya se hizo automáticamente)
                    return new List<Pedido>(); // Devolver lista vacía para que la app continúe
                }
                // Si no es recuperable, propagar
                throw;
            }
        }

        /// <summary>
        /// Crea un pedido con validaciones de negocio completas.
        /// </summary>
        public void CrearPedido(Pedido pedido, List<DetallePedido> detalles)
        {
            // 1. Validaciones de negocio PRIMERO
            if (pedido.IdCliente == Guid.Empty)
            {
                throw new BusinessException("El cliente es requerido", "CLIENTE_REQUERIDO");
            }

            if (detalles == null || detalles.Count == 0)
            {
                throw new PedidoException("El pedido debe tener al menos un producto", "PEDIDO_SIN_PRODUCTOS");
            }

            // 2. Validar stock ANTES de intentar guardar
            foreach (var detalle in detalles)
            {
                try
                {
                    var stock = _stockRepository.GetByProductoId(detalle.IdProducto);
                    
                    if (stock == null)
                    {
                        throw new StockException($"No se encontró stock para el producto {detalle.IdProducto}", "STOCK_NO_ENCONTRADO");
                    }

                    if (stock.Cantidad < detalle.Cantidad)
                    {
                        throw StockException.StockInsuficiente(
                            detalle.IdProducto,
                            detalle.Cantidad,
                            stock.Cantidad
                        );
                    }
                }
                catch (DALException dalEx)
                {
                    throw ExceptionMapper.MapToBusinessException(dalEx, "Error al verificar stock");
                }
            }

            // 3. Guardar en BD con manejo de excepciones
            try
            {
                pedido.Total = detalles.Sum(d => d.Subtotal);
                
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _pedidoRepository.Add(pedido);
                    
                    foreach (var detalle in detalles)
                    {
                        detalle.IdPedido = pedido.IdPedido;
                        _detallePedidoRepository.Add(detalle);
                        _stockService.DisminuirStock(detalle.IdProducto, detalle.Cantidad);
                    }
                }, "Error al crear el pedido");
            }
            catch (DatabaseException dbEx)
            {
                // Log automático ya se hizo
                // Aquí puedes agregar lógica de compensación si es necesario
                throw; // Propagar para que la UI maneje
            }
        }

        /// <summary>
        /// Obtiene un pedido por ID con validaciones.
        /// </summary>
        public Pedido ObtenerPedidoPorId(Guid idPedido)
        {
            try
            {
                var pedido = ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _pedidoRepository.GetById(idPedido);
                }, $"Error al obtener el pedido {idPedido}");

                if (pedido == null)
                {
                    throw PedidoException.PedidoNoEncontrado((int)idPedido);
                }

                // Cargar detalles
                pedido.Detalles = _detallePedidoRepository.GetByPedido(idPedido);
                
                return pedido;
            }
            catch (DatabaseException dbEx)
            {
                // Si hay error de conexión, devolver null en lugar de fallar
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    return null;
                }
                throw;
            }
        }
    }
}
```

---

## ??? Ejemplos de Implementación en UI

### Ejemplo 1: Form de Listado con Manejo de Errores Recuperables

```csharp
using Service.ManegerEx;
using BLL;
using BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Distribuidora_los_amigos.Forms.Pedidos
{
    public partial class MostrarPedidosForm : Form
    {
        private readonly PedidoService _pedidoService;
        private List<Pedido> _pedidosCache; // Cache local

        public MostrarPedidosForm()
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
            _pedidosCache = new List<Pedido>();
        }

        private void MostrarPedidosForm_Load(object sender, EventArgs e)
        {
            CargarPedidos();
        }

        private void CargarPedidos()
        {
            try
            {
                var pedidos = _pedidoService.ObtenerTodosLosPedidos();
                
                if (pedidos != null && pedidos.Count > 0)
                {
                    _pedidosCache = pedidos; // Actualizar cache
                    dgvPedidos.DataSource = pedidos;
                    lblEstado.Text = $"Se cargaron {pedidos.Count} pedidos";
                    lblEstado.ForeColor = Color.Green;
                }
                else
                {
                    MostrarModoSinConexion();
                }
            }
            catch (DatabaseException dbEx)
            {
                string username = ObtenerUsuarioActual();
                
                // Si es error recuperable, mostrar datos en caché
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                    MostrarModoSinConexion();
                }
                else
                {
                    // Error crítico - cerrar formulario
                    ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                    MessageBox.Show("No se puede continuar sin conexión a la base de datos.", 
                        "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
                this.Close();
            }
        }

        private void MostrarModoSinConexion()
        {
            if (_pedidosCache.Count > 0)
            {
                dgvPedidos.DataSource = _pedidosCache;
                lblEstado.Text = "?? MODO SIN CONEXIÓN - Mostrando datos en caché";
                lblEstado.ForeColor = Color.Orange;
            }
            else
            {
                dgvPedidos.DataSource = null;
                lblEstado.Text = "? Sin conexión - No hay datos en caché";
                lblEstado.ForeColor = Color.Red;
            }
            
            // Deshabilitar botones que requieren BD
            btnCrear.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private string ObtenerUsuarioActual()
        {
            try
            {
                return SesionService.ObtenerUsuarioActual()?.UserName ?? "Desconocido";
            }
            catch
            {
                return "Desconocido";
            }
        }
    }
}
```

### Ejemplo 2: Form de Creación con Validaciones Completas

```csharp
using Service.ManegerEx;
using BLL;
using BLL.Exceptions;
using System;
using System.Windows.Forms;

namespace Distribuidora_los_amigos.Forms.Pedidos
{
    public partial class CrearPedidoForm : Form
    {
        private readonly PedidoService _pedidoService;

        public CrearPedidoForm()
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validaciones de UI
                if (!ValidarFormulario())
                {
                    return;
                }

                // 2. Construir el pedido
                var pedido = ConstruirPedido();
                var detalles = ObtenerDetalles();

                // 3. Intentar guardar
                _pedidoService.CrearPedido(pedido, detalles);

                // 4. Éxito
                MessageBox.Show("Pedido creado exitosamente", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (StockException stockEx)
            {
                // Error de stock - mostrar mensaje específico
                MessageBox.Show(stockEx.Message, "Stock Insuficiente",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (PedidoException pedEx)
            {
                // Error de validación de pedido
                MessageBox.Show(pedEx.Message, "Error de Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (BusinessException bizEx)
            {
                // Otro error de negocio
                MessageBox.Show(bizEx.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DatabaseException dbEx)
            {
                // Error de base de datos
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                
                // Sugerir al usuario intentar más tarde
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    var result = MessageBox.Show(
                        "No se pudo guardar el pedido debido a problemas de conexión.\n\n" +
                        "¿Desea guardar los datos localmente e intentar más tarde?",
                        "Error de Conexión",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        GuardarEnCacheLocal(pedido, detalles);
                    }
                }
            }
            catch (Exception ex)
            {
                // Error inesperado
                ErrorHandler.HandleGeneralException(ex);
            }
        }

        private bool ValidarFormulario()
        {
            if (cmbCliente.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un cliente", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dgvDetalles.Rows.Count == 0)
            {
                MessageBox.Show("El pedido debe tener al menos un producto", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void GuardarEnCacheLocal(Pedido pedido, List<DetallePedido> detalles)
        {
            try
            {
                // Implementar lógica de cache local (archivo JSON, XML, etc.)
                // ...
                
                MessageBox.Show("Datos guardados localmente. Se sincronizarán cuando haya conexión.",
                    "Guardado Local", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar localmente: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
```

### Ejemplo 3: Form con Reintento Automático

```csharp
using Service.ManegerEx;
using BLL;
using BLL.Exceptions;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Distribuidora_los_amigos.Forms
{
    public partial class FormConReintento : Form
    {
        private const int MAX_REINTENTOS = 3;
        private const int DELAY_REINTENTOS_MS = 2000; // 2 segundos

        private readonly PedidoService _pedidoService;

        public FormConReintento()
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
        }

        private async void btnCargar_Click(object sender, EventArgs e)
        {
            await CargarDatosConReintento();
        }

        private async Task CargarDatosConReintento()
        {
            int intentos = 0;
            bool exito = false;

            while (intentos < MAX_REINTENTOS && !exito)
            {
                try
                {
                    intentos++;
                    lblEstado.Text = $"Intentando cargar datos... (Intento {intentos}/{MAX_REINTENTOS})";
                    
                    var pedidos = await Task.Run(() => _pedidoService.ObtenerTodosLosPedidos());
                    
                    dgvPedidos.DataSource = pedidos;
                    lblEstado.Text = $"? Datos cargados exitosamente ({pedidos.Count} registros)";
                    lblEstado.ForeColor = Color.Green;
                    exito = true;
                }
                catch (DatabaseException dbEx) when (
                    dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    if (intentos < MAX_REINTENTOS)
                    {
                        lblEstado.Text = $"?? Error de conexión. Reintentando en {DELAY_REINTENTOS_MS/1000} segundos...";
                        lblEstado.ForeColor = Color.Orange;
                        await Task.Delay(DELAY_REINTENTOS_MS);
                    }
                    else
                    {
                        lblEstado.Text = $"? No se pudo conectar después de {MAX_REINTENTOS} intentos";
                        lblEstado.ForeColor = Color.Red;
                        
                        string username = ObtenerUsuarioActual();
                        ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                    }
                }
                catch (Exception ex)
                {
                    lblEstado.Text = $"? Error inesperado";
                    lblEstado.ForeColor = Color.Red;
                    ErrorHandler.HandleGeneralException(ex);
                    break;
                }
            }
        }

        private string ObtenerUsuarioActual()
        {
            try
            {
                return SesionService.ObtenerUsuarioActual()?.UserName ?? "Desconocido";
            }
            catch
            {
                return "Desconocido";
            }
        }
    }
}
```

---

## ?? Ejemplo de Migración de Código Existente

### ANTES (Sin manejo de excepciones)

```csharp
public List<Pedido> ObtenerEstadosPedido()
{
    return _pedidoRepository.ObtenerEstadosPedido();
}
```

### DESPUÉS (Con manejo robusto)

```csharp
public List<EstadoPedido> ObtenerEstadosPedido()
{
    try
    {
        return ExceptionMapper.ExecuteWithMapping(() =>
        {
            return _pedidoRepository.ObtenerEstadosPedido();
        }, "Error al obtener estados de pedido");
    }
    catch (DatabaseException dbEx)
    {
        // Si falla, devolver estados por defecto
        if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
        {
            return ObtenerEstadosPorDefecto();
        }
        throw;
    }
}

private List<EstadoPedido> ObtenerEstadosPorDefecto()
{
    return new List<EstadoPedido>
    {
        new EstadoPedido { IdEstadoPedido = Guid.NewGuid(), NombreEstado = "Pendiente" },
        new EstadoPedido { IdEstadoPedido = Guid.NewGuid(), NombreEstado = "En Proceso" },
        new EstadoPedido { IdEstadoPedido = Guid.NewGuid(), NombreEstado = "Completado" }
    };
}
```

---

## ?? Checklist de Implementación

### Para cada método BLL:

- [ ] Usar `ExceptionMapper.ExecuteWithMapping()` para operaciones DAL
- [ ] Lanzar excepciones de negocio específicas (`ProductoException`, `PedidoException`, etc.)
- [ ] Validar entradas ANTES de acceder a BD
- [ ] Decidir qué hacer con errores recuperables (cache, valores por defecto, etc.)
- [ ] Documentar comportamiento en caso de error en el XML doc

### Para cada Form UI:

- [ ] Capturar excepciones específicas en orden (más específica a más general)
- [ ] Usar `ErrorHandler` para manejar errores SQL
- [ ] Mostrar mensajes amigables al usuario
- [ ] Implementar modo degradado para errores recuperables
- [ ] No cerrar form si el error es recuperable
- [ ] Obtener username para logs

### Logs automáticos:

- [ ] Todos los errores manejados por `ErrorHandler` se registran automáticamente
- [ ] Los errores van a BD Bitácora si está disponible
- [ ] Fallback a archivos si BD no disponible
- [ ] No es necesario llamar a `LoggerService` manualmente en la mayoría de casos

---

## ?? Patrones Recomendados

### Patrón 1: Try-Catch Granular

```csharp
// ? Bueno - Captura excepciones específicas
try
{
    // operación
}
catch (StockException stockEx)
{
    // Manejo específico para stock
}
catch (PedidoException pedEx)
{
    // Manejo específico para pedidos
}
catch (DatabaseException dbEx)
{
    // Manejo específico para BD
}
catch (Exception ex)
{
    // Manejo genérico
}
```

### Patrón 2: Fail-Safe con Cache

```csharp
public List<T> ObtenerDatos()
{
    try
    {
        var datos = _repository.GetAll();
        _cache.Guardar(datos); // Actualizar cache
        return datos;
    }
    catch (DatabaseException)
    {
        return _cache.Obtener() ?? new List<T>(); // Devolver cache o vacío
    }
}
```

### Patrón 3: Circuit Breaker Simple

```csharp
private int _erroresConsecutivos = 0;
private const int MAX_ERRORES = 5;

public List<T> ObtenerDatos()
{
    if (_erroresConsecutivos >= MAX_ERRORES)
    {
        throw new Exception("Circuito abierto - demasiados errores");
    }

    try
    {
        var datos = _repository.GetAll();
        _erroresConsecutivos = 0; // Reset en éxito
        return datos;
    }
    catch (DatabaseException)
    {
        _erroresConsecutivos++;
        throw;
    }
}
```

---

**Última actualización:** 2024  
**Sistema:** Distribuidora Los Amigos
