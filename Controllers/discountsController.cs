using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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
                // Insert directly into to DB since there is no pk for the model to assign values to
                db.Database.ExecuteSqlCommand("INSERT INTO discounts VALUES ( @discounttype, @stor_id, @lowqty, @highqty, @discount1 )", 
                    new SqlParameter("@discounttype", discount.discounttype),
                    new SqlParameter("@stor_id", discount.stor_id),
                    new SqlParameter("@lowqty", (object)discount.lowqty ?? DBNull.Value), // Send value or null
                    new SqlParameter("@highqty", (object)discount.highqty ?? DBNull.Value), // Send value or null
                    new SqlParameter("@discount1", discount.discount1)
                    );
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
        public ActionResult Edit([Bind(Include = "discounttype,stor_id,lowqty,highqty,discount1")] discount discount, string discounttypeCurrent, string stor_idCurrent)
        {
            if (ModelState.IsValid && discounttypeCurrent != null && stor_idCurrent != null)
            {
                // Update directly into to DB since there is no pk for the model to assign values to
                db.Database.ExecuteSqlCommand("UPDATE discounts SET discounttype = @discounttype, stor_id = @stor_id, lowqty = @lowqty, highqty = @highqty, discount = @discount1 WHERE discounttype = @discounttypeCurrent and stor_id = @stor_idCurrent",
                    new SqlParameter("@discounttype", discount.discounttype),
                    new SqlParameter("@stor_id", discount.stor_id),
                    new SqlParameter("@lowqty", (object)discount.lowqty ?? DBNull.Value), // Send value or null
                    new SqlParameter("@highqty", (object)discount.highqty ?? DBNull.Value), // Send value or null
                    new SqlParameter("@discount1", discount.discount1),
                    new SqlParameter("@discounttypeCurrent", discounttypeCurrent),
                    new SqlParameter("@stor_idCurrent", stor_idCurrent)
                    );
                return RedirectToAction("Index");
            }
            //ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name", discount.stor_id);
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
            if (db.discounts.Where(item => item.discounttype == discounttype && item.stor_id == stor_id).Count() == 1)
            {
                // Delete the entry
                db.Database.ExecuteSqlCommand("DELETE FROM discounts WHERE discounttype = @discounttype and stor_id = @stor_id",
                    new SqlParameter("@discounttype", discounttype),
                    new SqlParameter("@stor_id", stor_id)
                    );
            }
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
        public JsonResult VerifyDiscountKeys(string discounttype, string stor_id, string editMode)
        {
            if (editMode == "edit")
                return Json(true, JsonRequestBehavior.AllowGet);

            if (db.discounts.Where(item => item.discounttype == discounttype && item.stor_id == stor_id).Count() != 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
