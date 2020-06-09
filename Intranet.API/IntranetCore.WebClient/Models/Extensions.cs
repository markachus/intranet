using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IntranetCore.WebClient.Models
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Convierte un objeto a una cadena de texto que representa una query string
        /// con la forma campo1=valor1&campo2=valor2&...campon=valorn donde campo1...ampon son las
        /// propiedades del objeto
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }
    }
}
