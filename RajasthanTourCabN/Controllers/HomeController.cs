using RajasthanTourCabN.Data;
using RajasthanTourCabN.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RajasthanTourCabN.Controllers
{
    public class HomeController : Controller
    {
        PageService service = new PageService();
        public ActionResult Index()
        {
            var data = service.GetCabPricing().Where(x=>x.City== "Jaipur").ToList();
            ViewBag.Cities = service.GetCabPricing().Select(x => x.City).Distinct().ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult Feedback()
        {
            return View(new FeedbackViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback(FeedbackViewModel model)
        {
            if (ModelState.IsValid)
            {
                var feedback = new Feedback
                {
                    Rating = model.Rating,
                    Comments = model.Comments,
                    SubmittedOn = DateTime.Now
                };
                service.InsertFeedback(feedback);
                TempData["FeedbackMsg"] = "Thank you for your feedback!";
                return RedirectToAction("ThankYou");
            }
            return View(model);
        }

        
        public ActionResult ThankYou()
        {
            ViewBag.Message = TempData["FeedbackMsg"] ?? "Thank you!";
            return View();
        }
        public ActionResult Menu()
        {
            var pages = service.GetPages();
            return PartialView("_Menu", pages);
        }

        public ActionResult HomePartial()
        {
            var page = service.GetPages().FirstOrDefault(x => x.Slug.ToLower() == "home");
            return PartialView("_HomePartial", page);
        }
        public ActionResult FooterPartial()
        {
            var page = service.GetPages().FirstOrDefault(x => x.Slug.ToLower() == "footer");
            return PartialView("_FooterPartial", page);
        }
        
        public ActionResult Page(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                slug = "home";
            else if (Convert.ToString(slug).ToLower() == "home")
                slug = "home";

            // tolerate links that mistakenly include a .cshtml/.html extension
            if (slug.EndsWith(".cshtml", StringComparison.OrdinalIgnoreCase))
                slug = slug.Substring(0, slug.Length - ".cshtml".Length);
            else if (slug.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                slug = slug.Substring(0, slug.Length - ".html".Length);

            var page = service.GetPages().FirstOrDefault(x => x.Slug.ToLower() == slug.ToLower());

            if (page == null)
                return Error404();
            ViewBag.Title = !string.IsNullOrEmpty(page.MetaTitle) ? page.MetaTitle : page.Title;
            ViewBag.MetaDescription = page.MetaDescription;
            ViewBag.MetaKeywords = page.MetaKeywords;
            if(slug=="home")
            {
                var data = service.GetCabPricing().Where(x => x.City == "Jaipur").ToList();
                ViewBag.Cities = service.GetCabPricing().Select(x => x.City).Distinct().ToList();
                return View("index", data);
            }
            if (slug == "hire-driver-in-india")
            {
                return View("HireDriver", page);
            }
            if (slug == "driver-fares")
            {
                ViewBag.Fares = service.GetDriverFares();
                return View("DriverFares", page);
            }
            return View(page);
        }
        public string GenerateSlug(string text)
        {
            return text.ToLower()
                       .Replace(" ", "-")
                       .Replace("&", "and")
                       .Replace("--", "-");
        }
        public ActionResult Error404()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            ViewBag.Title = "Page Not Found";
            return View("NotFound");
        }

        public ActionResult About()
        {

            return View();
        }
        public ActionResult Gallery()
        {

            return View();
        }
        public ActionResult Blog()
        {

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public JsonResult GetPricingByCity(string city)
        {
            var data = service.GetCabPricing().Where(x => x.City == city).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Book(Booking model)
        {
            service.SaveBooking(model);
            return RedirectToAction("Index");
        }
        public ActionResult HireDriver()
        {
            var page = service.GetPages().FirstOrDefault(x => x.Slug.ToLower() == "hire-driver-in-india");
            if (page != null)
            {
                ViewBag.Title = !string.IsNullOrEmpty(page.MetaTitle) ? page.MetaTitle : page.Title;
                ViewBag.MetaDescription = page.MetaDescription;
                ViewBag.MetaKeywords = page.MetaKeywords;
            }
            return View(page);
        }

        [HttpPost]
        public JsonResult SaveDriverBooking(DriverBooking model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.MobileNo))
                {
                    return Json(new { status = false, message = "Please enter your mobile number." });
                }
                if (string.IsNullOrWhiteSpace(model.PickupLocation) || string.IsNullOrWhiteSpace(model.DropLocation))
                {
                    return Json(new { status = false, message = "Please enter both pickup and drop locations." });
                }

                service.SaveDriverBooking(model);
                return Json(new
                {
                    status = true,
                    message = "Driver booking request submitted successfully. We will call you shortly."
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SaveBooking(BookingInquiryModel model)
        {
            try
            {
                service.SaveBookingInquiry(model);
                return Json(new
                {
                    status = true,
                    message = "Booking submitted successfully."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult SubmitFeedback(Feedback model)
        {
            if (ModelState.IsValid)
            {
                model.SubmittedOn = DateTime.Now;
                // Save to DB (add to your DbContext and save)
                // db.Feedbacks.Add(model); db.SaveChanges();
                TempData["FeedbackMsg"] = "Thank you for your review!";
                return RedirectToAction("ImageList");
            }
            TempData["FeedbackMsg"] = "Please fill all required fields.";
            return RedirectToAction("ImageList");
        }
    }
}