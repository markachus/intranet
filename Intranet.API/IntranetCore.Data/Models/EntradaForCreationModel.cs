using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetCore.Data.Models
{
    public class EntradaForCreationModel
    {

        [Required, StringLength(100)]
        public string Titulo { get; set; }

        [Required, StringLength(4096, MinimumLength = 20)]
        public string Contenido { get; set; }

        public List<string> Etiquetas { get; set; } = new List<string>();
    }
}