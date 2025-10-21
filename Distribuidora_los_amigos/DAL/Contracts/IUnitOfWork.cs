using DAL.Contratcs;
using System;

namespace DAL.Contracts
{
    /// <summary>
    /// Define el patrón Unit of Work para coordinar repositorios y transacciones.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Repositorios
        /// <summary>
        /// Repositorio para operar con entidades de producto.
        /// </summary>
        IProductoRepository ProductoRepository { get; }

        /// <summary>
        /// Repositorio encargado de las operaciones de stock.
        /// </summary>
        IStockRepository StockRepository { get; }

        /// <summary>
        /// Repositorio asociado a la persistencia de proveedores.
        /// </summary>
        IProveedorRepository ProveedorRepository { get; }

        /// <summary>
        /// Repositorio responsable de los pedidos de clientes.
        /// </summary>
        IPedidoRepository PedidoRepository { get; }

        /// <summary>
        /// Repositorio para el mantenimiento de clientes.
        /// </summary>
        IClienteRepository ClienteRepository { get; }

        /// <summary>
        /// Repositorio que administra los detalles de pedido.
        /// </summary>
        IDetallePedidoRepository DetallePedidoRepository { get; }

        /// <summary>
        /// Repositorio que expone el catálogo de estados de pedido.
        /// </summary>
        IEstadoPedidoRepository EstadoPedidoRepository { get; }

        // Métodos de transacción
        /// <summary>
        /// Inicia una transacción explícita sobre la conexión subyacente.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Confirma los cambios realizados dentro de la transacción activa.
        /// </summary>
        void Commit();

        /// <summary>
        /// Revierte los cambios realizados durante la transacción actual.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Persiste los cambios cuando no existe una transacción manual.
        /// </summary>
        void SaveChanges();
    }
}
