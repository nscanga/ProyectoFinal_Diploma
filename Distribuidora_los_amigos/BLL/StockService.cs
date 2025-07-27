using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contracts;
using DAL.Contratcs;
using DAL.Factory;
using DOMAIN;

namespace BLL
{
    public class StockService
    {
        private readonly IStockRepository _stockRepository;

        public StockService()
        {
            _stockRepository = FactoryDAL.SqlStockRepository;
        }

        public void AgregarStock(Stock stock)
        {
            _stockRepository.Add(stock);
        }

        /// <summary>
        /// Modifica la cantidad del stock asociado a un producto.
        /// </summary>
        public void ModificarStock(Guid idProducto, int cantidad)
        {
            Stock stock = ObtenerStockPorProducto(idProducto);
            if (stock != null)
            {
                stock.Cantidad += cantidad;
                _stockRepository.Update(stock);
            }
        }


        public void EliminarStock(Guid idStock)
        {
            _stockRepository.Remove(idStock);
        }

        /// <summary>
        /// Aumenta la cantidad de stock disponible para un producto.
        /// </summary>
        public void AumentarStock(Guid idProducto, int cantidad)
        {
            Stock stock = _stockRepository.GetByProducto(idProducto).FirstOrDefault();
            if (stock != null)
            {
                stock.Cantidad += cantidad;
                _stockRepository.Update(stock);
            }
        }

        /// <summary>
        /// Disminuye la cantidad de stock disponible para un producto.
        /// </summary>
        public void DisminuirStock(Guid idProducto, int cantidad)
        {
            Stock stock = _stockRepository.GetByProducto(idProducto).FirstOrDefault();
            if (stock != null && stock.Cantidad >= cantidad)
            {
                stock.Cantidad -= cantidad;
                _stockRepository.Update(stock);
            }
        }

        public List<Stock> ObtenerStock()
        {
            return _stockRepository.GetAll();
        }

        /// <summary>
        /// Devuelve el stock asociado a un producto (toma el primer resultado encontrado).
        /// </summary>
        public Stock ObtenerStockPorProducto(Guid idProducto)
        {
            return _stockRepository.GetByProducto(idProducto).FirstOrDefault();
        }


    }
}
