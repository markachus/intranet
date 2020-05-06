using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntranetCore.Data.Entities
{
    public class Entrada : IntranetTrackingBase
    {

        [Required, StringLength(100)]
        public string Titulo { get; set; }

        [Required]
        public string Contenido { get; set; }

        private List<EntradaEtiqueta> _etiquetas = new List<EntradaEtiqueta>();
        public virtual List<EntradaEtiqueta> Etiquetas { get => _etiquetas; set => _etiquetas = value; }
    }
}
