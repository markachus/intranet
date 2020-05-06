using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntranetCore.Data.Entities
{
    public abstract class IntranetTrackingBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required, StringLength(50)]
        public string UsuarioCreacion { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required, StringLength(50)]
        public string UsuarioModificacion { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
        [Required]
        public bool Eliminado { get; set; }
    }
}
