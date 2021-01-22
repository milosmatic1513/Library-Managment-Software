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
    public class salesController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: sales
        public ActionResult Index()
        {
            var sales = db.sales.Include(s => s.store).Include(s => s.title);
            return View(sales.ToList());
        }

        // GET: sales/Details/5
        public ActionResult Details(string stor_id, string ord_num, string title_id)
        {
            if (stor_id == null || ord_num == null || title_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sale sale = db.sales.Find(stor_id, ord_num, title_id);
            if (sale == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(sale);
        }

        // GET: sales/Create
        public ActionResult Create(string stor_id, string title_id)
        {
            if (stor_id != null)
                ViewBag.stor_id = new SelectList(db.stores.Where(item => item.stor_id == stor_id), "stor_id", "stor_name");
            else
                ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name");

            if (title_id != null)
                ViewBag.title_id = new SelectList(db.titles.Where(item => item.title_id == title_id), "title_id", "title1");
            else
                ViewBag.title_id = new SelectList(db.titles, "title_id", "title1");

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View();
        }

        // POST: sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "stor_id,ord_num,ord_date,qty,payterms,title_id")] sale sale, string referUrl)
        {
            if (ModelState.IsValid)
            {
                db.sales.Add(sale);
                db.SaveChanges();

                return RedirectUrl(referUrl);
            }

            ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name", sale.stor_id);
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title1", sale.title_id);
            return View(sale);
        }

        // GET: sales/Edit/5
        public ActionResult Edit(string stor_id, string ord_num, string title_id)
        {
            if (stor_id == null || ord_num == null || title_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sale sale = db.sales.Find(stor_id, ord_num, title_id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name", sale.stor_id);
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title1", sale.title_id);

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(sale);
        }

        // POST: sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "stor_id,ord_num,ord_date,qty,payterms,title_id")] sale sale, string referUrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectUrl(referUrl);
            }
            ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name", sale.stor_id);
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title1", sale.title_id);
            return View(sale);
        }

        // GET: sales/Delete/5
        public ActionResult Delete(string stor_id, string ord_num, string title_id)
        {
            if (stor_id == null || ord_num == null || title_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sale sale = db.sales.Find(stor_id, ord_num, title_id);
            if (sale == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(sale);
        }

        // POST: sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string stor_id, string ord_num, string title_id, string referUrl)
        {
            sale sale = db.sales.Find(stor_id, ord_num, title_id);
            sale.Delete(db);
            db.SaveChanges();

            return RedirectUrl(referUrl);
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
        public JsonResult VerifySaleKeys(string stor_id, string ord_num, string title_id, string editMode)
        {
            if (editMode == "edit")
                return Json(true, JsonRequestBehavior.AllowGet);

            if (db.sales.Find(stor_id, ord_num, title_id) != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private ActionResult RedirectUrl(string referUrl)
        {
            if (string.IsNullOrWhiteSpace(referUrl))
                return RedirectToAction("Index"); // if referUrl is missing redirect to Index
            else
                return Redirect(referUrl); // else redirect to referUrl
        }
    }
}
