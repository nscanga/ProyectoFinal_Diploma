# Guía de Manejo de Excepciones - Distribuidora Los Amigos

## ?? Descripción General

Este sistema implementa un manejo robusto de excepciones que permite que la aplicación continúe funcionando incluso cuando hay problemas con la base de datos. Todos los errores se registran automáticamente en la bitácora.

---

## ??? Arquitectura de Excepciones

### Capas de Excepciones

```
???????????????????????????????????????????
?           CAPA UI (Forms)               ?
?  - Maneja excepciones con ErrorHandler ?
?  - Muestra mensajes al usuario          ?
???????????????????????????????????????????
               ?
???????????????????????????????????????????
?         CAPA BLL (Negocio)              ?
?  - BusinessException (base)             ?
?  - DatabaseException                    ?
?  - ProductoException                    ?
?  - ClienteException                     ?
?  - PedidoException                      ?
?  - StockException                       ?
???????????????????????????????????????????
               ?
???????????????????????????????????????????
?      CAPA DAL (Acceso a Datos)          ?
?  - DALException                         ?
?  - Manejo automático en SQLHelper      ?
???????????????????????????????????????????
```

---

## ?? Excepciones Personalizadas

### 1. **DALException** (Capa de Datos)

Captura errores de SQL Server y los categoriza.

**Tipos de Error (DALErrorType):**
- `ConnectionFailed`: No se puede conectar al servidor
- `Timeout`: Tiempo de espera agotado
- `Authentication`: Error de autenticación
- `DatabaseNotFound`: Base de datos no encontrada
- `NetworkError`: Error de red
- `ConstraintViolation`: Violación de restricción (unique, FK, not null)
- `Deadlock`: Deadlock detectado
- `PermissionDenied`: Permisos insuficientes
- `Unknown`: Error no categorizado

**Errores Recuperables:**
Los errores de tipo `ConnectionFailed`, `Timeout` y `NetworkError` son considerados recuperables. La aplicación puede continuar funcionando con funcionalidad limitada.

---

### 2. **DatabaseException** (Capa de Negocio)

Representa errores de base de datos desde la perspectiva del negocio.

```csharp
// Ejemplo de uso
try
{
    // Operación de base de datos
}
catch (DAL.DALException dalEx)
{
    throw ExceptionMapper.MapToBusinessException(dalEx, "Error al obtener pedidos");
}
```

**Métodos Factory:**
```csharp
DatabaseException.ConnectionFailed(innerException)
DatabaseException.ConnectionTimeout(innerException)
DatabaseException.AuthenticationFailed(innerException)
DatabaseException.DatabaseNotFound(innerException)
```

---

### 3. **BusinessException** (Base para Negocio)

Excepción base para todas las excepciones de negocio.

```csharp
public class BusinessException : Exception
{
    public string ErrorCode { get; set; }
}
```

---

### 4. **ProductoException**

Errores relacionados con productos.

```csharp
// Ejemplo de uso
if (producto == null || string.IsNullOrEmpty(producto.Nombre))
{
    throw ProductoException.CamposInvalidos();
}
```

**Métodos Factory:**
- `ProductoException.CamposInvalidos()`

---

### 5. **ClienteException**

Errores relacionados con clientes.

```csharp
// Ejemplo de uso
if (ClienteYaExiste(cliente.Cuit))
{
    throw ClienteException.ClienteYaExiste(cliente.Cuit);
}
```

**Métodos Factory:**
- `ClienteException.ClienteYaExiste(cuit)`
- `ClienteException.ClienteNoEncontrado(clienteId)`
- `ClienteException.CamposInvalidos()`

---

### 6. **PedidoException**

Errores relacionados con pedidos.

```csharp
// Ejemplo de uso
if (pedido == null)
{
    throw PedidoException.PedidoNoEncontrado(pedidoId);
}

if (stock < cantidadRequerida)
{
    throw PedidoException.StockInsuficiente(producto.Nombre);
}
```

**Métodos Factory:**
- `PedidoException.PedidoNoEncontrado(pedidoId)`
- `PedidoException.StockInsuficiente(productoNombre)`
- `PedidoException.PedidoNoModificable(estado)`

---

### 7. **StockException**

Errores relacionados con stock.

```csharp
// Ejemplo de uso
if (stockDisponible < cantidadRequerida)
{
    throw StockException.StockInsuficiente(productoId, cantidadRequerida, stockDisponible);
}
```

**Métodos Factory:**
- `StockException.StockInsuficiente(productoId, cantidadRequerida, cantidadDisponible)`
- `StockException.StockBajoMinimo(productoId, stockActual, stockMinimo)`

