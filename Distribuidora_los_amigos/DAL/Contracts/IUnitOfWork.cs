using DAL.Contratcs;
using System;

namespace DAL.Contracts
{
    /// <summary>
    /// Patrón Unit of Work para gestionar transacciones
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Repositorios
        IProductoRepository ProductoRepository { get; }
        IStockRepository StockRepository { get; }
        IProveedorRepository ProveedorRepository { get; }
        IPedidoRepository PedidoRepository { get; }
        IClienteRepository ClienteRepository { get; }
        IDetallePedidoRepository DetallePedidoRepository { get; }
        IEstadoPedidoRepository EstadoPedidoRepository { get; }

        // Métodos de transacción
        void BeginTransaction();
        void Commit();
        void Rollback();
        void SaveChanges();
    }
}