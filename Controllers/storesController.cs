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
    public class storesController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: stores
        public ActionResult Index(string store,string address,string state,string city,string zip,string orderby ,string order)
        {
            var stores = db.stores.ToList();
          
            //Get all cities 
            var cities = stores.Select(s => s.city).Distinct().ToList();
            //Get all states
            var states = stores.Select(s => s.state).Distinct().ToList();

            //add values to viewbag
            ViewBag.cities = cities;
            ViewBag.states = states;

            if (!String.IsNullOrEmpty(store))
            {
                stores = stores.Where(c => c.stor_name.ToLower().Contains(store.ToLower())).ToList();
                ViewBag.store = store;
            }
            if (!String.IsNullOrEmpty(address))
            {
                stores = stores.Where(c => c.stor_address.ToLower().Contains(address.ToLower())).ToList();
                ViewBag.address = address;
            }
            if (!String.IsNullOrEmpty(state))
            {
                stores = stores.Where(c => c.state.Contains(state)).ToList();
                ViewBag.state = state;
            }
            if (!String.IsNullOrEmpty(city))
            {
                stores = stores.Where(c => c.city.Contains(city)).ToList();
                ViewBag.city = city;
            }
            if (zip!=null)
            {
                stores = stores.Where(c => c.zip.Contains(zip)).ToList();
                ViewBag.zip = zip;
            }

            if (orderby == "store")
            {
                stores = stores.OrderBy(s => s.stor_name).ToList();
            }
            else if (orderby == "state")
            {
                stores = stores.OrderBy(s => s.state).ToList();
            }
            else if (orderby == "city")
            {
                stores = stores.OrderBy(s => s.city).ToList();
            }
            else if (orderby == "zip")
            {
                stores = stores.OrderBy(s => s.zip).ToList();
            }
            if (order =="desc")
            {
                stores.Reverse();
            }

            ViewBag.orderby = orderby;
            ViewBag.order = order;
            return View(stores);
        }

        // GET: stores/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            store store = db.stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // GET: stores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: stores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "stor_id,stor_name,stor_address,city,state,zip")] store store)
        {
            if (ModelState.IsValid)
            {
                db.stores.Add(store);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(store);
        }

        // GET: stores/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            store store = db.stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // POST: stores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "stor_id,stor_name,stor_address,city,state,zip")] store store)
        {
            if (ModelState.IsValid)
            {
                db.Entry(store).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(store);
        }

        // GET: stores/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            store store = db.stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // POST: stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            store store = db.stores.Find(id);
            store.Delete(db);
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
        public JsonResult VerifyStoreId(string stor_id, string editMode)
        {
            if (editMode == "edit")
                return Json(true, JsonRequestBehavior.AllowGet);

            if (stor_id != null && db.stores.Where(item => item.stor_id == stor_id).Count() != 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
