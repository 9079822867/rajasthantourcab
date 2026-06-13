using RajasthanTourCabN.Data;
using RajasthanTourCabN.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RajasthanTourCabN.Controllers
{
    public class AdminController : Controller
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
            var data = service.GetPages();
            return View(data);
        }

        // ✅ Create Page (GET)
        public ActionResult Create()
        {
            ViewBag.Parents = service.GetPages().Where(x => x.ParentId == null).ToList();
            return View();
        }

        // POST
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Page model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                ModelState.AddModelError("", "Title is required");
                return View(model);
            }

            // ✅ Auto Slug Generate
            model.Slug = GenerateSlug(model.Title);

            // ✅ Default values
            model.IsActive = true;
            //var sanitizer = new HtmlSanitizer();
            //model.Content = sanitizer.Sanitize(model.Content);
            service.InsertPage(model);

            return RedirectToAction("Pages");
        }
        // 🔥 Slug Generator
        public string GenerateSlug(string text)
        {
            return text.ToLower().Trim()
                       .Replace(" ", "-")
                       .Replace("&", "and")
                       .Replace("--", "-");
        }

        public ActionResult Edit(int id)
        {
            var page = service.GetPages().FirstOrDefault(x => x.Id == id);

            if (page == null)
                return HttpNotFound();

            ViewBag.Parents = service.GetPages().Where(x => x.ParentId == null).ToList();

            return View(page);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Page model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                ModelState.AddModelError("", "Title is required");
                return View(model);
            }

            // ✅ regenerate slug if title changed
            model.Slug = GenerateSlug(model.Title);
            //var sanitizer = new HtmlSanitizer();
            //model.Content = sanitizer.Sanitize(model.Content);
            service.UpdatePage(model);

            return RedirectToAction("Pages");
        }

        // ✅ Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            service.DeletePage(id);
            return RedirectToAction("Pages");
        }
        [HttpGet]
        public ActionResult UploadImage1()
        { return View(); }

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

        public ActionResult ImageList()
        {
            string imageFolder = Server.MapPath("~/Uploads/");
            var imageFiles = Directory.GetFiles(imageFolder)
                .Select(Path.GetFileName)
                .ToList();
            ViewBag.ImageFiles = imageFiles;
            return View();
        }

        public ActionResult BookingList()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("index", "Account");
            }
            List<BookingInquiryModel> list = new List<BookingInquiryModel>();
            list=service.GetBookingInquirys();
            return View(list);
        }
        public ActionResult AdminFeedback()
        {
            var feedbackList = service.GetAllFeedback();
            return View(feedbackList);
        }

        public ActionResult DeleteBooking(int id)
        {
            service.DeleteBookingInquiry(id);
            return RedirectToAction("BookingList");
        }
        [HttpPost]
        public JsonResult UpdateBookingStatus(int bookingId, string status)
        {
            try
            {
                service.UpdateBookingInquiryStatus(bookingId, status);
                return Json(new
                {
                    status = true
                });
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        }
    }
}