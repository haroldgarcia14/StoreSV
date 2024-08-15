using StoreSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StoreSV.Controllers
{
    public class MarcaController : Controller
    {
        // GET: Marca
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //variable constante
        private readonly StoreSVEntities databaseMarca;

        //constructor
        public MarcaController()
        {
            //creando instancia
            databaseMarca = new StoreSVEntities();
        }

        #region MARCA

        //CRUD

        //vista
        public ActionResult Marca()
        {
            var marcaActivos = databaseMarca.Marcas.Where(m => m.Activo).ToList();
            return View(marcaActivos);
        }

        //devolver la lista
        [HttpGet]
        public JsonResult GetMarca(int id)
        {
            var marca = databaseMarca.Marcas.Find(id);
            if(marca == null)
            {
                return Json(new { success = false, message = "Marca no encontrado" });
            }
            else
            {
                var marcaData = new
                {
                    IdMarca = marca.IdMarca,
                    Descripcion = marca.Descripcion,
                    Activo = marca.Activo
                };
                return Json(new { success = true, data = marcaData }, JsonRequestBehavior.AllowGet);
            }
        }

        //Crear y editar Marcas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAndEditBrand(Marca marca)
        {
            if (marca.IdMarca > 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Console.WriteLine(marca);
                        await databaseMarca.EditBrandAsync(marca.IdMarca, marca.Descripcion, marca.Activo);
                        return RedirectToAction(nameof(Marca));
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
            else if (marca.IdMarca == 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Console.WriteLine(marca);
                        await databaseMarca.CreateBrandAsync(marca.Descripcion, marca.Activo);
                        return RedirectToAction(nameof(Marca));
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

            return View(marca);
        }

        //Eliminar
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await databaseMarca.DeleteBrandAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        #endregion
    }
}