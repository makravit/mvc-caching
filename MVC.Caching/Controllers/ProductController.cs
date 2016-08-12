using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC.Caching.Models;
using System.Runtime.Caching;
using DevTrends.MvcDonutCaching;

namespace MVC.Caching.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [OutputCache(Duration = 10, VaryByParam = "none")]
        //[OutputCache(CacheProfile = "ProductsIndex")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        [OutputCache(Duration = 30, VaryByParam = "none")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await db.Products.FindAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [OutputCache(Duration = 30, VaryByParam = "productId")] // Necesary to cache one version of the page for each product 
        //[OutputCache(CacheProfile = "ProductsDetails")]
        public async Task<ActionResult> DetailsVaryByParam(int? productId)
        {
            if (productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await db.Products.FindAsync(productId);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View("Details", product);
        }

        public async Task<ActionResult> DetailsWithChildAction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await db.Products.FindAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 30, VaryByParam = "none")]
        public ActionResult ProductDetailsChild(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return PartialView("_ProductDetailsPartial", product);
        }

        [DonutOutputCache(Duration = 300, VaryByParam = "none")]
        public async Task<ActionResult> DetailsWithDonutCaching(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await db.Products.FindAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View("DetailsWithDonutCaching", product);
        }

        public async Task<ActionResult> DetailsWithMemoryCache(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = MemoryCache.Default.Get(id.ToString()) as Product;

            if (product == null)
            {
                product = await db.Products.FindAsync(id);
            }

            if (product == null)
            {
                return HttpNotFound();
            }

            MemoryCache.Default.Add(id.ToString(), product, DateTime.Now.AddMinutes(10));

            return View("Details", product);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await db.Products.FindAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await db.Products.FindAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
