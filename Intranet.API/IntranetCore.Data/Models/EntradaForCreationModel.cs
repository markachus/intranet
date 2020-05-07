using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetCore.Data.Models
{
    /// <summary>
    /// Modelo para crear una nueva entrada o post en la intranet
    /// </summary>
    public class EntradaForCreationModel
    {
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
        /// Lista de nombres de etiquetas para asociar con la entrada que se quiere crear
        /// </summary>
        public List<string> Etiquetas { get; set; } = new List<string>();
    }
}