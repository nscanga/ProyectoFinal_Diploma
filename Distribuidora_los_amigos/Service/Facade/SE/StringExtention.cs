using Service.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Facade.SE
{
    public static class StringExtention
    {
        public static string Translate(this string key)
        {
            return IdiomaLogic.Translate(key);
        }

        public static string ToUpperCapital(this string word)
        {
            return word;
        }

        public static string ExtentionWithParameters(this string word, int a, string pepe)
        {
            return "hola";
        }
    }
}
