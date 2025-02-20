using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class ProductoException : Exception
    {
        public ProductoException(string message) : base(message) { }

        public static ProductoException CamposInvalidos()
        {
            return new ProductoException("Uno o más campos del producto son inválidos.");
        }
    }
}
