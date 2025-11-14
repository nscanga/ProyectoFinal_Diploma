# ??? Plan de Implementación de Resiliencia Completa

## ?? Objetivo
Hacer que toda la aplicación sea resiliente a errores de conexión de base de datos, permitiendo que continúe funcionando con funcionalidad limitada cuando SQL Server no esté disponible.

---

## ? YA IMPLEMENTADO

### 1. **Infraestructura Base** ?
- ? `DAL\Exceptions\DALException.cs` - Excepciones personalizadas DAL
- ? `DAL\Implementations\SqlServer\Helpers\SQLHelper.cs` - Manejo en capa de datos
- ? `BLL\Exceptions\DatabaseException.cs` - Excepciones de negocio
- ? `BLL\Exceptions\DatabaseErrorType.cs` - Tipos de error
- ? `BLL\Helpers\ExceptionMapper.cs` - Mapeo de excepciones
- ? `Service\ManegerEx\ErrorHandler.cs` - Manejo centralizado en UI

### 2. **Módulo de Pedidos** ?
- ? `BLL\PedidoService.cs`
  - ? `ObtenerEstadosPedido()` - Con estados por defecto
  - ? `ObtenerTodosLosPedidos()` - Con manejo de excepciones
- ? `Distribuidora_los_amigos\Forms\Pedidos\MostrarPedidosForm.cs`
  - ? `CargarPedidos()` - Con manejo de excepciones
  - ? `CargarEstadosEnCombo()` - Con manejo de excepciones

---

## ?? PENDIENTE DE IMPLEMENTACIÓN

### PRIORIDAD ALTA - Servicios BLL Core

#### 1. **ClienteService.cs** ?? CRÍTICO
**Ubicación:** `BLL\ClienteService.cs`

**Métodos a modificar:**
```csharp
// ? SIN MANEJO
public List<Cliente> ObtenerTodosLosClientes()
{
    return _clienteRepository.GetAll();
}

// ? CON MANEJO
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
            Console.WriteLine("?? Error de conexión al obtener clientes.");
            throw; // Propagar para que la UI maneje
        }
        throw;
    }
}
```

**Otros métodos:**
- `CrearCliente(Cliente cliente)` - Validar antes de insertar
- `ModificarCliente(Cliente cliente)` - Validar antes de actualizar
- `EliminarCliente(Guid idCliente)` - Validar antes de eliminar
- `ObtenerClientePorId(Guid idCliente)` - Con manejo

**Impacto:** Alto - Afecta todo el módulo de clientes

---

#### 2. **ProductoService.cs** ?? CRÍTICO
**Ubicación:** `BLL\ProductoService.cs`

**Métodos a modificar:**
```csharp
public List<Producto> ObtenerTodosLosProductos()
{
    try
    {
        return ExceptionMapper.ExecuteWithMapping(() =>
        {
            return _productoRepository.GetAll();
        }, "Error al obtener productos");
    }
    catch (DatabaseException dbEx)
    {
        if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
            dbEx.ErrorType == DatabaseErrorType.Timeout)
        {
            Console.WriteLine("?? Error de conexión al obtener productos.");
            throw;
        }
        throw;
    }
}
```

**Otros métodos:**
- `CrearProducto(Producto producto)` - Validar antes de insertar
- `ModificarProducto(Producto producto)` - Validar antes de actualizar
- `EliminarProducto(Guid idProducto)` - Validar antes de eliminar
- `ObtenerProductoPorId(Guid idProducto)` - Con manejo
- `BuscarProductosPorNombre(string nombre)` - Con manejo

**Impacto:** Alto - Afecta todo el módulo de productos

---

#### 3. **StockService.cs** ?? CRÍTICO
**Ubicación:** `BLL\StockService.cs`

