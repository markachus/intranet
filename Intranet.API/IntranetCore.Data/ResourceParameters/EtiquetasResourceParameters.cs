using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntranetCore.Data.ResourceParameters
{
    public class EtiquetasResourceParameters
    {

        const int _maxSize = 20;
        public int PageSize { get; set; } = 3;

        private int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (value > _maxSize) _pageNumber = _maxSize;
                else _pageNumber = value;
            }
        }

        public string SearchQuery { get; set; }
    }
}
