using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Intranet.App.Models
{
    [Table("etiquetas")]
    public class EtiquetaModel
    {

        [PrimaryKey, MaxLength(50)]
        public string Nombre { get; set; }
        public string HexColor { get; set; }

        public Color Color { 
            get {
                if (string.IsNullOrEmpty(HexColor)) return Color.Default;
                else return Color.FromHex(this.HexColor);
        } }

        public DateTime FechaCreacion { get; set; }
    }
}