**Métodos a modificar:**
```csharp
public List<Stock> ObtenerTodosLosStocks()
{
    try
    {
        return ExceptionMapper.ExecuteWithMapping(() =>
        {
            return _stockRepository.GetAll();
        }, "Error al obtener stocks");
    }
    catch (DatabaseException dbEx)
    {
        if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
            dbEx.ErrorType == DatabaseErrorType.Timeout)
        {
            Console.WriteLine("?? Error de conexión al obtener stocks.");
            throw;
        }
        throw;
    }
}

public void DisminuirStock(Guid idProducto, int cantidad)
{
    try
    {
        ExceptionMapper.ExecuteWithMapping(() =>
        {
            // Validar stock disponible primero
            var stock = _stockRepository.GetByProductoId(idProducto);
            if (stock.Cantidad < cantidad)
            {
                throw StockException.StockInsuficiente(idProducto, cantidad, stock.Cantidad);
            }
            _stockRepository.DisminuirStock(idProducto, cantidad);
        }, "Error al disminuir stock");
    }
    catch (DatabaseException dbEx)
    {
        if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
        {
            throw new StockException("No se puede actualizar el stock: Sin conexión a la base de datos");
        }
        throw;
    }
}
```

**Otros métodos:**
- `AumentarStock(Guid idProducto, int cantidad)` - Con manejo
- `ObtenerStockPorProducto(Guid idProducto)` - Con manejo
- `ObtenerProductosBajoMinimo()` - Con manejo (para reportes)

**Impacto:** Alto - Crítico para operaciones de pedidos

---

#### 4. **ProveedorService.cs** ?? MEDIO
**Ubicación:** `BLL\ProveedorService.cs`

**Métodos a modificar:**
```csharp
public List<Proveedor> ObtenerTodosLosProveedores()
{
    try
    {
        return ExceptionMapper.ExecuteWithMapping(() =>
        {
            return _proveedorRepository.GetAll();
        }, "Error al obtener proveedores");
    }
    catch (DatabaseException dbEx)
    {
        if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
            dbEx.ErrorType == DatabaseErrorType.Timeout)
        {
            Console.WriteLine("?? Error de conexión al obtener proveedores.");
            throw;
        }
        throw;
    }
}
```

**Otros métodos:**
- `CrearProveedor(Proveedor proveedor)` - Con validación
- `ModificarProveedor(Proveedor proveedor)` - Con validación
- `EliminarProveedor(Guid idProveedor)` - Con validación

**Impacto:** Medio - Afecta gestión de proveedores

---

### PRIORIDAD ALTA - Formularios de Listado

#### 1. **MostrarClientesForm.cs** ?? CRÍTICO
**Ubicación:** `Distribuidora_los_amigos\Forms\Clientes\MostrarClientesForm.cs`

**Método a modificar:**
```csharp
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

**También modificar:**
- `MostrarClientesForm_Load()` - Wrap con try/catch

**Impacto:** Alto - Formulario muy usado

---

#### 2. **MostrarProductosForm.cs** ?? CRÍTICO
**Ubicación:** `Distribuidora_los_amigos\Forms\Productos\MostrarProductosForm.cs`

**Método a modificar:**
```csharp
private void CargarProductos()
{
    try
    {
        List<Producto> listaProductos = _productoService.ObtenerTodosLosProductos();
        
        var productosEnriquecidos = listaProductos.Select(p => new
        {
            IdProducto = p.IdProducto,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            Precio = p.Precio,
            Stock = ObtenerStockProducto(p.IdProducto),
            ProductoOriginal = p
        }).ToList();
        
        dataGridViewProductos.DataSource = productosEnriquecidos;
    }
    catch (DatabaseException dbEx)
    {
        string username = ObtenerUsuarioActual();
        ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
        dataGridViewProductos.DataSource = new List<object>();
        Console.WriteLine("? Error de conexión al cargar productos");
    }
    catch (Exception ex)
    {
        ErrorHandler.HandleGeneralException(ex);
        dataGridViewProductos.DataSource = new List<object>();
    }
}

