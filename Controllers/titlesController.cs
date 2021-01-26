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
    public class titlesController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: titles
        public ActionResult Index(string title , string type,int? price_from ,int? price_to,int? advance_from , int? advance_to,int? royalty_from,int? royalty_to,int? sales_from,int? sales_to,string date_from,string date_to,string publisher,string orderby,string order)
        {
            var titles = db.titles.Include(t => t.publisher).Include(t => t.roysched).ToList();
            //add distinct types to a list
            var types = titles.Select(t => t.type).Distinct().ToList();
            //add values to ViewBag
            ViewBag.types = types;
            //apply filters
            if (!String.IsNullOrEmpty(title))
            {
                titles = titles.Where(t => t.title1.ToLower().Contains(title.ToLower())).ToList();
                ViewBag.title = title;
            }
            if (!String.IsNullOrEmpty(type))
            {
                titles = titles.Where(t => t.type.ToLower().Contains(type.ToLower())).ToList();
                ViewBag.type = type;
            }
            if (price_from != null)
            {
                titles = titles.Where(t => t.price >= price_from).ToList();
                ViewBag.price_from = price_from;
            }
            if (price_to != null)
            {
                titles = titles.Where(t => t.price <= price_to).ToList();
                ViewBag.price_to = price_to;
            }
            if (advance_from != null)
            {
                titles = titles.Where(t => t.advance >= advance_from).ToList();
                ViewBag.advance_from = advance_from;
            }
            if (advance_to != null)
            {
                titles = titles.Where(t => t.advance <= advance_to).ToList();
                ViewBag.advance_to = advance_to;
            }
            if (royalty_from != null)
            {
                titles = titles.Where(t => t.royalty >= royalty_from).ToList();
                ViewBag.royalty_from = royalty_from;
            }
            if (royalty_to != null)
            {
                titles = titles.Where(t => t.royalty <= royalty_to).ToList();
                ViewBag.royalty_to = royalty_to;
            }
            if (sales_from != null)
            {
                titles = titles.Where(t => t.ytd_sales >= sales_from).ToList();
                ViewBag.sales_from = sales_from;
            }
            if (sales_to != null)
            {
                titles = titles.Where(t => t.ytd_sales <= sales_to).ToList();
                ViewBag.sales_to = sales_to;
            }
            if (!String.IsNullOrEmpty(date_from))
            {
                titles = titles.Where(t => DateTime.Compare(t.pubdate,DateTime.ParseExact(date_from, "yyyy-MM-dd", null)) >= 0).ToList();
                ViewBag.date_from = date_from;
            }
            if (!String.IsNullOrEmpty(date_to))
            {
                titles = titles.Where(t => DateTime.Compare(t.pubdate, DateTime.ParseExact(date_to, "yyyy-MM-dd", null)) <= 0).ToList();
                ViewBag.date_to = date_to;
            }
            if (!String.IsNullOrEmpty(publisher))
            {
                titles = titles.Where(t => t.publisher.pub_name.ToLower().Contains(publisher.ToLower())).ToList();
                ViewBag.date_to = date_to;
            }
            //Order the list
            if (orderby == "title")
            {
                titles = titles.OrderBy(t=>t.title1).ToList();

            }
            else if (orderby == "price")
            {
                titles = titles.OrderBy(t => t.price).ToList();
            }
            else if (orderby == "advance")
            {
                titles = titles.OrderBy(t => t.advance).ToList();
            }
            else if (orderby == "royalty")
            {
                titles = titles.OrderBy(t => t.royalty).ToList();
            }
            else if (orderby == "sales")
            {
                titles = titles.OrderBy(t => t.ytd_sales).ToList();
            }
            else if (orderby == "date")
            {
                titles = titles.OrderBy(t => t.pubdate).ToList();

            }

            if (order == "desc")
            {
                titles.Reverse();
            }

            //add order values to viewbag
            ViewBag.order = order;
            ViewBag.orderby = orderby;



            return View(titles);
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