---

## ??? Uso en la Capa BLL

### Opción 1: Usar ExceptionMapper

```csharp
using BLL.Helpers;
using BLL.Exceptions;

public List<Pedido> ObtenerPedidos()
{
    return ExceptionMapper.ExecuteWithMapping(() =>
    {
        return _pedidoRepository.GetAll();
    }, "Error al obtener la lista de pedidos");
}
```

### Opción 2: Manejo Manual

```csharp
using BLL.Exceptions;
using DAL;

public Pedido ObtenerPedidoPorId(int pedidoId)
{
    try
    {
        var pedido = _pedidoRepository.GetById(pedidoId);
        
        if (pedido == null)
        {
            throw PedidoException.PedidoNoEncontrado(pedidoId);
        }
        
        return pedido;
    }
    catch (DALException dalEx)
    {
        throw ExceptionMapper.MapToBusinessException(dalEx, $"Error al obtener el pedido {pedidoId}");
    }
}
```

### Opción 3: Lógica de Negocio Compleja

```csharp
public void CrearPedido(Pedido pedido, List<DetallePedido> detalles)
{
    try
    {
        // Validaciones de negocio
        if (pedido.ClienteId <= 0)
        {
            throw new BusinessException("El cliente es requerido", "CLIENTE_REQUERIDO");
        }

        // Validar stock
        foreach (var detalle in detalles)
        {
            var stock = _stockRepository.GetByProductoId(detalle.ProductoId);
            if (stock.Cantidad < detalle.Cantidad)
            {
                throw StockException.StockInsuficiente(
                    detalle.ProductoId, 
                    detalle.Cantidad, 
                    stock.Cantidad);
            }
        }

        // Guardar en BD
        _pedidoRepository.Insert(pedido);
    }
    catch (DALException dalEx)
    {
        throw ExceptionMapper.MapToBusinessException(dalEx, "Error al crear el pedido");
    }
}
```

---

## ??? Uso en la Capa UI (Forms)

### Manejo Básico

```csharp
using Service.ManegerEx;
using BLL.Exceptions;

private void btnGuardar_Click(object sender, EventArgs e)
{
    try
    {
        // Operación de negocio
        _pedidoService.CrearPedido(pedido, detalles);
        
        MessageBox.Show("Pedido creado exitosamente", "Éxito", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (DatabaseException dbEx)
    {
        // Errores de base de datos
        string username = SesionService.ObtenerUsuarioActual()?.UserName;
        ErrorHandler.HandleDALException(
            new DAL.DALException(dbEx.Message, DAL.DALErrorType.Unknown, dbEx), 
            username);
    }
    catch (PedidoException pedEx)
    {
        // Errores de negocio de pedidos
        MessageBox.Show(pedEx.Message, "Error de Validación", 
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
    catch (StockException stockEx)
    {
        // Errores de stock
        MessageBox.Show(stockEx.Message, "Stock Insuficiente", 
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
    catch (BusinessException bizEx)
    {
        // Otros errores de negocio
        MessageBox.Show(bizEx.Message, "Error", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    catch (Exception ex)
    {
        // Cualquier otro error
        ErrorHandler.HandleGeneralException(ex);
    }
}
```

### Manejo con DALException directa

```csharp
using Service.ManegerEx;
using DAL;

private void CargarDatos()
{
    try
    {
        var pedidos = _pedidoService.ObtenerTodos();
        dgvPedidos.DataSource = pedidos;
    }
    catch (DALException dalEx)
    {
        string username = SesionService.ObtenerUsuarioActual()?.UserName;
        
        // Si es un error recuperable, permitir que la app continúe
        if (dalEx.IsRecoverable())
        {
            ErrorHandler.HandleDALException(dalEx, username, showMessageBox: true);
            // Cargar datos de caché o mostrar vacío
            dgvPedidos.DataSource = new List<Pedido>();
        }
        else
        {
            // Error crítico
            ErrorHandler.HandleDALException(dalEx, username, showMessageBox: true);
            this.Close();
        }
    }
}
```

---

## ?? Registro en Bitácora

### Registro Automático

Todos los errores manejados por `ErrorHandler` se registran automáticamente en:
1. **Base de datos Bitácora** (si está disponible)
2. **Archivos de log** (fallback si BD no disponible)
   - `C:\Logs\error.log` - Errores
   - `C:\Logs\info.log` - Información

### Registro Manual

```csharp
using Service.Facade;

// Log simple
LoggerService.WriteLog("Operación completada", System.Diagnostics.TraceLevel.Info);

// Log de excepción
try
{
    // Operación
}
catch (Exception ex)
{
    LoggerService.WriteException(ex);
    throw;
}
```

