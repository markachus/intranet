using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetCore.Data.Models
{
    public class EtiquetaModel
    {
        /// <summary>
        /// Nombre de la etiqueta
        /// </summary>
        [Required, StringLength(50)]
        public string Nombre { get; set; }

        /// <summary>
        /// Color de la etiqueta en hexadecimal. Ej: #FF4499
        /// </summary>
        public string HexColor { get; set; }

        /// <summary>
        /// Fecha de la última modificación efectuada a la etiqueta
        /// </summary>
        public DateTime FechaUltimaModificacion { get; set; }
    }
}