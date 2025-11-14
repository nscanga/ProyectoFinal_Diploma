using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contratcs;
using DAL.Factory;
using DOMAIN;
using Service.Facade;
using BLL.Exceptions;
using BLL.Helpers;

namespace BLL
{
    public class PedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IDetallePedidoRepository _detallePedidoRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IEstadoPedidoRepository _estadoPedidoRepository;
        private readonly StockService _stockService; // 🆕 Agregar StockService

        /// <summary>
        /// Inicializa el servicio configurando los repositorios y servicios necesarios para gestionar pedidos.
        /// </summary>
        public PedidoService()
        {
            _pedidoRepository = FactoryDAL.SqlPedidoRepository;
            _clienteRepository = FactoryDAL.SqlClienteRepository;
            _detallePedidoRepository = FactoryDAL.SqlDetallePedidoRepository;
            _stockRepository = FactoryDAL.SqlStockRepository;
            _estadoPedidoRepository = FactoryDAL.SqlEstadoPedidoRepository;
            _stockService = new StockService(); // 🆕 Inicializar StockService
        }

        /// <summary>
        /// Crea un nuevo pedido y lo guarda en la base de datos.
        /// </summary>
        /// <param name="pedido">Pedido a crear.</param>
        public void CrearPedido(Pedido pedido, int cantidad)
        {
            ValidarPedido(pedido);

            // Calcular el total sumando los subtotales de los detalles
            pedido.Total = pedido.Detalles.Sum(d => d.Subtotal);

            // Primero, insertar el pedido en la base de datos
            _pedidoRepository.Add(pedido);

            // Luego, insertar los detalles del pedido con el mismo IdPedido
            foreach (var detalle in pedido.Detalles)
            {
                detalle.IdPedido = pedido.IdPedido; // Asegurar que todos los detalles tengan el mismo IdPedido
                _detallePedidoRepository.Add(detalle);
                
                // 🆕 USAR StockService en lugar de _stockRepository
                // Esto activará la verificación y notificación de stock bajo
                _stockService.DisminuirStock(detalle.IdProducto, detalle.Cantidad);
            }
        }

