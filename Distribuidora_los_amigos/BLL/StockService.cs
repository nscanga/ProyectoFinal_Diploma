using System;
using System.Collections.Generic;
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

        public void ModificarStock(Stock stock)
        {
            _stockRepository.Update(stock);
        }

        public void EliminarStock(Guid idStock)
        {
            _stockRepository.Remove(idStock);
        }

        public List<Stock> ObtenerStock()
        {
            return _stockRepository.GetAll();
        }

        public List<Stock> ObtenerStockPorProducto(Guid idProducto)
        {
            return _stockRepository.GetByProducto(idProducto);
        }
    }
}
