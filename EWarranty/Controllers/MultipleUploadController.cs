using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWarranty.Controllers
{
    public class MultipleUploadController : Controller
    {
        //
        // GET: /MultipleUpload/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ImageUpload(EWarranty.Models.GroupClass.ProductViewModel model)
        {

            var file = model.ImageFile;

            if (file != null)
            {

                var fileName = Path.GetFileName(file.FileName);
                var extention = Path.GetExtension(file.FileName);
                var filenamewithoutextension = Path.GetFileNameWithoutExtension(file.FileName);

                file.SaveAs(Server.MapPath("/UploadedImage/" + file.FileName));


            }

            return Json(file.FileName, JsonRequestBehavior.AllowGet);

        }
       
    }
}
