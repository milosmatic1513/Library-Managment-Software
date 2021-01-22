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
    public class titleauthorsController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: titleauthors
        public ActionResult Index()
        {
            var titleauthors = db.titleauthors.Include(t => t.author).Include(t => t.title);
            return View(titleauthors.ToList());
        }

        // GET: titleauthors/Details/5
        public ActionResult Details(string au_id, string title_id)
        {
            if (au_id == null || title_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            titleauthor titleauthor = db.titleauthors.Find(au_id, title_id);
            if (titleauthor == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(titleauthor);
        }

        // GET: titleauthors/Create
        public ActionResult Create(string au_id)
        {
            if (au_id != null)
                ViewBag.au_id = new SelectList(db.authors.Where(item => item.au_id == au_id), "au_id", "au_lname");
            else
                ViewBag.au_id = new SelectList(db.authors, "au_id", "au_lname");

            ViewBag.title_id = new SelectList(db.titles, "title_id", "title1");

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View();
        }

        // POST: titleauthors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "au_id,title_id,au_ord,royaltyper")] titleauthor titleauthor, string referUrl)
        {
            if (ModelState.IsValid)
            {
                db.titleauthors.Add(titleauthor);
                db.SaveChanges();

                return RedirectUrl(referUrl);
            }

            ViewBag.au_id = new SelectList(db.authors, "au_id", "au_lname", titleauthor.au_id);
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title1", titleauthor.title_id);
            return View(titleauthor);
        }

        // GET: titleauthors/Edit/5
        public ActionResult Edit(string au_id, string title_id)
        {
            if (au_id == null || title_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            titleauthor titleauthor = db.titleauthors.Find(au_id, title_id);
            if (titleauthor == null)
            {
                return HttpNotFound();
            }
            ViewBag.au_id = new SelectList(db.authors, "au_id", "au_lname", titleauthor.au_id);
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title1", titleauthor.title_id);

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(titleauthor);
        }

        // POST: titleauthors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "au_id,title_id,au_ord,royaltyper")] titleauthor titleauthor, string referUrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(titleauthor).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectUrl(referUrl);
            }
            ViewBag.au_id = new SelectList(db.authors, "au_id", "au_lname", titleauthor.au_id);
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title1", titleauthor.title_id);
            return View(titleauthor);
        }

        // GET: titleauthors/Delete/5
        public ActionResult Delete(string au_id, string title_id)
        {
            if (au_id == null || title_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            titleauthor titleauthor = db.titleauthors.Find(au_id, title_id);
            if (titleauthor == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(titleauthor);
        }

        // POST: titleauthors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string au_id, string title_id, string referUrl)
        {
            titleauthor titleauthor = db.titleauthors.Find(au_id, title_id);
            titleauthor.Delete(db);
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
        public JsonResult VerifyTitleAuthorId(string au_id, string title_id, string editMode)
        {
            if (editMode == "edit")
                return Json(true, JsonRequestBehavior.AllowGet);

            if (db.titleauthors.Where(item => item.au_id == au_id && item.title_id == title_id).Count() != 0)
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
