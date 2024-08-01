using StoreSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StoreSV.Controllers
{
    public class CategoriaController : Controller
    {
        // GET: Categoria
        //public ActionResult Index()
        //{
        //    return View();
        //}

        private readonly StoreSVEntities databaseCategoria;

        public CategoriaController()
        {
            databaseCategoria = new StoreSVEntities();
        }

        #region CATEGORIA
        //para que cargue el html(razor)
        public ActionResult Categoria()
        {
            var CategoriaActivos = databaseCategoria.Categorias.Where(c => c.Activo);
            return View(CategoriaActivos);
        }

        // GET: Categoria
        //devolver la lista de las categorias
        [HttpGet]
        public JsonResult TablaCategoria()
        {
            var CategoriaActivos = databaseCategoria.Categorias.Where(u => u.Activo).ToList();
            return Json(CategoriaActivos, JsonRequestBehavior.AllowGet);
        }

        //para obtener los datos de un id
        public JsonResult GetCategoria(int id)
        {
            var categoria = databaseCategoria.Categorias.Find(id);
            if (categoria == null)
            {
                return Json(new { success = false, message = "Categoria no encontrado" });
            }
            else
            {
                var categoriaData = new
                {
                    IdCategoria = categoria.IdCategoria,
                    Descripcion = categoria.Descripcion,
                    Activo = categoria.Activo
                };

                return Json(new { success = true, data = categoriaData }, JsonRequestBehavior.AllowGet);
            }
        }

        //Crear y editar categorias
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAndEditCategory(Categoria categoria)
        {
            Console.WriteLine(categoria);
            if (categoria.IdCategoria > 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        await databaseCategoria.EditCategoryAsync(categoria.IdCategoria, categoria.Descripcion, categoria.Activo);
                        return RedirectToAction(nameof(Categoria));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Error: ${ex.Message}");
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
            else if (categoria.IdCategoria == 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        await databaseCategoria.CreateCategoryAsync(categoria.Descripcion, categoria.Activo);
                        return RedirectToAction(nameof(Categoria));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Error: ${ex.Message}");
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

            return View(categoria);
        }

        //eliminar
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await databaseCategoria.DeleteCategoryAsync(id);
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        #endregion
    }
}