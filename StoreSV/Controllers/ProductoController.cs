using StoreSV.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace StoreSV.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
        //public ActionResult Index()
        //{
        //    return View();
        //}

        private readonly StoreSVEntities databaseProducto;

        public ProductoController()
        {
            databaseProducto = new StoreSVEntities();
        }

        public ActionResult Producto()
        {
            //var producto = databaseProducto
            var ProductoActivos = databaseProducto.Productos.Where(a => a.Activo).Include(c => c.categoria).Include(m => m.marca).ToList();

            ViewBag.Categorias = new SelectList(databaseProducto.Categorias.ToList(), "IdCategoria", "Descripcion");
            ViewBag.Marcas = new SelectList(databaseProducto.Marcas.ToList(), "IdMarca", "Descripcion");

            return View(ProductoActivos);
        }

        [HttpGet]
        public JsonResult GetProducto(int id)
        {
            var producto = databaseProducto.Productos.Find(id);
            if(producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado" });
            }
            else
            {
                return Json(new { success = true, data = producto }, JsonRequestBehavior.AllowGet);
            }
        }

        //crear y editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAndEditProduct(Producto producto, HttpPostedFileBase imagen)
        {
            ViewBag.Categorias = new SelectList(await databaseProducto.Categorias.ToListAsync(), "IdCategoria", "Descripcion");
            ViewBag.Marcas = new SelectList(await databaseProducto.Marcas.ToListAsync(), "IdMarca", "Descripcion");

            Console.WriteLine(producto);
            if(producto.IdProducto > 0)
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine(producto);
                    //actualizar producto
                    var productoExistente = await databaseProducto.Productos.FindAsync(producto.IdProducto);

                    string nuevaRutaImagen = null;

                    if (productoExistente == null)
                    {
                        return HttpNotFound();
                    }

                    //actuializar imagen si se proporciona una
                    if (imagen != null && imagen.ContentLength > 0)
                    {
                        //validando tamaño imagen
                        if (imagen.ContentLength > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("", "El archivo es demasiado grande. el tamaño maximo es 5 MB");
                            return View(producto);
                        }
                        else
                        {
                            try
                            {
                                 nuevaRutaImagen = CloudinaryHelper.UpdateImage(imagen, productoExistente.RutaImagen);
                            }
                            catch(Exception ex)
                            {
                                ModelState.AddModelError("", "Error al subir la imagen:" + ex.Message);
                                return View(producto);
                            }
                            producto.RutaImagen = nuevaRutaImagen;
                            producto.NombreImagen = imagen.FileName;

                        }
                    }
                    else
                    {
                        producto.RutaImagen = productoExistente.RutaImagen;
                        producto.NombreImagen = productoExistente.NombreImagen;
                    }
                    
                    try
                    {
                        await databaseProducto.EditProductAsync(producto.IdProducto, producto.Nombre, producto.IdCategoria, producto.IdMarca, producto.Precio, producto.Stock, producto.RutaImagen, producto.NombreImagen, producto.Activo);
                        return RedirectToAction(nameof(Producto));
                    }
                    catch(Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                    }
                }
            }
            else if (producto.IdProducto == 0)
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine(producto);

                    string rutaImagen = null, nombreImagen = null;

                    if(imagen != null && imagen.ContentLength > 0)
                    {
                        //validando tamaño de imagen
                        if(imagen.ContentLength > 5 * 1024 * 1024)
                        {
                            
                            ModelState.AddModelError("","El archivo es demasiado grande. el tamaño maximo es 5 MB");
                            return View(producto);
                            /*
                            return Json(new { success = false, message = "El archivo es demasiado grande. El tamaño máximo permitido es 5 MB." });
                            */
                        }

                        try
                        {
                            rutaImagen = CloudinaryHelper.UploadImage(imagen);
                            nombreImagen = imagen.FileName;
                        }
                        catch(Exception ex)
                        {
                            
                            ModelState.AddModelError("", "Error al subir la imagen:" + ex.Message);
                            return View(producto);
                            
                            //return Json(new { success = false, message = "Error al subir la imagen: " + ex.Message });
                        }
                    }

                    producto.RutaImagen = rutaImagen;
                    producto.NombreImagen = nombreImagen;

                    try
                    {
                        await databaseProducto.CreateProductoAsync(producto.Nombre, producto.IdCategoria, producto.IdMarca, producto.Precio, producto.Stock, producto.RutaImagen, producto.NombreImagen, producto.Activo);
                        return RedirectToAction(nameof(Producto));
                        //return Json(new { success = true });
                    }catch(Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                        //return Json(new { success = false, message = "Modelo invalido" });
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

            ViewBag.Categorias = new SelectList(await databaseProducto.Categorias.ToListAsync(), "IdCategoria", "Descripcion");
            ViewBag.Marcas = new SelectList(await databaseProducto.Marcas.ToListAsync(), "IdMarca", "Descripcion");

            return View(producto);
        }

        //Eliminar
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await databaseProducto.DeleteProductAsync(id);
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}