using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Intranet.Data.Entities
{
    public class Entrada : IntranetTrackingBase
    {

        [Required, StringLength(100)]
        public string Titulo { get; set; }

        [Required]
        public string Contenido { get; set; }

        private List<Etiqueta> _etiquetas = new List<Etiqueta>();
        public virtual List<Etiqueta> Etiquetas { get => _etiquetas; set => _etiquetas = value; }
    }
}
