using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace StoreSV.Models
{
    public static class CloudinaryHelper
    {
        private static readonly Cloudinary cloudinary;

        static CloudinaryHelper() 
        {
            try
            {
                var cloudName = ConfigurationManager.AppSettings["CloudinaryCloud"];
                var apiKey = ConfigurationManager.AppSettings["CloudinaryApiKey"];
                var apiSecret = ConfigurationManager.AppSettings["CloudinaryApiSecret"];

                if(string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
                {
                    throw new ConfigurationErrorsException("Faltan configuraciones de Cloudinary en el Web.config");
                }

                var account = new Account(cloudName, apiKey, apiSecret);
                cloudinary = new Cloudinary(account);
            }
            catch(Exception ex)
            {
                throw new Exception("Error al inicializar Cloudinary: " + ex.Message, ex);
            }
        }

        //crear
        public static string UploadImage(HttpPostedFileBase file)
        {
            if(file == null || file.ContentLength == 0) return null;

            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName,file.InputStream)
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                return uploadResult.SecureUri.AbsoluteUri;
            }
            catch(Exception ex)
            {
                throw new Exception("Error al subir la imagen a Cloudinary: " + ex.Message, ex);
            }
        }



        //actualizar
        public static string UpdateImage(HttpPostedFileBase file, string oldImageUrl)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    return oldImageUrl;
                }

                //subir la nueva imagen
                string newImageUrl = UploadImage(file);

                //si la nueva imagen se subio correctamente, se elimina la antigua
                if(!string.IsNullOrEmpty(newImageUrl) && !string.IsNullOrEmpty(oldImageUrl))
                {
                    DeleteImage(oldImageUrl);
                }

                // Devolver la URL de la nueva imagen o la antigua si algo falló
                return newImageUrl ?? oldImageUrl;

            }
            catch(Exception ex)
            {
                throw new Exception("Error al actualizar la imagen en Cloudinary: " + ex.Message, ex);
            }
        }

        //metodo para eliminar la imagen actual
        public static void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            try
            {
                //extraer el public id de la url
                var uri = new Uri(imageUrl);
                var pathSegment = uri.Segments;
                var fileName = pathSegment[pathSegment.Length - 1];
                var publicId = System.IO.Path.GetFileNameWithoutExtension(fileName);

                var deleteParams = new DeletionParams(publicId);
                var result = cloudinary.Destroy(deleteParams);

                if (result.Result != "ok")
                {
                    throw new Exception($"Error al eliminar la imagen: {result.Result}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la imagen de Cloudinary: " + ex.Message, ex);
            }
        }
    }
}