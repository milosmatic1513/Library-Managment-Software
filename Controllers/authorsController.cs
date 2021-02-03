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
        public ActionResult Index(string firstname, string lastname, string phone, string address, string city, string state, string zip, string orderby, string order)
        {
            List<author> authors = db.authors.ToList();

            //find all distinct states
            var states = authors.Select(s => s.state).Distinct();
            //find all distinct cities
            var cities = authors.Select(s => s.city).Distinct();

            //add to viewbag
            ViewBag.states = states;
            ViewBag.cities = cities;

            //add orderby value to viebag
            ViewBag.orderby = orderby;
            //add order value to viebag
            ViewBag.order = order;

            //Filter Starting List for each provided element	
            if (!String.IsNullOrEmpty(firstname))
            {
                ViewBag.firstname = firstname;
                authors = authors.Where(s => s.au_fname.ToLower().Contains(firstname.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(lastname))
            {
                ViewBag.lastname = lastname;
                authors = authors.Where(s => s.au_lname.ToLower().Contains(lastname.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(phone))
            {
                ViewBag.phone = phone;
                authors = authors.Where(s => s.phone.Contains(phone)).ToList();
            }
            if (!String.IsNullOrEmpty(address))
            {
                ViewBag.address = address;
                authors = authors.Where(s => s.address.ToLower().Contains(address.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(city))
            {
                ViewBag.city = city;
                authors = authors.Where(s => s.city.ToLower().Contains(city.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(state))
            {
                ViewBag.state = state;
                authors = authors.Where(s => s.state.ToLower().Contains(state.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(zip))
            {
                ViewBag.zip = zip;
                authors = authors.Where(s => s.zip.Contains(zip)).ToList();
            }

            //Ordering 
            if (orderby == "lastname")
            {
                authors = authors.OrderBy(s => s.au_lname).ToList();
               
            }
            else if (orderby == "firstname")
            {
                authors = authors.OrderBy(s => s.au_fname).ToList();
            }
            else if (orderby == "phone")
            {
                authors = authors.OrderBy(s => s.phone).ToList();
            }
            else if (orderby == "address")
            {
                authors = authors.OrderBy(s => s.address).ToList();
            }
            else if (orderby == "city")
            {
                authors = authors.OrderBy(s => s.city).ToList();
            }
            else if (orderby == "state")
            {
                authors = authors.OrderBy(s => s.state).ToList();
            }
            //check the order of the list
            if (order == "desc")
            {
                authors.Reverse();
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
        public JsonResult VerifyAuthorId(string au_id, string editMode)
        {
            if (editMode == "edit")
                return Json(true, JsonRequestBehavior.AllowGet);

            if (au_id != null && db.authors.Where(item => item.au_id == au_id).Count() != 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
