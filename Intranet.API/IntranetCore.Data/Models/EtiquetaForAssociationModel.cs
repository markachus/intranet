using IntranetCore.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetCore.Data.Models
{
    /// <summary>
    /// Modelo para crear asociar una etiqueta
    /// </summary>
    public class EtiquetaForAssociationModel
    {
        /// <summary>
        /// Nombre de la etiqueta
        /// </summary>
        [Required]
        public string Nombre { get; set; }

        //TODO añadir Guid Id y hacer regla para requerir o Nombre o Id
    }
}