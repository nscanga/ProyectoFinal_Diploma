# ?? Resumen Ejecutivo: Plan de Resiliencia

## ? COMPLETADO (20%)

### Módulo de Pedidos
- ? `BLL\PedidoService.cs` - Manejo completo
- ? `MostrarPedidosForm.cs` - Manejo completo
- ? Infraestructura base (SQLHelper, ErrorHandler, ExceptionMapper)

---

## ?? PRIORIDAD ALTA (Siguiente)

### 1. Servicios BLL Core (4 archivos)
```
BLL\ClienteService.cs        - ObtenerTodosLosClientes()
BLL\ProductoService.cs       - ObtenerTodosLosProductos()
BLL\StockService.cs          - ObtenerTodosLosStocks(), DisminuirStock()
BLL\ProveedorService.cs      - ObtenerTodosLosProveedores()
```

### 2. Formularios de Listado (4 archivos)
```
Forms\Clientes\MostrarClientesForm.cs      - CargarClientes()
Forms\Productos\MostrarProductosForm.cs    - CargarProductos()
Forms\StockForm\MostrarStockForm.cs        - CargarStocks()
Forms\Proveedores\MostrarProveedoresForm.cs - CargarProveedores()
```

**Tiempo estimado:** 4-6 horas  
**Impacto:** CRÍTICO - Son los módulos más usados

---

## ?? PRIORIDAD MEDIA

### 3. Formularios de Creación (4 archivos principales)
```
Forms\Pedidos\CrearPedidoForm.cs       - btnGuardar_Click()
Forms\Clientes\CrearClienteForm.cs     - btnGuardar_Click()
Forms\Productos\CrearProductoForm.cs   - btnGuardar_Click()
Forms\Pedidos\ModificarPedidoForm.cs   - btnGuardar_Click()
```

**Tiempo estimado:** 3-4 horas  
**Impacto:** MEDIO - Operaciones importantes pero menos frecuentes

---

## ?? PRIORIDAD BAJA

### 4. Reportes y Otros (3-5 archivos)
```
Forms\Reportes\ReporteStockBajoForm.cs
Forms\Reportes\ReporteProductosMasVendidosForm.cs
Forms\GestionUsuarios\BackUpForm.cs
```

**Tiempo estimado:** 1-2 horas  
**Impacto:** BAJO - Funcionalidades secundarias

---

## ?? Patrón de Implementación

### Para Servicios BLL:
```csharp
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
            throw; // Propagar para UI
        }
        throw;
    }
}
```

### Para Formularios:
```csharp
private void CargarDatos()
{
    try
    {
        var datos = _service.ObtenerTodos();
        dataGridView.DataSource = datos;
    }
    catch (DatabaseException dbEx)
    {
        ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), true);
        dataGridView.DataSource = new List<object>();
    }
    catch (Exception ex)
    {
        ErrorHandler.HandleGeneralException(ex);
    }
}

private string ObtenerUsuarioActual()
{
    try { return SesionService.UsuarioLogueado?.UserName ?? "Desconocido"; }
    catch { return "Desconocido"; }
}
```

---

## ?? Orden Sugerido de Implementación

1. **ClienteService** (30 min) ? **MostrarClientesForm** (30 min)
2. **ProductoService** (30 min) ? **MostrarProductosForm** (45 min)
3. **StockService** (45 min) ? **MostrarStockForm** (30 min)
4. **ProveedorService** (30 min) ? **MostrarProveedoresForm** (30 min)
5. **CrearPedidoForm** (45 min)
6. **Otros formularios de creación** (2-3 horas)
7. **Reportes y BackUp** (1-2 horas)

**Total estimado:** 8-10 horas

---

## ?? Progreso

| Fase | Componentes | % Completado |
|------|-------------|--------------|
| ? Infraestructura | 6/6 | 100% |
| ? Módulo Pedidos | 2/2 | 100% |
| ? Servicios Core | 0/4 | 0% |
| ? Formularios Listado | 0/4 | 0% |
| ? Formularios Creación | 0/4 | 0% |
| ? Reportes | 0/3 | 0% |

**Progreso Global: 20%** (8 de ~25 componentes críticos)

---

## ? Checklist Rápido

### Por cada archivo:
- [ ] Agregar usings necesarios
- [ ] Implementar try/catch con DatabaseException
- [ ] Llamar a ErrorHandler.HandleDatabaseException()
- [ ] Probar con SQL Server detenido
- [ ] Verificar que no crashee

---

## ?? Próximo Paso

**Comenzar con:** `BLL\ClienteService.cs` + `MostrarClientesForm.cs`

**Razón:** Es el módulo más simple y establecerá el patrón para los demás.

---

Ver documento completo: `PLAN_RESILIENCIA_COMPLETA.md`
