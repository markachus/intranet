using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Data.Entities
{
    public class Etiqueta : IntranetTrackingBase
    {
        [Required, Index(IsUnique = true), StringLength(50)]
        public string Nombre { get; set; }

        public string HexColor { get; set; }

    }
}
