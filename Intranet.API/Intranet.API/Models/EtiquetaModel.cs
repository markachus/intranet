using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intranet.API.Models
{
    public class EtiquetaModel
    {
        [Required, StringLength(50)]
        public string Nombre { get; set; }

        public string HexColor { get; set; }

        public DateTime FechaUltimaModificacion { get; set; }
    }
}