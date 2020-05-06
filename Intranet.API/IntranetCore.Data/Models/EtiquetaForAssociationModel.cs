using IntranetCore.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetCore.Data.Models
{
    public class EtiquetaForAssociationModel
    {
        [Required]
        public string Nombre { get; set; }
    }
}