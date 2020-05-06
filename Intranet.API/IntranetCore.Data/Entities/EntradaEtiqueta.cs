using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IntranetCore.Data.Entities
{
    public class EntradaEtiqueta
    {
        [Key, ForeignKey("Entrada")]
        public Guid EntradaId { get; set; }

        public Entrada Entrada { get; set; }

        [Key, ForeignKey("Etiqueta")]
        public Guid EtiquetaId { get; set; }
        public virtual Etiqueta Etiqueta { get; set; }
    }
}
