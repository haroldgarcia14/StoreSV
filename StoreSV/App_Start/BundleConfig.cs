using System.Web;
using System.Web.Optimization;

namespace StoreSV
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /******************ENLACES JS*******************/
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios.  De esta manera estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));


            //AGREGADOS

            bundles.Add(new Bundle("~/bundles/easing").Include(
                    "~/Scripts/lib/easing/easing.js"
                ));


            bundles.Add(new Bundle("~/bundles/waypoints").Include(
                    "~/Scripts/lib/waypoints/waypoints.min.js"
                ));

            bundles.Add(new Bundle("~/bundles/main").Include(
                    "~/Scripts/js/main.js"
                ));

            /**************ENLACES CSS*******************/
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //AGREGADOS

            //carrousel
            bundles.Add(new StyleBundle("~/Content/libcss").Include(
                      "~/Scripts/lib/owlcarousel/assets/owl.carousel.min.css",
                      "~/Scripts/lib/tempusdominus/css/tempusdominus-bootstrap-4.min.css"
                        ));

            //customized bootstrap
            bundles.Add(new StyleBundle("~/Content/bootstrapcustom").Include(
                       "~/Content/css/bootstrap.min.css"
                       ));

            //template Stylesheet
            bundles.Add(new StyleBundle("~/Content/style").Include(
                       "~/Content/css/style.css"
                       ));
        }
    }
}
