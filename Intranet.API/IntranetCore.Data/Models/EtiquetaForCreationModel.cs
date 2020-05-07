using IntranetCore.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetCore.Data.Models
{
    /// <summary>
    /// Modelo para crear una nueva etiqueta
    /// </summary>
    public class EtiquetaForCreationModel
    {
        /// <summary>
        /// Nombre de la etiqueta. Debe ser único
        /// </summary>
        [Required, StringLength(50)]
        public string Nombre { get; set; }

        /// <summary>
        /// Color de la etiqueta en hexadecimal. Ej: #FF4499
        /// </summary>
        [MustBeHexColor]
        public string HexColor { get; set; }
    }
}