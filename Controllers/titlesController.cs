﻿using System;
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
    public class titlesController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: titles
        public ActionResult Index()
        {
            var titles = db.titles.Include(t => t.publisher).Include(t => t.roysched);
            return View(titles.ToList());
        }

        // GET: titles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            title title = db.titles.Find(id);
            if (title == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(title);
        }

        // GET: titles/Create
        public ActionResult Create(string pub_id)
        {
            if (pub_id != null)
                ViewBag.pub_id = new SelectList(db.publishers.Where(item => item.pub_id == pub_id), "pub_id", "pub_name");
            else
                ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name");

            ViewBag.title_id = new SelectList(db.royscheds, "title_id", "title_id");

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View();
        }

        // POST: titles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "title_id,title1,type,pub_id,price,advance,royalty,ytd_sales,notes,pubdate")] title title, string referUrl)
        {
            if (ModelState.IsValid)
            {
                db.titles.Add(title);
                db.SaveChanges();

                return RedirectUrl(referUrl);
            }

            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", title.pub_id);
            ViewBag.title_id = new SelectList(db.royscheds, "title_id", "title_id", title.title_id);
            return View(title);
        }

        // GET: titles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            title title = db.titles.Find(id);
            if (title == null)
            {
                return HttpNotFound();
            }
            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", title.pub_id);
            ViewBag.title_id = new SelectList(db.royscheds, "title_id", "title_id", title.title_id);

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(title);
        }

        // POST: titles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "title_id,title1,type,pub_id,price,advance,royalty,ytd_sales,notes,pubdate")] title title, string referUrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(title).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectUrl(referUrl);
            }
            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", title.pub_id);
            ViewBag.title_id = new SelectList(db.royscheds, "title_id", "title_id", title.title_id);
            return View(title);
        }

        // GET: titles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            title title = db.titles.Find(id);
            if (title == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(title);
        }

        // POST: titles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string referUrl)
        {
            title title = db.titles.Find(id);
            title.Delete(db);
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
        public JsonResult VerifyTitleId(string title_id, string editMode)
        {
            if (editMode == "edit")
                return Json(true, JsonRequestBehavior.AllowGet);

            if (title_id != null && db.titles.Where(item => item.title_id == title_id).Count() != 0)
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
