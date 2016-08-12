using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVC.Caching.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Content generated on: " + DateTime.Now;

            return View();
        }

        [OutputCache(Duration = 30, VaryByParam = "none")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Content generated on: " + DateTime.Now;

            return View();
        }

        [OutputCache(Duration = 10, VaryByParam = "none")]
        public ActionResult SayHelloCachingIssue()
        {
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous";
            ViewBag.Message = "Hello " + userName + ". Content generated on: " + DateTime.Now;
            return View("About");
        }

        [OutputCache(Duration = 10, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        public ActionResult SayHello()
        {
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous";
            ViewBag.Message = "Hello " + userName + ". Content generated on: " + DateTime.Now;
            return View("About");
        }
    }
}