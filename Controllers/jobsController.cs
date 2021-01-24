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
    public class jobsController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: jobs
        public ActionResult Index(string job_desc,string min_lvl_from,string min_lvl_to,string max_lvl_from,string max_lvl_to )
        {

            var jobs = db.jobs.ToList();
            var job_descs = jobs.Select(j => j.job_desc).Distinct().ToList();

            ViewBag.job_descs = job_descs;
            if (!String.IsNullOrEmpty(job_desc))
            {
                jobs = jobs.Where(j => j.job_desc.Contains(job_desc)).ToList();
                ViewBag.job_desc = job_desc;
            }
            if (!String.IsNullOrEmpty(min_lvl_from))
            {
                jobs=jobs.Where(j=>j.min_lvl>=Int32.Parse(min_lvl_from)).ToList();
                ViewBag.min_lvl_from = min_lvl_from;
            }
            if (!String.IsNullOrEmpty(min_lvl_to))
            {
                jobs = jobs.Where(j => j.min_lvl <= Int32.Parse(min_lvl_to)).ToList();
                ViewBag.min_lvl_to = min_lvl_to;
            }
            if (!String.IsNullOrEmpty(max_lvl_from))
            {
                jobs = jobs.Where(j => j.max_lvl >= Int32.Parse(max_lvl_from)).ToList();
                ViewBag.max_lvl_from = max_lvl_from;
            }
            if (!String.IsNullOrEmpty(max_lvl_to))
            {
                jobs = jobs.Where(j => j.max_lvl <= Int32.Parse(max_lvl_to)).ToList();
                ViewBag.max_lvl_to = max_lvl_to;
            }
            return View(jobs);
        }

        // GET: jobs/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: jobs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "job_id,job_desc,min_lvl,max_lvl")] job job)
        {
            if (ModelState.IsValid)
            {
                db.jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(job);
        }

        // GET: jobs/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "job_id,job_desc,min_lvl,max_lvl")] job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        // GET: jobs/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            job job = db.jobs.Find(id);
            job.Delete(db);
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
