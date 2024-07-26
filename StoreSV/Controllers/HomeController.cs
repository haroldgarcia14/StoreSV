using StoreSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StoreSV.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}

        private readonly StoreSVEntities database;

        //constructor
        public HomeController()
        {
            database = new StoreSVEntities();
        }

        //Crud razor
        #region USUARIO

        //obtener listado
        public ActionResult Index()
        {
            var UsuarioActivos = database.Usuarios.Where(u => u.Activo);
            return View(UsuarioActivos);
        }

        //para obtener los datos de un id
        public JsonResult GetUsuario(int id)
        {
            var usuario = database.Usuarios.Find(id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var usuarioData = new
                {
                    idUsuario = usuario.idUsuario,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    Correo = usuario.Correo,
                    Clave = usuario.Clave,
                    Activo = usuario.Activo
                };

                return Json(new { success = true, data = usuarioData }, JsonRequestBehavior.AllowGet);
            }

        }

        //crear y editar usuarios
        [HttpPost]
        [ValidateAntiForgeryToken] //impidiendo falsificacion de solicitud
        public async Task<ActionResult> CreateAndEdit(Usuario usuario)
        {
            Console.WriteLine(usuario);
            if (usuario.idUsuario > 0)
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(usuario.Clave))
                    {
                        try
                        {
                            await database.EditUserAsync(usuario.idUsuario, usuario.Nombres, usuario.Apellidos, usuario.Correo, usuario.Activo);
                            return RedirectToAction(nameof(Index));
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                        }
                    }
                }
                else
                {
                    var errores = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errores)
                    {
                        System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                    }
                }

            }
            else if (usuario.idUsuario == 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        await database.CreateUserAsync(usuario.Nombres, usuario.Apellidos, usuario.Correo, usuario.Clave, usuario.Activo);
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                    }
                }
                else
                {
                    var errores = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errores)
                    {
                        System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                    }
                }
            }
            else
            {
                return HttpNotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {

            try
            {
                await database.DeleteUserAync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        #endregion

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}