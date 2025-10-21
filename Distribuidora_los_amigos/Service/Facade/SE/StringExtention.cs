using Service.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Facade.SE
{
    /// <summary>
    /// Métodos de extensión auxiliares para cadenas de texto.
    /// </summary>
    public static class StringExtention
    {
        /// <summary>
        /// Traduce la cadena interpretándola como clave de recursos.
        /// </summary>
        public static string Translate(this string key)
        {
            return IdiomaLogic.Translate(key);
        }

        /// <summary>
        /// Devuelve la cadena tal cual (espacio reservado para futuras transformaciones).
        /// </summary>
        public static string ToUpperCapital(this string word)
        {
            return word;
        }

        /// <summary>
        /// Ejemplo de extensión que retorna un saludo fijo (sin uso actual).
        /// </summary>
        public static string ExtentionWithParameters(this string word, int a, string pepe)
        {
            return "hola";
        }
    }
}
