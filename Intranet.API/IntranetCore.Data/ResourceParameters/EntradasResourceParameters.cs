using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetCore.API.Data
{
    public class EntradasResourceParameters
    {

        public EntradasResourceParameters()
        {
        }

        const int _maxSize = 20;
        /// <summary>
        /// Tamaño de página. Máximo 20
        /// </summary>
        public int PageSize { get; set; } = 3;

        private int _pageNumber = 1;
        
        /// <summary>
        /// Numero de página. Empieza en 1.
        /// </summary>
        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (value > _maxSize) _pageNumber = _maxSize;
                else _pageNumber = value;
            }
        }

        private string _orderBy = "FechaCreacion";
        /// <summary>
        /// Campos de ordenación separados por coma
        /// </summary>
        /// <remarks>Ejemplo: OrderBy=FechaCreacion descending, Titulo ascending</remarks>
        public string OrderBy
        {
            get => _orderBy;
            set
            {
                if (value == null)
                {
                    _orderBy = "FechaCreacion";
                }
                else
                {
                    _orderBy = value;
                }
            }
        }

        /// <summary>
        /// Texto que se buscara en el título y contenido de la entrada
        /// </summary>
        public string SearchQuery { get; set; }
    }
}