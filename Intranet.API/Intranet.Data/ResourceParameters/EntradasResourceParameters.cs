using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.API.Data
{
    public class EntradasResourceParameters
    {

        public EntradasResourceParameters()
        {
        }

        const int _maxSize = 20;
        public int PageSize { get; set; } = 3;

        private int _pageNumber = 1;
        public int PageNumber { get => _pageNumber; 
            set {
                if (value > _maxSize) _pageNumber = _maxSize;
                else _pageNumber = value;
        } }
    }
}