---

## ?? Errores Comunes y Soluciones

### Error: "El equipo remoto rechazó la conexión de red"

**Causa:** SQL Server no está disponible o hay problemas de red.

**Solución:**
1. Verificar que SQL Server esté ejecutándose
2. Verificar configuración de red
3. La aplicación continuará funcionando con funcionalidad limitada

**Código de Ejemplo:**
```csharp
catch (DatabaseException dbEx) when (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
{
    // Mostrar mensaje al usuario y continuar con datos en caché
    MessageBox.Show("No se puede conectar a la base de datos. Trabajando en modo sin conexión.", 
        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
```

---

### Error: "Se agotó el tiempo de espera"

**Causa:** La consulta SQL tardó demasiado.

**Solución:**
1. Optimizar consultas SQL
2. Aumentar timeout en conexión
3. Verificar carga del servidor

---

### Error: "Fallo en la autenticación"

**Causa:** Credenciales de SQL incorrectas.

**Solución:**
1. Verificar `App.config` - connectionStrings
2. Verificar permisos del usuario SQL
3. No es recuperable - requiere intervención

---

## ?? Diagnóstico y Debugging

### Información en Excepciones

Todas las excepciones DAL incluyen:
- `SqlErrorNumber`: Número de error SQL original
- `CommandText`: Comando SQL que causó el error
- `InnerException`: Excepción original de SQL Server

```csharp
catch (DALException dalEx)
{
    Console.WriteLine($"Error SQL #{dalEx.SqlErrorNumber}");
    Console.WriteLine($"Comando: {dalEx.CommandText}");
    Console.WriteLine($"Tipo: {dalEx.ErrorType}");
    Console.WriteLine($"Recuperable: {dalEx.IsRecoverable()}");
}
```

---

## ?? Tabla de Códigos de Error SQL

| Código SQL | Tipo DAL | Recuperable | Descripción |
|------------|----------|-------------|-------------|
| -1, -2 | Timeout | ? Sí | Timeout de conexión |
| 2, 10053, 10054, 10060, 10061 | ConnectionFailed | ? Sí | Error de red/conexión |
| 18456 | Authentication | ? No | Error de autenticación |
| 4060 | DatabaseNotFound | ? No | BD no encontrada |
| 2627 | ConstraintViolation | ?? Depende | Violación de clave única |
| 547 | ConstraintViolation | ?? Depende | Violación de FK |
| 515 | ConstraintViolation | ?? Depende | Valor NULL no permitido |
| 1205 | Deadlock | ? Sí | Deadlock - reintentar |
| 229 | PermissionDenied | ? No | Permisos insuficientes |

---

## ?? Mejores Prácticas

1. **Siempre capturar excepciones específicas primero**
   ```csharp
   try { }
   catch (PedidoException) { }
   catch (DatabaseException) { }
   catch (BusinessException) { }
   catch (Exception) { }
   ```

2. **No ocultar información de error**
   - Siempre registrar en bitácora
   - Incluir contexto relevante
   - Preservar InnerException

3. **Errores recuperables vs críticos**
   - Recuperables: Mostrar warning, continuar
   - Críticos: Mostrar error, cerrar formulario/app

4. **Usar métodos factory**
   ```csharp
   // ? Correcto
   throw ProductoException.CamposInvalidos();
   
   // ? Evitar
   throw new ProductoException("Campos inválidos");
   ```

5. **Traducir mensajes al usuario**
   - Todos los mensajes pasan por `IdiomaService.Translate()`
   - Mantener claves de traducción consistentes

---

## ?? Flujo de Manejo de Errores

```
??????????????
?   DAL      ? ? DALException (ConnectionFailed)
? SQLHelper  ?   ?
??????????????   ? IsRecoverable() = true
                 ?
??????????????   ?
?   BLL      ? ? ExceptionMapper.MapToBusinessException()
?  Service   ?   ? DatabaseException
??????????????   ?
                 ?
??????????????   ?
?    UI      ? ? ErrorHandler.HandleDALException()
?   Form     ?   ? Registra en Bitácora
??????????????   ? Muestra mensaje al usuario
                 ? App continúa funcionando ?
```

---

## ?? Soporte

Para más información o problemas con el manejo de excepciones:
1. Revisar logs en `C:\Logs\`
2. Consultar bitácora en la BD
3. Verificar configuración en `App.config`

---

**Última actualización:** 2024  
**Versión:** 1.0  
**Sistema:** Distribuidora Los Amigos