        /// <summary>
        /// Modifica un pedido existente y verifica cambios de estado para notificaciones
        /// </summary>
        /// <param name="pedido">Pedido modificado.</param>
        public void ModificarPedido(Pedido pedido)
        {
            Console.WriteLine($"📌 Modificando pedido {pedido.IdPedido} con {pedido.Detalles.Count} detalles.");

            // 🆕 VERIFICAR CAMBIO DE ESTADO ANTES DE ACTUALIZAR
            Pedido pedidoAnterior = _pedidoRepository.GetById(pedido.IdPedido);
            bool cambioAEnCamino = false;
            
            if (pedidoAnterior != null && pedidoAnterior.IdEstadoPedido != pedido.IdEstadoPedido)
            {
                string estadoAnterior = ObtenerNombreEstadoPorId(pedidoAnterior.IdEstadoPedido);
                string nuevoEstado = ObtenerNombreEstadoPorId(pedido.IdEstadoPedido);
                
                // 🎯 Detectar cambio específico a "En camino"
                cambioAEnCamino = nuevoEstado.Equals("En camino", StringComparison.OrdinalIgnoreCase);
                
                Console.WriteLine($"🔄 Cambio de estado detectado: {estadoAnterior} → {nuevoEstado}");
                
                if (cambioAEnCamino)
                {
                    Console.WriteLine($"📧 Preparando envío de email para pedido {pedido.IdPedido}");
                }
            }

            // 📌 Recalcular el total del pedido sumando los subtotales de sus detalles
            pedido.Total = pedido.Detalles.Sum(d => d.Subtotal);

            // 📌 Primero actualizamos el pedido con su nuevo total
            _pedidoRepository.Update(pedido);

            // 📌 Obtener los detalles actuales del pedido en la base de datos
            List<DetallePedido> detallesActuales = _detallePedidoRepository.GetByPedido(pedido.IdPedido);

            foreach (var detalle in pedido.Detalles)
            {
                var detalleExistente = detallesActuales.FirstOrDefault(d => d.IdDetallePedido == detalle.IdDetallePedido);

                if (detalleExistente != null)
                {
                    Console.WriteLine($"🔄 Actualizando detalle {detalle.IdDetallePedido} - Producto: {detalle.IdProducto}, Cantidad: {detalle.Cantidad}");
                    _detallePedidoRepository.Update(detalle);
                }
                else
                {
                    Console.WriteLine($"🆕 Agregando nuevo detalle {detalle.IdDetallePedido} - Producto: {detalle.IdProducto}, Cantidad: {detalle.Cantidad}");
                    _detallePedidoRepository.Add(detalle);
                }
            }

            // 🆕 ENVIAR EMAIL SI CAMBIÓ A "EN CAMINO"
            if (cambioAEnCamino)
            {
                try
                {
                    // Obtener datos del cliente
                    Cliente cliente = _clienteRepository.GetById(pedido.IdCliente);
                    
                    Console.WriteLine($"🔍 DEBUG - Datos del cliente obtenidos:");
                    Console.WriteLine($"   Cliente encontrado: {cliente != null}");
                    
                    if (cliente != null)
                    {
                        Console.WriteLine($"   ID: {cliente.IdCliente}");
                        Console.WriteLine($"   Nombre: {cliente.Nombre}");
                        Console.WriteLine($"   Email: '{cliente.Email}'");
                        Console.WriteLine($"   Email válido: {!string.IsNullOrEmpty(cliente.Email)}");
                        
                        if (!string.IsNullOrEmpty(cliente.Email))
                        {
                            // 🎯 Usar el EmailService existente
                            EmailService.EnviarNotificacionPedidoEnCamino(pedido, cliente);
                            Console.WriteLine($"✅ Email de notificación enviado correctamente a {cliente.Email}");
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ Cliente sin email: ID={cliente.IdCliente}, Nombre={cliente.Nombre}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"❌ Cliente no encontrado para ID: {pedido.IdCliente}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error completo al enviar email:");
                    Console.WriteLine($"   Mensaje: {ex.Message}");
                    Console.WriteLine($"   Tipo: {ex.GetType().Name}");
                    Console.WriteLine($"   Stack: {ex.StackTrace}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"   Inner: {ex.InnerException.Message}");
                    }
                    // No lanzamos la excepción para que no afecte la actualización del pedido
                }
            }
        }

        /// <summary>
        /// Elimina un pedido por su ID.
        /// </summary>
        /// <param name="idPedido">ID del pedido.</param>
        public void EliminarPedido(Guid idPedido)
        {
            // Primero obtener los detalles del pedido para restaurar el stock
            List<DetallePedido> detalles = _detallePedidoRepository.GetByPedido(idPedido);
            
            // Restaurar el stock de cada producto
            foreach (var detalle in detalles)
            {
                // 🆕 USAR StockService para restaurar el stock
                // Esto también verificará si después de aumentar sigue bajo
                _stockService.AumentarStock(detalle.IdProducto, detalle.Cantidad);
                _detallePedidoRepository.Remove(detalle.IdDetallePedido);
            }
            
            // Finalmente eliminar el pedido
            _pedidoRepository.Remove(idPedido);
        }

        /// <summary>
        /// Obtiene un pedido por su ID.
        /// </summary>
        /// <param name="idPedido">ID del pedido.</param>
        /// <returns>Pedido encontrado.</returns>
        public Pedido ObtenerPedidoPorId(Guid idPedido)
        {
            Pedido pedido = _pedidoRepository.GetById(idPedido);

            if (pedido != null)
            {
                // Asegurar que se traen los detalles del pedido
                pedido.Detalles = _detallePedidoRepository.GetByPedido(idPedido);
                Console.WriteLine($"📌 Pedido {idPedido} cargado con {pedido.Detalles.Count} detalles.");
            }
            else
            {
                Console.WriteLine($"⚠ Pedido {idPedido} no encontrado.");
            }

            return pedido;
        }

        /// <summary>
        /// Recupera el nombre descriptivo del estado de pedido indicado.
        /// </summary>
        /// <param name="idEstadoPedido">Identificador del estado.</param>
        /// <returns>Nombre del estado o mensaje por defecto si no existe.</returns>
        public string ObtenerNombreEstadoPorId(Guid idEstadoPedido)
        {
            EstadoPedido estado = _estadoPedidoRepository.GetById(idEstadoPedido);
            return estado != null ? estado.NombreEstado : "Estado no encontrado";
        }

        /// <summary>
        /// Obtiene y registra en consola los detalles asociados al pedido.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido solicitado.</param>
        /// <returns>Listado de detalles recuperados.</returns>
        public List<DetallePedido> ObtenerDetallesPorPedido(Guid idPedido)
        {
            var detalles = _detallePedidoRepository.GetByPedido(idPedido);
            Console.WriteLine($"🔍 Buscando detalles para el pedido {idPedido}: {detalles.Count} encontrados.");
            return detalles;
        }

        /// <summary>
        /// Obtiene todos los pedidos de la base de datos.
        /// Si hay error de conexión, devuelve lista vacía para que la aplicación continúe funcionando.
        /// </summary>
        /// <returns>Lista de pedidos.</returns>
        public List<Pedido> ObtenerTodosLosPedidos()
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _pedidoRepository.GetAll();
                }, "Error al obtener todos los pedidos");
            }
            catch (DatabaseException dbEx)
            {
                // Si es error de conexión o timeout, devolver lista vacía
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener pedidos. Devolviendo lista vacía.");
                    // Propagar la excepción para que la UI pueda manejarla apropiadamente
                    throw;
                }
                // Si es otro error crítico, propagar
                throw;
            }
        }

        /// <summary>
        /// Obtiene los pedidos con estado "Pendiente".
        /// </summary>
        /// <returns>Lista de pedidos pendientes.</returns>
        public List<Pedido> ObtenerPedidosPendientes()
        {
            return _pedidoRepository.GetPedidosPendientes();
        }

        /// <summary>
        /// Devuelve el nombre del cliente solicitado o un mensaje de error si no existe.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente buscado.</param>
        /// <returns>Nombre del cliente o mensaje alternativo.</returns>
        public string ObtenerNombreClientePorId(Guid idCliente)
        {
            Cliente cliente = _clienteRepository.GetById(idCliente);
            return cliente != null ? cliente.Nombre : "Cliente no encontrado";
        }

        /// <summary>
        /// Recupera el catálogo de estados disponibles para los pedidos.
        /// Si hay error de conexión, devuelve estados por defecto para que la aplicación continúe funcionando.
        /// </summary>
        /// <returns>Listado de estados.</returns>
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
                // Si es error de conexión o timeout, devolver estados por defecto
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener estados. Usando estados por defecto.");
                    return ObtenerEstadosPorDefecto();
                }
                // Si es otro error crítico, propagar
                throw;
            }
        }

        /// <summary>
        /// Devuelve estados de pedido por defecto cuando la base de datos no está disponible.
        /// Permite que la aplicación continúe funcionando con funcionalidad limitada.
        /// </summary>
        private List<EstadoPedido> ObtenerEstadosPorDefecto()
        {
            return new List<EstadoPedido>
            {
                new EstadoPedido 
                { 
                    IdEstadoPedido = Guid.Parse("00000000-0000-0000-0000-000000000001"), 
                    NombreEstado = "Pendiente" 
                },
                new EstadoPedido 
                { 
                    IdEstadoPedido = Guid.Parse("00000000-0000-0000-0000-000000000002"), 
                    NombreEstado = "En Proceso" 
                },
                new EstadoPedido 
                { 
                    IdEstadoPedido = Guid.Parse("00000000-0000-0000-0000-000000000003"), 
                    NombreEstado = "En Camino" 
                },
                new EstadoPedido 
                { 
                    IdEstadoPedido = Guid.Parse("00000000-0000-0000-0000-000000000004"), 
                    NombreEstado = "Entregado" 
                },
                new EstadoPedido 
                { 
                    IdEstadoPedido = Guid.Parse("00000000-0000-0000-0000-000000000005"), 
                    NombreEstado = "Cancelado" 
                }
            };
        }

        /// <summary>
        /// Valida que un pedido tenga datos correctos.
        /// </summary>
        /// <param name="pedido">Pedido a validar.</param>
        private void ValidarPedido(Pedido pedido)
        {
            if (pedido.IdCliente == Guid.Empty)
                throw new ArgumentException("El pedido debe estar asociado a un cliente.");

            if (pedido.IdEstadoPedido == Guid.Empty)
                throw new ArgumentException("El pedido debe tener un estado válido.");

            if (pedido.FechaPedido > DateTime.Now)
                throw new ArgumentException("La fecha del pedido no puede ser en el futuro.");
        }

        /// <summary>
        /// Modifica únicamente el estado de un pedido sin afectar productos ni total
        /// </summary>
        /// <param name="idPedido">ID del pedido</param>
        /// <param name="nuevoEstadoId">ID del nuevo estado</param>
        public void CambiarEstadoPedido(Guid idPedido, Guid nuevoEstadoId)
        {
            // Obtener el pedido actual
            Pedido pedidoAnterior = _pedidoRepository.GetById(idPedido);
            if (pedidoAnterior == null)
                throw new ArgumentException("Pedido no encontrado.");

            // Verificar cambio de estado
            bool cambioAEnCamino = false;
            if (pedidoAnterior.IdEstadoPedido != nuevoEstadoId)
            {
                string estadoAnterior = ObtenerNombreEstadoPorId(pedidoAnterior.IdEstadoPedido);
                string nuevoEstado = ObtenerNombreEstadoPorId(nuevoEstadoId);
                
                cambioAEnCamino = nuevoEstado.Equals("En camino", StringComparison.OrdinalIgnoreCase);
                Console.WriteLine($"🔄 Cambio de estado: {estadoAnterior} → {nuevoEstado}");
            }

            // Actualizar solo el estado del pedido (sin modificar total ni detalles)
            _pedidoRepository.UpdateEstado(idPedido, nuevoEstadoId);

            // Enviar email si cambió a "En camino"
            if (cambioAEnCamino)
            {
                try
                {
                    Cliente cliente = _clienteRepository.GetById(pedidoAnterior.IdCliente);
                    if (cliente != null && !string.IsNullOrEmpty(cliente.Email))
                    {
                        // Crear objeto pedido con el nuevo estado para el email
                        pedidoAnterior.IdEstadoPedido = nuevoEstadoId;
                        EmailService.EnviarNotificacionPedidoEnCamino(pedidoAnterior, cliente);
                        Console.WriteLine($"✅ Email enviado a {cliente.Email}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error al enviar email: {ex.Message}");
                }
            }
        }
    }
}
