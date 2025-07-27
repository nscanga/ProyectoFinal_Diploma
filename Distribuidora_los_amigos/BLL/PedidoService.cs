using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contratcs;
using DAL.Factory;
using DOMAIN;

namespace BLL
{
    public class PedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IDetallePedidoRepository _detallePedidoRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IEstadoPedidoRepository _estadoPedidoRepository; 


        public PedidoService()
        {
            _pedidoRepository = FactoryDAL.SqlPedidoRepository;
            _clienteRepository = FactoryDAL.SqlClienteRepository;
            _detallePedidoRepository = FactoryDAL.SqlDetallePedidoRepository;
            _stockRepository = FactoryDAL.SqlStockRepository;
            _estadoPedidoRepository = FactoryDAL.SqlEstadoPedidoRepository; 

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
                _stockRepository.DescontarStock(detalle.IdProducto, detalle.Cantidad);

            }
        }


        /// <summary>
        /// Modifica un pedido existente.
        /// </summary>
        /// <param name="pedido">Pedido modificado.</param>
        public void ModificarPedido(Pedido pedido)
        {
            Console.WriteLine($"📌 Modificando pedido {pedido.IdPedido} con {pedido.Detalles.Count} detalles.");

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
        }




        /// <summary>
        /// Elimina un pedido por su ID.
        /// </summary>
        /// <param name="idPedido">ID del pedido.</param>
        public void EliminarPedido(Guid idPedido)
        {
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

        public string ObtenerNombreEstadoPorId(Guid idEstadoPedido)
        {
            EstadoPedido estado = _estadoPedidoRepository.GetById(idEstadoPedido);
            return estado != null ? estado.NombreEstado : "Estado no encontrado";
        }


        public List<DetallePedido> ObtenerDetallesPorPedido(Guid idPedido)
        {
            var detalles = _detallePedidoRepository.GetByPedido(idPedido);
            Console.WriteLine($"🔍 Buscando detalles para el pedido {idPedido}: {detalles.Count} encontrados.");
            return detalles;
        }



        /// <summary>
        /// Obtiene todos los pedidos de la base de datos.
        /// </summary>
        /// <returns>Lista de pedidos.</returns>
        public List<Pedido> ObtenerTodosLosPedidos()
        {
            return _pedidoRepository.GetAll();
        }

        /// <summary>
        /// Obtiene los pedidos con estado "Pendiente".
        /// </summary>
        /// <returns>Lista de pedidos pendientes.</returns>
        public List<Pedido> ObtenerPedidosPendientes()
        {
            return _pedidoRepository.GetPedidosPendientes();
        }

        public string ObtenerNombreClientePorId(Guid idCliente)
        {
            Cliente cliente = _clienteRepository.GetById(idCliente);
            return cliente != null ? cliente.Nombre : "Cliente no encontrado";
        }


        public List<EstadoPedido> ObtenerEstadosPedido()
        {
            return _pedidoRepository.ObtenerEstadosPedido();
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

    }
}
