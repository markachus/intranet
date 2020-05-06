using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntranetCore.Data.Entities
{
    public class Etiqueta : IntranetTrackingBase
    {
        [Required, StringLength(50)]
        public string Nombre { get; set; }

        public string HexColor { get; set; }

        private List<EntradaEtiqueta> _entradas = new List<EntradaEtiqueta>();
        public virtual List<EntradaEtiqueta> Entradas { get => _entradas; set => _entradas = value; }

    }
}
