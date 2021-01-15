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
    public class publishersController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: publishers
        public ActionResult Index()
        {
            var publishers = db.publishers.Include(p => p.pub_info);
            return View(publishers.ToList());
        }

        // GET: publishers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publisher publisher = db.publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // GET: publishers/Create
        public ActionResult Create()
        {
            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info");
            return View();
        }

        // POST: publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pub_id,pub_name,city,state,country")] publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.publishers.Add(publisher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info", publisher.pub_id);
            return View(publisher);
        }

        // GET: publishers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publisher publisher = db.publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info", publisher.pub_id);
            return View(publisher);
        }

        // POST: publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pub_id,pub_name,city,state,country")] publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publisher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info", publisher.pub_id);
            return View(publisher);
        }

        // GET: publishers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publisher publisher = db.publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            publisher publisher = db.publishers.Find(id);
            db.publishers.Remove(publisher);
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
    }
}
