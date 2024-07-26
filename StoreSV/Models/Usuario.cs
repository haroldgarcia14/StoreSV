using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreSV.Models
{
    public class Usuario
    {
        //variables de la tabla Usuario

        //id de la tabla
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100, ErrorMessage = "Los nombres no pueden tener mas de 100 caracteres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden tener más de 100 caracteres")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        public string Correo { get; set; }

        //[Required(ErrorMessage = "La clave es obligatoria")]
        //[StringLength(100, MinimumLength = 3, ErrorMessage = "La clave debe tener entre 3 y 100 caracteres")]
        public string Clave { get; set; }
        public bool Reestablecer { get; set; }

        //se hace referencia a activo o inactivo
        public bool Activo { get; set; }
    }
}