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
        public ActionResult Index(int? au_ord , int? royaltyper_from,int? royaltyper_to,string lastname,string title,string orderby,string order)
        {
            var titleauthors = db.titleauthors.Include(t => t.author).Include(t => t.title).ToList();

            //apply Filters 
            if (au_ord != null)
            {
                titleauthors=titleauthors.Where(t => t.au_ord == au_ord).ToList();
                ViewBag.au_ord = au_ord;
            }
            if (royaltyper_from != null)
            {
                titleauthors=titleauthors.Where(t => t.royaltyper >= royaltyper_from).ToList();
                ViewBag.royaltyper_from = royaltyper_from;
            }
            if (royaltyper_to != null)
            {
                titleauthors=titleauthors.Where(t => t.royaltyper <= royaltyper_to).ToList();
                ViewBag.royaltyper_to = royaltyper_to;
            }
            if (!String.IsNullOrEmpty(lastname))
            {
                titleauthors = titleauthors.Where(t => t.author.au_lname.ToLower().Contains(lastname.ToLower())).ToList();
                ViewBag.lastname = lastname;
            }
            if (!String.IsNullOrEmpty(title))
            {
                titleauthors = titleauthors.Where(t => t.title.title1.ToLower().Contains(title.ToLower())).ToList();
                ViewBag.title = title;
            }
            //Order results 
            if (orderby == "au_ord")
            {
                titleauthors = titleauthors.OrderBy(t => t.au_ord).ToList();
            }
            else if (orderby == "royaltyper")
            {
                titleauthors = titleauthors.OrderBy(t => t.royaltyper).ToList();
            }
            else if (orderby == "lastname")
            {
                titleauthors = titleauthors.OrderBy(t => t.author.au_lname).ToList();
            }
            else if (orderby == "title")
            {
                titleauthors = titleauthors.OrderBy(t => t.title.title1).ToList();
            }
            if (order == "desc")
            {
                titleauthors.Reverse();
            }
            //add order values to viewbags

            ViewBag.order = order;
            ViewBag.orderby = orderby;

            return View(titleauthors);
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
            return View();
        }

        // POST: titleauthors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "au_id,title_id,au_ord,royaltyper")] titleauthor titleauthor)
        {
            if (ModelState.IsValid)
            {
                db.titleauthors.Add(titleauthor);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            return View(titleauthor);
        }

        // POST: titleauthors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "au_id,title_id,au_ord,royaltyper")] titleauthor titleauthor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(titleauthor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            return View(titleauthor);
        }

        // POST: titleauthors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string au_id, string title_id)
        {
            titleauthor titleauthor = db.titleauthors.Find(au_id, title_id);
            titleauthor.Delete(db);
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
    }
}
