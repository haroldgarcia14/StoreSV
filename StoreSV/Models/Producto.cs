using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreSV.Models
{
    public class Producto
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProducto { get; set; }
            public string Nombre { get; set; }
            public int IdCategoria { get; set; }
            public virtual Categoria categoria { get; set; }

            public int IdMarca { get; set; }
            public virtual Marca marca { get; set; }

            public decimal Precio { get; set; }
            public int Stock { get; set; }
            public string RutaImagen { get; set; }
            public string NombreImagen { get; set; }
            public bool Activo { get; set; }
    }
}