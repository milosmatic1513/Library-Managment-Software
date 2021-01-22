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
        public ActionResult Index(string discounttype,string storename,string discount_from,string discount_to,string lowqty,string highqty,string orderby)
        {
           
            //set a list of available discounts
            var discounts = db.discounts.Include(d => d.store).ToList();
            //set a list of all available  stores
            var stores = discounts.Select(s => s.store.stor_name).Distinct();
            //set a list of all available discount types 
            var discounttypes = discounts.Select(s => s.discounttype).Distinct();

            //add The lists to the Viewbag
            ViewBag.stores = stores;
            ViewBag.discounttypes = discounttypes;
            //add orderby value to viebag
            ViewBag.orderby = orderby;
            //Apply Filters 	
            if (!String.IsNullOrEmpty(discounttype))
            {
                discounts = discounts.Where(s => s.discounttype.Contains(discounttype)).ToList();
                ViewBag.discounttype = discounttype;
            }
            if (!String.IsNullOrEmpty(storename))
            {
                discounts = discounts.Where(s => s.store.stor_name.Contains(storename)).ToList();
                ViewBag.storename = storename;
            }
            if (!String.IsNullOrEmpty(discount_from) || !String.IsNullOrEmpty(discount_to))
            {
                if (!String.IsNullOrEmpty(discount_from))
                {
                    discounts = discounts.Where(s => s.discount1 >= Int32.Parse(discount_from)).ToList();
                }
                else if (!String.IsNullOrEmpty(discount_to))
                {
                    discounts = discounts.Where(s => s.discount1 <= Int32.Parse(discount_to)).ToList();
                }
              
                ViewBag.discount_from = discount_from;
                ViewBag.discount_to = discount_to;
            }
            if (!String.IsNullOrEmpty(lowqty))
            {
                discounts = discounts.Where(s => s.lowqty>=Int32.Parse(lowqty)).ToList();
                ViewBag.lowqty = lowqty;
            }
            if (!String.IsNullOrEmpty(highqty))
            {
                discounts = discounts.Where(s => s.highqty <= Int32.Parse(highqty)).ToList();
                ViewBag.highqty = highqty;
            }
      
            //Ordering 
            if (orderby == "discounttype")
            {
                discounts = discounts.OrderBy(s => s.discounttype).ToList();
            }
            else if(orderby == "storename")
            {
                discounts = discounts.OrderBy(s => s.store.stor_name).ToList();
            }  
            else if (orderby == "discount")
            {
                discounts = discounts.OrderBy(s => s.discount1).ToList();
            }
            else if (orderby == "highqty")
            {
                discounts = discounts.OrderBy(s => s.highqty).ToList();
            }
            else if (orderby == "lowqty")
            {
                discounts = discounts.OrderBy(s => s.lowqty).ToList();
            }
           
            return View(discounts);
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
            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(discount);
        }

        // GET: discounts/Create
        public ActionResult Create(string stor_id)
        {
            if (stor_id != null)
                ViewBag.stor_id = new SelectList(db.stores.Where(item => item.stor_id == stor_id), "stor_id", "stor_name");
            else
                ViewBag.stor_id = new SelectList(db.stores, "stor_id", "stor_name");

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View();
        }

        // POST: discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "discounttype,stor_id,lowqty,highqty,discount1")] discount discount, string referUrl)
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

                return RedirectUrl(referUrl);
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

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(discount);
        }

        // POST: discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "discounttype,stor_id,lowqty,highqty,discount1")] discount discount, string discounttypeCurrent, string stor_idCurrent, string referUrl)
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

                return RedirectUrl(referUrl);
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
            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(discount);
        }

        // POST: discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string discounttype, string stor_id, string referUrl)
        {
            discount discount = db.discounts.Where(item => item.discounttype == discounttype && item.stor_id == stor_id).First();
            discount.Delete(db);

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
