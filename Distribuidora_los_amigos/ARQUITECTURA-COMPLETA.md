# ??? Arquitectura del Sistema - Distribuidora Los Amigos

## ?? Resumen Ejecutivo

Sistema de gestión empresarial desarrollado con .NET Framework 4.7.2, implementando arquitectura en capas con múltiples patrones de diseño para garantizar mantenibilidad, escalabilidad y robustez.

---

## ?? Arquitectura Principal: Layered Architecture (Capas)

### Estructura de Capas

```
???????????????????????????????????????????????????????
?                   UI Layer                          ?
?         (Presentación - Windows Forms)              ?
???????????????????????????????????????????????????????
                        ??
???????????????????????????????????????????????????????
?                   BLL Layer                         ?
?          (Lógica de Negocio - Services)             ?
???????????????????????????????????????????????????????
                        ??
???????????????????????????????????????????????????????
?                   DAL Layer                         ?
?         (Acceso a Datos - Repositories)             ?
???????????????????????????????????????????????????????
                        ??
???????????????????????????????????????????????????????
?                DOMAIN Layer                         ?
?           (Entidades y DTOs)                        ?
???????????????????????????????????????????????????????

        ???????????????????????????????
        ?     Service Layer           ?
        ?  (Transversal - Cross)      ?
        ???????????????????????????????
```

### Descripción por Capa

#### 1. **UI Layer (Presentación)**
- **Tecnología:** Windows Forms (.NET Framework 4.7.2)
- **Responsabilidad:** Interacción con el usuario
- **Componentes:**
  - Forms (CrearClienteForm, ModificarProductoForm, etc.)
  - UserControls personalizados
  - Validación de entrada de usuario
  - Binding de datos
  - Manejo de eventos

#### 2. **BLL Layer (Business Logic Layer)**
- **Responsabilidad:** Lógica de negocio y reglas
- **Componentes:**
  - Services (ClienteService, ProductoService, PedidoService, etc.)
  - Commands (CrearProductoCommand, ModificarClienteCommand, etc.)
  - Validators
  - Exceptions personalizadas
  - Helpers

#### 3. **DAL Layer (Data Access Layer)**
- **Responsabilidad:** Acceso y persistencia de datos
- **Componentes:**
  - Repositories (SqlClienteRepository, SqlProductoRepository, etc.)
  - Unit of Work (SqlUnitOfWork)
  - Factory (FactoryDAL)
  - SQLHelper

#### 4. **DOMAIN Layer**
- **Responsabilidad:** Modelos del dominio
- **Componentes:**
  - Entities (Cliente, Producto, Pedido, etc.)
  - DTOs (Data Transfer Objects)
  - Value Objects
  - Enums

#### 5. **Service Layer (Transversal)**
- **Responsabilidad:** Servicios compartidos
- **Componentes:**
  - Logger
  - Security (SessionManager, Encryption)
  - BackupService
  - RecuperoPassService
  - ErrorHandler

---

## ?? Patrones de Diseño Implementados

### 1. **Repository Pattern** ??

**Propósito:** Abstrae el acceso a datos y centraliza la lógica de consultas.

**Ubicación:** `DAL\Implementations\SqlServer\`

**Implementación:**
```csharp
public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

public class SqlClienteRepository : IRepository<Cliente>
{
    // Implementación específica para SQL Server
}
```

**Beneficios:**
- ? Desacoplamiento entre lógica de negocio y persistencia
- ? Facilita pruebas unitarias (mocking)
- ? Centraliza lógica de acceso a datos
- ? Permite cambiar el origen de datos fácilmente

---

### 2. **Unit of Work Pattern** ??

**Propósito:** Gestiona transacciones y mantiene un contexto único de trabajo.

**Ubicación:** `DAL\Implementations\SqlServer\SqlUnitOfWork.cs`

**Implementación:**
```csharp
public interface IUnitOfWork : IDisposable
{
    void BeginTransaction();
    void Commit();
    void Rollback();
    IClienteRepository Clientes { get; }
    IProductoRepository Productos { get; }
    // ... otros repositorios
}
```

**Beneficios:**
- ? Consistencia transaccional
- ? Gestión centralizada de conexiones
- ? Rollback automático en caso de error
- ? Optimización de operaciones múltiples

---

### 3. **Command Pattern** ?

**Propósito:** Encapsula operaciones como objetos independientes.

**Ubicación:** `BLL\Commands\`

**Implementación:**
```csharp
public interface ICommand
{
    void Execute();
    void Undo();
}

public class CrearProductoCommand : ICommand
{
    private readonly Producto _producto;
    
    public void Execute()
    {
        // Lógica de creación
    }
    
    public void Undo()
    {
        // Lógica de reversión
    }
}

public class CommandInvoker
{
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        // Logging, historial, etc.
    }
}
```

**Beneficios:**
- ? Deshacer/Rehacer operaciones
- ? Logging automático de operaciones
- ? Cola de comandos
- ? Validación centralizada

---

### 4. **Factory Pattern** ??

**Propósito:** Centraliza la creación de objetos.

**Ubicación:** `DAL\Factory\FactoryDAL.cs`, `Service\DAL\FactoryServices\`

**Implementación:**
```csharp
public class FactoryDAL
{
    public static IClienteRepository CrearClienteRepository()
    {
        return new SqlClienteRepository();
    }
    