private int ObtenerStockProducto(Guid idProducto)
{
    try
    {
        var stock = _stockService.ObtenerStockPorProducto(idProducto);
        return stock?.Cantidad ?? 0;
    }
    catch
    {
        return 0; // Si falla, mostrar 0
    }
}
```

**Impacto:** Alto - Formulario muy usado

---

#### 3. **MostrarStockForm.cs** ?? CRÍTICO
**Ubicación:** `Distribuidora_los_amigos\Forms\StockForm\MostrarStockForm.cs`

**Método a modificar:**
```csharp
private void CargarStocks()
{
    try
    {
        List<Stock> listaStocks = _stockService.ObtenerTodosLosStocks();
        
        var stocksEnriquecidos = listaStocks.Select(s => new
        {
            IdStock = s.IdStock,
            IdProducto = s.IdProducto,
            NombreProducto = ObtenerNombreProducto(s.IdProducto),
            Cantidad = s.Cantidad,
            StockMinimo = s.StockMinimo,
            BajoMinimo = s.Cantidad < s.StockMinimo,
            StockOriginal = s
        }).ToList();
        
        dataGridViewStocks.DataSource = stocksEnriquecidos;
    }
    catch (DatabaseException dbEx)
    {
        string username = ObtenerUsuarioActual();
        ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
        dataGridViewStocks.DataSource = new List<object>();
        Console.WriteLine("? Error de conexión al cargar stocks");
    }
    catch (Exception ex)
    {
        ErrorHandler.HandleGeneralException(ex);
        dataGridViewStocks.DataSource = new List<object>();
    }
}
```

**Impacto:** Alto - Crítico para control de inventario

---

#### 4. **MostrarProveedoresForm.cs** ?? MEDIO
**Ubicación:** `Distribuidora_los_amigos\Forms\Proveedores\MostrarProveedoresForm.cs`

**Método a modificar:** Similar a MostrarClientesForm

**Impacto:** Medio - Usado con menos frecuencia

---

### PRIORIDAD MEDIA - Formularios de Creación/Modificación

#### 1. **CrearPedidoForm.cs** ?? CRÍTICO
**Ubicación:** `Distribuidora_los_amigos\Forms\Pedidos\CrearPedidoForm.cs`

**Métodos a modificar:**
```csharp
private void btnGuardar_Click(object sender, EventArgs e)
{
    try
    {
        // Validaciones...
        _pedidoService.CrearPedido(pedido, detalles);
        MessageBox.Show("Pedido creado exitosamente", "Éxito", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.Close();
    }
    catch (DatabaseException dbEx)
    {
        string username = ObtenerUsuarioActual();
        ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
        
        if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
        {
            MessageBox.Show(
                "No se puede crear el pedido sin conexión a la base de datos.\n" +
                "Por favor, verifique la conexión e intente nuevamente.",
                "Error de Conexión",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
    catch (StockException stockEx)
    {
        MessageBox.Show(stockEx.Message, "Stock Insuficiente", 
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
    catch (PedidoException pedEx)
    {
        MessageBox.Show(pedEx.Message, "Error de Validación", 
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
    catch (Exception ex)
    {
        ErrorHandler.HandleGeneralException(ex);
    }
}

private void CargarProductos()
{
    try
    {
        var productos = _productoService.ObtenerTodosLosProductos();
        cmbProductos.DataSource = productos;
    }
    catch (DatabaseException dbEx)
    {
        ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
        cmbProductos.Enabled = false;
        btnAgregar.Enabled = false;
    }
}

private void CargarClientes()
{
    try
    {
        var clientes = _clienteService.ObtenerTodosLosClientes();
        cmbClientes.DataSource = clientes;
    }
    catch (DatabaseException dbEx)
    {
        ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
        cmbClientes.Enabled = false;
    }
}
```

**Impacto:** Alto - Operación crítica de negocio

---

#### 2. **ModificarPedidoForm.cs** ?? MEDIO
Similar a CrearPedidoForm

---

#### 3. **CrearClienteForm.cs** ?? MEDIO
**Método btnGuardar_Click:** Similar al de CrearPedidoForm

---

#### 4. **CrearProductoForm.cs** ?? MEDIO
**Método btnGuardar_Click:** Similar al de CrearPedidoForm

---

### PRIORIDAD MEDIA - Reportes

#### 1. **ReporteStockBajoForm.cs** ?? MEDIO
**Ubicación:** `Distribuidora_los_amigos\Forms\Reportes\ReporteStockBajoForm.cs`

```csharp
private void CargarReporte()
{
    try
    {
        var productosAlertas = _stockService.ObtenerProductosBajoMinimo();
        dgvReporte.DataSource = productosAlertas;
    }
    catch (DatabaseException dbEx)
    {
        ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
        dgvReporte.DataSource = new List<object>();
        lblInfo.Text = "?? No se puede generar el reporte sin conexión";
    }
}
```

---

#### 2. **ReporteProductosMasVendidosForm.cs** ?? MEDIO
Similar a ReporteStockBajoForm

---

### PRIORIDAD BAJA - Otros Módulos

#### 1. **LoginForm.cs** ?? BAJO
Ya debería tener manejo de errores, revisar y mejorar si es necesario.

#### 2. **BackUpForm.cs** ?? BAJO
```csharp
private void btnRealizarBackup_Click(object sender, EventArgs e)
{
    try
    {
        _backupService.RealizarBackup();
        MessageBox.Show("Backup realizado exitosamente");
    }
    catch (DatabaseException dbEx)
    {
        ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
        MessageBox.Show(
            "No se puede realizar el backup sin conexión a la base de datos.",
            "Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }
}
```

---

## ?? Resumen de Implementación

### Por Prioridad

| Prioridad | Componentes | Cantidad | Estado |
|-----------|-------------|----------|--------|
| ? **Completado** | PedidoService, MostrarPedidosForm | 2 | ? HECHO |
| ?? **Alta** | ClienteService, ProductoService, StockService, ProveedorService | 4 | ? PENDIENTE |
| ?? **Alta** | MostrarClientesForm, MostrarProductosForm, MostrarStockForm | 3 | ? PENDIENTE |
| ?? **Media** | CrearPedidoForm, ModificarPedidoForm, CrearClienteForm, CrearProductoForm | 4 | ? PENDIENTE |
| ?? **Media** | ReporteStockBajoForm, ReporteProductosMasVendidosForm | 2 | ? PENDIENTE |
| ?? **Baja** | BackUpForm, RestoreForm, GestionUsuarios | 5+ | ? PENDIENTE |

---

## ?? Estrategia de Implementación

### Fase 1: Servicios Core (CRÍTICO) ??
**Orden sugerido:**
1. ? `PedidoService` - YA COMPLETADO
2. `ClienteService` - SIGUIENTE
3. `ProductoService` - SIGUIENTE
4. `StockService` - SIGUIENTE
5. `ProveedorService` - SIGUIENTE

**Tiempo estimado:** 2-3 horas

---

### Fase 2: Formularios de Listado (CRÍTICO) ??
**Orden sugerido:**
1. ? `MostrarPedidosForm` - YA COMPLETADO
2. `MostrarClientesForm` - SIGUIENTE
3. `MostrarProductosForm` - SIGUIENTE
4. `MostrarStockForm` - SIGUIENTE
5. `MostrarProveedoresForm` - SIGUIENTE

**Tiempo estimado:** 2-3 horas

---

### Fase 3: Formularios de Creación (MEDIO) ??
**Orden sugerido:**
1. `CrearPedidoForm` - Más crítico
2. `CrearClienteForm`
3. `CrearProductoForm`
4. `ModificarPedidoForm`
5. Otros formularios de modificación

**Tiempo estimado:** 3-4 horas

---

### Fase 4: Reportes y Otros (BAJO) ??
**Orden sugerido:**
1. `ReporteStockBajoForm`
2. `ReporteProductosMasVendidosForm`
3. `BackUpForm`
4. Otros

**Tiempo estimado:** 1-2 horas

---

## ?? Plantilla de Implementación

### Para Servicios BLL:
```csharp
using BLL.Exceptions;
using BLL.Helpers;

public List<T> ObtenerTodos()
{
    try
    {
        return ExceptionMapper.ExecuteWithMapping(() =>
        {
            return _repository.GetAll();
        }, "Error al obtener [entidad]");
    }
    catch (DatabaseException dbEx)
    {
        if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
            dbEx.ErrorType == DatabaseErrorType.Timeout)
        {
            Console.WriteLine($"?? Error de conexión al obtener [entidad].");
            throw; // Propagar para que UI maneje
        }
        throw;
    }
}
```

### Para Formularios:
```csharp
using BLL.Exceptions;
using Service.ManegerEx;

private void CargarDatos()
{
    try
    {
        var datos = _service.ObtenerTodos();
        dataGridView.DataSource = datos;
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

---

## ? Checklist de Implementación

### Por cada Servicio:
- [ ] Agregar `using BLL.Exceptions;`
- [ ] Agregar `using BLL.Helpers;`
- [ ] Envolver llamadas a repositorio con `ExceptionMapper.ExecuteWithMapping()`
- [ ] Agregar catch para `DatabaseException`
- [ ] Verificar errores recuperables
- [ ] Agregar logs con `Console.WriteLine()`
- [ ] Probar con SQL Server detenido

### Por cada Formulario:
- [ ] Agregar `using BLL.Exceptions;`
- [ ] Agregar `using Service.ManegerEx;`
- [ ] Agregar método `ObtenerUsuarioActual()`
- [ ] Envolver `CargarDatos()` con try/catch
- [ ] Catch específico para `DatabaseException`
- [ ] Llamar a `ErrorHandler.HandleDatabaseException()`
- [ ] Limpiar DataGridView en caso de error
- [ ] Probar con SQL Server detenido
- [ ] Verificar que no crashee

---

## ?? Testing por Fase

### Fase 1: Servicios
**Test:** Detener SQL Server, ejecutar tests unitarios o llamar métodos desde consola
**Resultado esperado:** DatabaseException con mensaje claro

### Fase 2: Formularios de Listado
**Test:** Detener SQL Server, abrir cada formulario
**Resultado esperado:** 
- Formulario se abre ?
- Mensaje de advertencia ?
- Grid vacío ?
- No crash ?

### Fase 3: Formularios de Creación
**Test:** Detener SQL Server, intentar crear/modificar registro
**Resultado esperado:**
- Error claro ?
- No se pierde información ingresada ?
- Formulario no se cierra ?

---

## ?? Métricas de Éxito

### Antes de Implementación
- ? 0% de resiliencia
- ? App crashea sin SQL Server
- ? Pérdida de datos
- ? Experiencia de usuario mala

### Después de Fase 1 (Servicios)
- ?? 30% de resiliencia
- ?? Algunos módulos funcionan
- ? Infraestructura base lista

### Después de Fase 2 (Listados)
- ?? 60% de resiliencia
- ? Formularios principales no crashean
- ? Mensajes amigables

### Después de Fase 3 (Creación)
- ? 85% de resiliencia
- ? Operaciones críticas protegidas
- ? App completamente funcional

### Después de Fase 4 (Otros)
- ? 100% de resiliencia
- ? Todos los módulos protegidos
- ? Experiencia de usuario excelente

---

## ?? Conclusión

Con este plan, la aplicación será **completamente resiliente** a errores de conexión de base de datos.

**Próximo paso recomendado:** Comenzar con **Fase 1 - ClienteService**

**Tiempo total estimado:** 8-12 horas de desarrollo

**Beneficios:**
- ? App no crashea nunca
- ? Usuarios pueden seguir trabajando
- ? Datos se preservan
- ? Logs automáticos
- ? Mejor experiencia de usuario
- ? Aplicación profesional y robusta

---

**Última actualización:** 2024  
**Estado:** ?? PLAN COMPLETO - LISTO PARA EJECUTAR
