using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Web;

namespace StoreSV.Models
{
    public class StoreSVEntities : DbContext
    {
        public StoreSVEntities() : base("cadenaconexion") 
        {
            //Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        //variable de referencia para entidades
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("USUARIO");

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        //crud
        /*********************USUARIO**********************/
        #region Usuario
        //crear
        public async Task CreateUserAsync(string nombre, string apellido, string correo,
            string clave, bool activo)
        {


            var passwordHash = Argon2.Hash(clave);
            if (Argon2.Verify(passwordHash, clave))
            {
                var NombreParam = new SqlParameter("@Nombres", nombre);
                var ApellidoParam = new SqlParameter("@Apellidos", apellido);
                var CorreoParam = new SqlParameter("@Correo", correo);
                var ClaveParam = new SqlParameter("@Clave", passwordHash);
                var ActivoParam = new SqlParameter("@Activo", activo);

                await Database.ExecuteSqlCommandAsync("EXECUTE sp_RegistrarUsuario @Nombres,@Apellidos,@Correo,@Clave,@Activo",
                NombreParam, ApellidoParam, CorreoParam, ClaveParam, ActivoParam);
            }


        }

        //editar
        public async Task EditUserAsync(int id, string nombre, string apellido, string correo, bool activo)
        {
            var IdParam = new SqlParameter("@IdUsuario", id);
            var NombreParam = new SqlParameter("@Nombres", nombre);
            var ApellidoParam = new SqlParameter("@Apellidos", apellido);
            var CorreoParam = new SqlParameter("@Correo", correo);
            var ActivoParam = new SqlParameter("@Activo", activo);

            await Database.ExecuteSqlCommandAsync("EXECUTE sp_EditarUsuario @IdUsuario,@Nombres,@Apellidos,@Correo,@Activo",
                IdParam, NombreParam, ApellidoParam, CorreoParam, ActivoParam);
        }

        //eliminar registro
        public async Task DeleteUserAync(int id)
        {
            var IdParam = new SqlParameter("@IdUsuario", id);

            await Database.ExecuteSqlCommandAsync("EXECUTE sp_EliminarRegistro @IdUsuario", IdParam);
        }
        #endregion
        /****************Fin de Usuario********************/
    }
}