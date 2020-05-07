using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetCore.Data.Models
{
    /// <summary>
    /// Entrada/post de la intranet
    /// </summary>
    public class EntradaModel
    {
        /// <summary>
        /// Identificador único
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Titulo de la entrada
        /// </summary>
        [Required, StringLength(100)]
        public string Titulo { get; set; }

        /// <summary>
        /// Contenido de la entrada
        /// </summary>
        [Required, StringLength(4096, MinimumLength = 20)]
        public string Contenido { get; set; }

        /// <summary>
        /// Etiquetas asociadas con esta entrada
        /// </summary>
        public  List<EtiquetaModel> Etiquetas { get; set; }
    }
}