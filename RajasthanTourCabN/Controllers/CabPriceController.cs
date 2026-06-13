using RajasthanTourCabN.Data;
using RajasthanTourCabN.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RajasthanTourCabN.Controllers
{
    public class CabPriceController : Controller
    {
        PageService service = new PageService();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Admin"] == null)
            {
                filterContext.Result = RedirectToAction("index", "Account");
                return;
            }

            // ❌ Editor cannot delete
            if (Session["Role"].ToString() == "Editor" &&
                filterContext.ActionDescriptor.ActionName == "Delete")
            {
                filterContext.Result = RedirectToAction("Dashboard");
            }

            base.OnActionExecuting(filterContext);
        }

        public ActionResult Dashboard()
        {
            return View();
        }
        // ✅ List Page
        public ActionResult Pages()
        {
            var data = service.GetCabPricing();
            return View(data);
        }

        // ✅ Create Page (GET)
        public ActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CabPricing model)
        {
            service.InsertCab(model);

            return RedirectToAction("Pages");
        }
        // EDIT
        public ActionResult Edit(int id)
        {
            var data = service.GetById(id);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(CabPricing model)
        {
            service.UpdateCab(model);
            return RedirectToAction("Index");
        }

        // DELETE
        public ActionResult Delete(int id)
        {
            service.DeleteCab(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadImage()
        {
            var file = Request.Files[0];

            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string path = Server.MapPath("~/Uploads/" + fileName);

                file.SaveAs(path);

                string url = "/Uploads/" + fileName;

                return Json(new
                {
                    uploaded = 1,
                    fileName = fileName,
                    url = url
                });
            }

            return Json(new { uploaded = 0 });
        }
    }
}