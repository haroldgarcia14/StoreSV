﻿using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

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

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Marca> Marcas { get; set; }

        public DbSet<Producto> Productos { get; set; }

        //modelos de la tabla para el ingreso de productos Jeans-pantalones

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("USUARIO");
            modelBuilder.Entity<Categoria>().ToTable("CATEGORIA");
            modelBuilder.Entity<Marca>().ToTable("MARCA");
            modelBuilder.Entity<Producto>().ToTable("PRODUCTO");

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }



        /*********************USUARIO**********************/
        #region Usuario
        //crud

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

        /*********************CATEGORIA********************/
        #region Categoria
        //Crear
        public async Task CreateCategoryAsync(string descripcion, bool activo)
        {
            try
            {
                var DescripcionParam = new SqlParameter("@Descripcion", descripcion);
                var ActivoParam = new SqlParameter("@Activo", activo);

                await Database.ExecuteSqlCommandAsync("EXECUTE sp_RegistrarCategoria @Descripcion, @Activo",
                    DescripcionParam, ActivoParam);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar sp_RegistrarCategoria. Detalles:" + ex.Message);
            }
        }

        //Editar
        public async Task EditCategoryAsync(int id, string descripcion, bool activo)
        {
            try
            {
                var idParam = new SqlParameter("@IdCategoria", id);
                var DescripcionParam = new SqlParameter("@Descripcion", descripcion);
                var ActivoParam = new SqlParameter("@Activo", activo);

                await Database.ExecuteSqlCommandAsync("EXECUTE sp_EditarCategoria @IdCategoria, @Descripcion, @Activo",
                    idParam, DescripcionParam, ActivoParam);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar el sp_EditarCategoria. Detalles: " + ex.Message);
            }
        }

        //eliminar
        public async Task DeleteCategoryAsync(int id)
        {
            var IdParam = new SqlParameter("@IdCategoria", id);

            await Database.ExecuteSqlCommandAsync("EXECUTE sp_EliminarCategoria @IdCategoria", IdParam);
        }

        #endregion
        /****************Fin de Categoria******************/

        /*********************Marca************************/
        #region Marca
        //registrar
        public async Task CreateBrandAsync(string descripcion, bool activo)
        {
            try
            {
                var DescripcionParam = new SqlParameter("@Descripcion", descripcion);
                var activoParam = new SqlParameter("@Activo", activo);
                Console.WriteLine(DescripcionParam);
                Console.WriteLine(activoParam);


                await Database.ExecuteSqlCommandAsync("EXECUTE sp_RegistrarMarca @Descripcion, @Activo", DescripcionParam, activoParam);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar el sp_RegistrarMarca. Detalles: " + ex.Message);
            }
        }

        //Editar
        public async Task EditBrandAsync(int id, string descripcion, bool activo)
        {
            try
            {
                var idParam = new SqlParameter("@IdMarca", id);
                var DescripcionParam = new SqlParameter("@Descripcion", descripcion);
                var ActivoParam = new SqlParameter("@Activo", activo);

                await Database.ExecuteSqlCommandAsync("EXECUTE sp_EditarMarca @IdMarca, @Descripcion, @Activo",
                    idParam, DescripcionParam, ActivoParam);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar el sp_EditarCategoria. Detalles: " + ex.Message);
            }
        }

        //Eliminar
        public async Task DeleteBrandAsync(int id)
        {
            var IdParam = new SqlParameter("@IdMarca", id);

            await Database.ExecuteSqlCommandAsync("EXECUTE sp_EliminarMarca @IdMarca", IdParam);
        }
        #endregion
        /****************Fin de Marca**********************/

        /*********************Producto************************/
        #region Producto

        //Registrar
        public async Task CreateProductoAsync(string nombre, int IdCategoria, int IdMarca, decimal precio, 
            int stock, string rutaImagen, string nombreImagen, decimal resultadoIVA, bool activo)
        {
            try
            {
                /*
                var NombreParam = new SqlParameter("@Nombre", nombre);
                var IdCategoriaParam = new SqlParameter("@IdCategoria", IdCategoria);
                var IdMarcaParam = new SqlParameter("@IdMarca", IdMarca);
                var PrecioParam = new SqlParameter("@Precio", precio);
                var StockParam = new SqlParameter("@Stock", stock);
                var RutaImagenParam = new SqlParameter("@RutaImagen", rutaImagen);
                var NombreImagenParam = new SqlParameter("@NombreImagen",nombreImagen);
                var ActivoParam = new SqlParameter("@Activo",activo);
                */
                var NombreParam = new SqlParameter("@Nombre", SqlDbType.VarChar, 100) {Value = nombre ?? (object)DBNull.Value };
                var IdCategoriaParam = new SqlParameter("@IdCategoria", SqlDbType.Int) {Value = IdCategoria };
                var IdMarcaParam = new SqlParameter("@IdMarca", SqlDbType.Int) {Value = IdMarca };
                var PrecioParam = new SqlParameter("@Precio", SqlDbType.Decimal) { Value = precio };
                var StockParam = new SqlParameter("@Stock", SqlDbType.Int) { Value = stock };
                var RutaImagenParam = new SqlParameter("@RutaImagen", SqlDbType.VarChar, -1) {Value = rutaImagen ?? (object)DBNull.Value };
                var NombreImagenParam = new SqlParameter("@NombreImagen", nombreImagen);
                var ResultadoIVAParam = new SqlParameter("@Resultado_IVA", resultadoIVA);
                var ActivoParam = new SqlParameter("@Activo", activo);

                await Database.ExecuteSqlCommandAsync("EXECUTE sp_RegistrarProducto @Nombre, @IdCategoria, @IdMarca, @Precio, @Stock, @RutaImagen, @NombreImagen, @Resultado_IVA, @Activo", NombreParam,IdCategoriaParam,IdMarcaParam,PrecioParam,StockParam,RutaImagenParam,NombreImagenParam, ResultadoIVAParam, ActivoParam);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar el sp_RegistrarProducto. Detalles: " + ex.Message);
            }
        }

        //Editar
        public async Task EditProductAsync(int id, string nombre, int IdCategoria, int IdMarca, decimal precio, int stock, string rutaImagen, string nombreImagen, decimal resultadoIVA, bool activo)
        {
            try
            {
                var idParam = new SqlParameter("@IdProducto", SqlDbType.Int) { Value = id };
                var NombreParam = new SqlParameter("@Nombre", SqlDbType.VarChar, 100) { Value = nombre ?? (object)DBNull.Value };
                var IdCategoriaParam = new SqlParameter("@IdCategoria", SqlDbType.Int) { Value = IdCategoria };
                var IdMarcaParam = new SqlParameter("@IdMarca", SqlDbType.Int) { Value = IdMarca };
                var PrecioParam = new SqlParameter("@Precio", SqlDbType.Decimal) { Value = precio };
                var StockParam = new SqlParameter("@Stock", SqlDbType.Int) { Value = stock };
                var RutaImagenParam = new SqlParameter("@RutaImagen", SqlDbType.VarChar, -1) { Value = rutaImagen ?? (object)DBNull.Value };
                var NombreImagenParam = new SqlParameter("@NombreImagen", nombreImagen);
                var ResultadoIVAParam = new SqlParameter("@Resultado_IVA", resultadoIVA);
                var ActivoParam = new SqlParameter("@Activo", activo);

                await Database.ExecuteSqlCommandAsync("EXECUTE sp_EditarProducto @IdProducto, @Nombre, @IdCategoria, @IdMarca, @Precio, @Stock, @RutaImagen, @NombreImagen, @Resultado_IVA, @Activo", idParam, NombreParam, IdCategoriaParam, IdMarcaParam, PrecioParam, StockParam, RutaImagenParam, NombreImagenParam, ResultadoIVAParam, ActivoParam);
            }
            catch(Exception ex)
            {
                throw new Exception("Error al ejecutar el sp_RegistrarProducto. Detalles: " + ex.Message);
            }
        }

        //Eliminar
        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var idParam = new SqlParameter("@IdProducto", id);

                await Database.ExecuteSqlCommandAsync("EXECUTE sp_EliminarProducto @IdProducto", idParam);
            }
            catch(Exception ex)
            {
                throw new Exception("Error al ejecutar el sp_RegistrarProducto. Detalles: " + ex.Message);
            }
        }
        #endregion
        /****************Fin de Producto**********************/

    }
}