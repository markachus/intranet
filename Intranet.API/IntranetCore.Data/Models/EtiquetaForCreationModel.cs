using IntranetCore.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetCore.Data.Models
{
    public class EtiquetaForCreationModel
    {
        [Required, StringLength(50)]
        public string Nombre { get; set; }

        [MustBeHexColor]
        public string HexColor { get; set; }
    }
}