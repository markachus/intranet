using IntranetCore.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetCore.Data.Models
{
    /// <summary>
    /// Modelo para actualizar una etiqueta
    /// </summary>
    public class EtiquetaForUpdateModel
    {

        /// <summary>
        /// Color de la etiqueta en hexadecimal. Ej: #FF4499
        /// </summary>
        [MustBeHexColor()]
        public string HexColor { get; set; }
    }
}