    public static IUnitOfWork CrearUnitOfWork()
    {
        return new SqlUnitOfWork();
    }
}
```

**Beneficios:**
- ? Flexibilidad para cambiar implementaciones
- ? Configuración centralizada
- ? Inyección de dependencias manual
- ? Testing simplificado

---

### 5. **Strategy Pattern** ??

**Propósito:** Define familia de algoritmos intercambiables.

**Ubicación:** `BLL\Validators\`

**Implementación:**
```csharp
public interface IValidator<T>
{
    ValidationResult Validate(T entity);
}

public class ClienteValidator : IValidator<Cliente>
{
    public ValidationResult Validate(Cliente cliente)
    {
        // Validaciones específicas
    }
}
```

**Beneficios:**
- ? Validaciones reutilizables
- ? Extensible sin modificar código existente
- ? Separación de responsabilidades

---

### 6. **Facade Pattern** ??

**Propósito:** Simplifica interfaces complejas.

**Ubicación:** `Service\Facade\BackUpService.cs`, `RecuperoPassService.cs`

**Implementación:**
```csharp
public class BackUpService
{
    public void RealizarBackupCompleto()
    {
        // Orquesta múltiples operaciones
        ValidarPermisos();
        CrearDirectorio();
        EjecutarBackup();
        ComprimirArchivo();
        NotificarResultado();
    }
}
```

**Beneficios:**
- ? API simplificada
- ? Oculta complejidad interna
- ? Punto único de entrada

---

### 7. **Singleton Pattern** ??

**Propósito:** Garantiza una única instancia global.

**Ubicación:** `Service\Logger\`, `Service\Security\SessionManager`

**Implementación:**
```csharp
public class Logger
{
    private static Logger _instance;
    private static readonly object _lock = new object();
    
    public static Logger Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Logger();
                    }
                }
            }
            return _instance;
        }
    }
}
```

**Beneficios:**
- ? Control centralizado
- ? Recursos compartidos
- ? Thread-safe

---

## ?? Sistema de Manejo de Excepciones

### Jerarquía de Excepciones Personalizadas

```
Exception
    ??? BusinessException (base)
        ??? ClienteException
        ??? ProductoException
        ??? PedidoException
        ??? StockException
        ??? ProveedorException
        ??? DatabaseException
    
    ??? DALException
        ??? ConnectionException
```

### Componentes Clave

#### 1. **ErrorHandler** (Service\ManegerEx\ErrorHandler.cs)
```csharp
public class ErrorHandler
{
    public void Handle(Exception ex)
    {
        Logger.Instance.LogError(ex);
        
        if (ex is BusinessException businessEx)
        {
            MostrarMensajeUsuario(businessEx.Message);
        }
        else if (ex is DALException dalEx)
        {
            MostrarMensajeTecnico(dalEx);
        }
        else
        {
            MostrarMensajeGenerico();
        }
    }
}
```

#### 2. **ExceptionMapper** (BLL\Helpers\ExceptionMapper.cs)
- Traduce excepciones técnicas a mensajes de usuario
- Mapea códigos de error SQL a excepciones personalizadas
- Proporciona contexto adicional

### Estrategia de Manejo
1. **Captura en capa apropiada**
2. **Logging automático**
3. **Transformación a mensaje usuario-friendly**
4. **Rollback de transacciones si es necesario**
5. **Notificación al usuario**

---

## ?? Seguridad

### Componentes de Seguridad

#### 1. **SessionManager**
- Gestión de sesión de usuario
- Control de timeout
- Información de usuario actual

#### 2. **Encryption Service**
- Encriptación de contraseñas (SHA-256)
- Hashing seguro
- Validación de contraseñas

#### 3. **RecuperoPassService**
- Recuperación de contraseña
- Validación de usuario
- Generación de código temporal

#### 4. **Control de Permisos**
- Roles de usuario
- Permisos granulares por función
- Validación en cada operación

---

## ?? Backup y Restore

### BackupService

**Funcionalidades:**
- ? Backup automático programado
- ? Backup manual on-demand
- ? Restore de base de datos
- ? Configuración flexible (ruta, frecuencia)
- ? Historial de backups
- ? Compresión de archivos
- ? Validación de integridad

**Ubicación:** `Service\Facade\BackUpService.cs`

---

## ?? Logging

### Sistema de Logs

**Características:**
- ? Niveles: Info, Warning, Error, Debug
- ? Logging a archivo
- ? Rotación automática de logs
- ? Contexto de usuario y timestamp
- ? Stack trace en errores
- ? Filtrado por nivel

**Ubicación:** `Service\Logger\`

---

## ? Sistema de Validaciones

### Validación en Múltiples Capas

#### 1. **UI Layer**
- Validación de formato de entrada
- Campos requeridos
- Rangos de valores

#### 2. **BLL Layer**
- Reglas de negocio
- Validación de estado
- Consistencia de datos

#### 3. **DAL Layer**
- Constraints de base de datos
- Integridad referencial

### Validators

```csharp
public class ClienteValidator
{
    public ValidationResult ValidarCreacion(Cliente cliente)
    {
        var result = new ValidationResult();
        
        if (string.IsNullOrEmpty(cliente.Nombre))
            result.AddError("Nombre es requerido");
            
        if (!EsDNIValido(cliente.DNI))
            result.AddError("DNI inválido");
            
        return result;
    }
}
```

---

## ?? Flujo de Datos Típico

### Ejemplo: Crear un Producto

```
1. UI: Usuario completa formulario CrearProductoForm
   ?
