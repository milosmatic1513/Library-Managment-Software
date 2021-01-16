using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClassProject.Models;

namespace ClassProject.Controllers
{
    public class discountsController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: discounts
        public ActionResult Index()
        {
            var discounts = db.discounts.Include(d => d.store);
            return View(discounts.ToList());
        }

        // GET: discounts/Details/5
        public ActionResult Details(string discounttype, string stor_id)
        {
            if (discounttype == null || stor_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            discount[] discountArray = db.discounts.Where(item => item.discounttype == discounttype && item.stor_id == stor_id).ToArray();
            if (discountArray == null || discountArray.Length == 0)
            {
                return HttpNotFound();
            }
            discount discount = discountArray[0];
            //ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name", discount.stor_id);
            return View(discount);
        }

        // GET: discounts/Create
        public ActionResult Create()
        {
            ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name");
            return View();
        }

        // POST: discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "discounttype,stor_id,lowqty,highqty,discount1")] discount discount)
        {
            if (ModelState.IsValid)
            {
                db.discounts.Add(discount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name", discount.stor_id);
            return View(discount);
        }

        // GET: discounts/Edit/5
        public ActionResult Edit(string discounttype, string stor_id)
        {
            if (discounttype == null || stor_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            discount[] discountArray = db.discounts.Where(item => item.discounttype == discounttype && item.stor_id == stor_id).ToArray();
            if (discountArray == null || discountArray.Length == 0)
            {
                return HttpNotFound();
            }
            discount discount = discountArray[0];
            ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name", discount.stor_id);
            return View(discount);
        }

        // POST: discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "discounttype,stor_id,lowqty,highqty,discount1")] discount discount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name", discount.stor_id);
            return View(discount);
        }

        // GET: discounts/Delete/5
        public ActionResult Delete(string discounttype, string stor_id)
        {
            if (discounttype == null || stor_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            discount[] discountArray = db.discounts.Where(item => item.discounttype == discounttype && item.stor_id == stor_id).ToArray();
            if (discountArray == null || discountArray.Length == 0)
            {
                return HttpNotFound();
            }
            discount discount = discountArray[0];
            //ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name", discount.stor_id);
            return View(discount);
        }

        // POST: discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string discounttype, string stor_id)
        {
            discount discount = db.discounts.Find(discounttype, stor_id);
            db.discounts.Remove(discount);
            db.SaveChanges();
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

        [AcceptVerbs("GET", "POST")]
        public JsonResult VerifyDiscountKeys(string discounttype, string stor_id)
        {
            if (db.discounts.Where(item => item.discounttype == discounttype && item.stor_id == stor_id).Count() != 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
