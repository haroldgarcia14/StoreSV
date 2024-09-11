using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreSV.Models
{
    public class Categoria
    {
        //campos de la tabla Categoria

        //id de la tabla categoria
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(100, ErrorMessage = "La descripción no puede tener mas de 100 caracteres")]
        public string Descripcion { get; set; }

        //se hace referencia a activo o inactivo
        public bool Activo { get; set; }

        //public virtual ICollection<Producto> Productos { get; set; }
    }
}