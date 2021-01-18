using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ClassProject.Models;

namespace ClassProject.Controllers
{
    public class authorsController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: authors
        public ActionResult Index(String Firstname, String Lastname, String Phone, String Address, String City, String State, String Zip)
        {
            List<author> authors = db.authors.ToList();

            //Filter Starting List for each provided element	
            if (!String.IsNullOrEmpty(Firstname))
            {
                ViewBag.Firstname = Firstname;
                authors = authors.Where(s => s.au_fname.Contains(Firstname)).ToList();
            }
            if (!String.IsNullOrEmpty(Lastname))
            {
                ViewBag.Lastname = Lastname;
                authors = authors.Where(s => s.au_lname.Contains(Lastname)).ToList();
            }
            if (!String.IsNullOrEmpty(Phone))
            {
                ViewBag.Phone = Phone;
                authors = authors.Where(s => s.phone.Contains(Phone)).ToList();
            }
            if (!String.IsNullOrEmpty(Address))
            {
                ViewBag.Address = Address;
                authors = authors.Where(s => s.address.Contains(Address)).ToList();
            }
            if (!String.IsNullOrEmpty(City))
            {
                ViewBag.City = City;
                authors = authors.Where(s => s.city.Contains(City)).ToList();
            }
            if (!String.IsNullOrEmpty(Zip))
            {
                ViewBag.Zip = Zip;
                authors = authors.Where(s => s.zip.Contains(Zip)).ToList();
            }
            return View(authors);
        }

        // GET: authors/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            author author = db.authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "au_id,au_lname,au_fname,phone,address,city,state,zip,contract")] author author)
        {
            if (ModelState.IsValid)
            {
                db.authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: authors/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            author author = db.authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "au_id,au_lname,au_fname,phone,address,city,state,zip,contract")] author author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: authors/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            author author = db.authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            author author = db.authors.Find(id);
            author.Delete(db);
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
        public JsonResult VerifyAuthorId(string au_id)
        {
            if (au_id != null && db.authors.Where(item => item.au_id == au_id).Count() != 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
