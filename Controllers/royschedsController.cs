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
    public class royschedsController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: royscheds
        public ActionResult Index(string title,int? low_range_from , int? low_range_to , int? high_range_from, int? high_range_to, int? royalty_from, int? royalty_to,string orderby,bool? asc,string order)
        {
            var royscheds = db.royscheds.Include(r => r.title).ToList();
            ViewBag.titles = royscheds.Select(r => r.title.title1).Distinct().ToList();
            ViewBag.orderby = orderby;
            ViewBag.order = order;
            if (!String.IsNullOrEmpty(title))
            {
                royscheds = royscheds.Where(r => r.title.title1.Contains(title)).ToList();
                ViewBag.title = title;
            }
            if (low_range_from != null)
            {
                royscheds = royscheds.Where(r => r.lorange >= low_range_from).ToList();
                ViewBag.low_range_from = low_range_from;
            }
            if (low_range_to != null)
            {
                royscheds = royscheds.Where(r => r.lorange <= low_range_to).ToList();
                ViewBag.low_range_to = low_range_to;
            }
            if (high_range_from != null)
            {
                royscheds = royscheds.Where(r => r.hirange >= high_range_from).ToList();
                ViewBag.high_range_from = high_range_from;
            }
            if (high_range_to != null)
            {
                royscheds = royscheds.Where(r => r.hirange <= high_range_to).ToList();
                ViewBag.high_range_to = high_range_to;
            }
            if (royalty_from != null)
            {
                royscheds = royscheds.Where(r => r.royalty >= royalty_from).ToList();
                ViewBag.royalty_from = royalty_from;
            }
            if (royalty_to != null)
            {
                royscheds = royscheds.Where(r => r.royalty <= royalty_to).ToList();
                ViewBag.royalty_to = royalty_to;
            }
          
            if (orderby == "title")
            {
                royscheds = royscheds.OrderBy(s => s.title.title1).ToList();
            }
            else if (orderby == "low_range")
            {
                royscheds = royscheds.OrderBy(s => s.lorange).ToList();
            }
            else if(orderby == "high_range")
            {
                royscheds = royscheds.OrderBy(s => s.hirange).ToList();
            }
            else if (orderby == "royalty")
            {
                royscheds = royscheds.OrderBy(s => s.royalty).ToList();
            }


            if (!String.IsNullOrEmpty(order))
            {
                if (order == "desc")
                {
                    royscheds.Reverse();
                }
            }
            return View(royscheds);
        }

        // GET: royscheds/Details/5
        public ActionResult Details(string title_id, int? lorange, int? hirange)
        {
            if (title_id == null || lorange == null || hirange == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            roysched[] royschedArray = db.royscheds.Where(item => item.title_id == title_id && item.lorange == lorange && item.hirange == hirange).ToArray();
            if (royschedArray.Count() == 0)
            {
                return HttpNotFound();
            }
            return View(royschedArray[0]);
        }

        // GET: royscheds/Create
        public ActionResult Create(string title_id)
        {
            if (title_id != null)
                ViewBag.title_id = new SelectList(db.titles.Where(item => item.title_id == title_id), "title_id", "title1");
            else
                ViewBag.title_id = new SelectList(db.titles, "title_id", "title1");

            return View();
        }

        // POST: royscheds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "title_id,lorange,hirange,royalty")] roysched roysched)
        {
            if (ModelState.IsValid)
            {
                // Insert directly into to DB since there is no pk for the model to assign values to
                db.Database.ExecuteSqlCommand("INSERT INTO roysched VALUES ( @title_id, @lorange, @hirange, @royalty )",
                    new SqlParameter("@title_id", roysched.title_id),
                    new SqlParameter("@lorange", roysched.lorange),
                    new SqlParameter("@hirange", roysched.hirange),
                    new SqlParameter("@royalty", (object)roysched.royalty ?? DBNull.Value) // Send value or null
                    );
                return RedirectToAction("Index");
            }
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title1", roysched.title_id);
            return View(roysched);
        }

        // GET: royscheds/Edit/5
        public ActionResult Edit(string title_id, int? lorange, int? hirange)
        {
            if (title_id == null || lorange == null || hirange == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            roysched[] royschedArray = db.royscheds.Where(item => item.title_id == title_id && item.lorange == lorange && item.hirange == hirange).ToArray();
            if (royschedArray.Count() == 0)
            {
                return HttpNotFound();
            }
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title1", royschedArray[0].title_id);
            return View(royschedArray[0]);
        }

        // POST: royscheds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "title_id,lorange,hirange,royalty")] roysched roysched, string title_idCurrent, int? lorangeCurrent, int? hirangeCurrent)
        {
            if (ModelState.IsValid && title_idCurrent != null && lorangeCurrent != null && hirangeCurrent != null)
            {
                // Update directly into to DB since there is no pk for the model to assign values to
                db.Database.ExecuteSqlCommand("UPDATE roysched SET title_id = @title_id, lorange = @lorange, hirange = @hirange, royalty = @royalty WHERE title_id = @title_idCurrent and lorange = @lorangeCurrent and hirange = @hirangeCurrent",
                    new SqlParameter("@title_id", roysched.title_id),
                    new SqlParameter("@lorange", roysched.lorange),
                    new SqlParameter("@hirange", roysched.hirange),
                    new SqlParameter("@royalty", (object)roysched.royalty ?? DBNull.Value), // Send value or null
                    new SqlParameter("@title_idCurrent", title_idCurrent),
                    new SqlParameter("@lorangeCurrent", lorangeCurrent),
                    new SqlParameter("@hirangeCurrent", hirangeCurrent)
                    );
                return RedirectToAction("Index");
            }
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title1", roysched.title_id);
            return View(roysched);
        }

        // GET: royscheds/Delete/5
        public ActionResult Delete(string title_id, int? lorange, int? hirange)
        {
            if (title_id == null || lorange == null || hirange == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            roysched[] royschedArray = db.royscheds.Where(item => item.title_id == title_id && item.lorange == lorange && item.hirange == hirange).ToArray();
            if (royschedArray.Count() == 0)
            {
                return HttpNotFound();
            }
            return View(royschedArray[0]);
        }

        // POST: royscheds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string title_id, int? lorange, int? hirange)
        {
            roysched roysched = db.royscheds.Where(item => item.title_id == title_id && item.lorange == lorange && item.hirange == hirange).First();
            roysched.Delete(db);
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
        public JsonResult VerifyRoychedKeys(string title_id, Nullable<int> lorange, Nullable<int> hirange, string editMode)
        {
            if (editMode == "edit")
                return Json(true, JsonRequestBehavior.AllowGet);

            if (db.royscheds.Where(item => item.title_id == title_id && item.lorange == lorange && item.hirange == hirange).Count() != 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