2. UI: Validación de campos requeridos
   ?
3. BLL: ProductoService.CrearProducto(producto)
   ?
4. BLL: ProductoValidator.Validate(producto)
   ?
5. BLL: CrearProductoCommand.Execute()
   ?
6. DAL: UnitOfWork.BeginTransaction()
   ?
7. DAL: ProductoRepository.Add(producto)
   ?
8. DAL: UnitOfWork.Commit()
   ?
9. Logger: Log operación exitosa
   ?
10. UI: Mostrar mensaje de éxito
```

### Manejo de Errores en el Flujo

```
Error en cualquier paso
   ?
DAL: UnitOfWork.Rollback()
   ?
Logger: Log error con stack trace
   ?
ExceptionMapper: Traducir a mensaje usuario
   ?
UI: Mostrar mensaje descriptivo
```

---

## ?? Stack Tecnológico

### Framework y Lenguajes
- **.NET Framework:** 4.7.2
- **Lenguaje:** C# 7.3
- **UI:** Windows Forms
- **IDE:** Visual Studio 2022

### Base de Datos
- **Motor:** SQL Server (2014+)
- **Acceso:** ADO.NET
- **Características:**
  - Stored Procedures
  - Triggers
  - Constraints
  - Indexes

### Librerías y Referencias
- `System.Data.SqlClient` - Acceso a SQL Server
- `System.Configuration` - Configuración
- `System.Security.Cryptography` - Encriptación
- `System.IO` - Manejo de archivos

### Herramientas de Desarrollo
- Visual Studio 2022
- SQL Server Management Studio (SSMS)
- Git / GitHub
- Sandcastle (Documentación)

---

## ?? Métricas del Sistema

### Capas
- **UI:** ~50 formularios
- **BLL:** 8 servicios principales
- **DAL:** 8 repositorios
- **DOMAIN:** 15+ entidades
- **Service:** 5 servicios transversales

### Patrones
- 7 patrones de diseño implementados
- 8 excepciones personalizadas
- 10+ validadores

### Funcionalidades
- Gestión de clientes, productos, proveedores
- Control de stock e inventario
- Gestión de pedidos
- Backup/Restore automático
- Sistema de permisos
- Multi-idioma
- Logging completo

---

## ?? Principios de Diseño Aplicados

### SOLID

#### 1. **Single Responsibility Principle (SRP)**
- Cada clase tiene una única responsabilidad
- Services separados por entidad
- Validators independientes

#### 2. **Open/Closed Principle (OCP)**
- Abierto para extensión (Strategy, Command)
- Cerrado para modificación (interfaces)

#### 3. **Liskov Substitution Principle (LSP)**
- Implementaciones intercambiables de IRepository
- Polimorfismo en Commands

#### 4. **Interface Segregation Principle (ISP)**
- Interfaces específicas y pequeñas
- IRepository<T>, IValidator<T>

#### 5. **Dependency Inversion Principle (DIP)**
- Dependencia de abstracciones (interfaces)
- Factory para inyección de dependencias

### DRY (Don't Repeat Yourself)
- Reutilización de código en Helpers
- Base classes para lógica común
- Validators reutilizables

### KISS (Keep It Simple, Stupid)
- Código claro y legible
- Métodos pequeños y enfocados
- Nombres descriptivos

---

## ?? Ventajas de la Arquitectura

### Mantenibilidad
- ? Código organizado y estructurado
- ? Fácil localización de bugs
- ? Documentación clara

### Escalabilidad
- ? Fácil agregar nuevas funcionalidades
- ? Patrones extensibles
- ? Desacoplamiento de componentes

### Testabilidad
- ? Interfaces mockeable
- ? Lógica de negocio aislada
- ? Unit tests simplificados

### Robustez
- ? Manejo de errores completo
- ? Transacciones garantizadas
- ? Logging exhaustivo

### Seguridad
- ? Validación en múltiples capas
- ? Encriptación de datos sensibles
- ? Control de acceso granular

---

## ?? Documentación Adicional

- **Manejo de Excepciones:** `Docs\MANEJO_EXCEPCIONES.md`
- **Resiliencia:** `Docs\RESUMEN_RESILIENCIA.md`
- **Validaciones:** `Docs\REFACTORIZACION_VALIDACIONES_COMPLETADA.md`
- **API Reference:** `Help\index.html`

---

**Arquitectura diseñada y documentada para Distribuidora Los Amigos**  
**Versión:** 1.0  
**Framework:** .NET Framework 4.7.2  
**Fecha:** 2024
