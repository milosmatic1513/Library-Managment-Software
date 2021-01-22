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
    public class employeesController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: employees
        public ActionResult Index(String firstname,String minit, String lastname)
        {
            List<employee> employees = db.employees.Include(e => e.job).Include(e => e.publisher).ToList();
            //Add Selections to Viewbag	
            var minitList = employees.Select(s => s.minit).Distinct();

            ViewBag.minitList = minitList;

            //Apply Filters 	
            if (!String.IsNullOrEmpty(firstname))
            {
                employees = employees.Where(s => s.fname.Contains(firstname)).ToList();
            }
            if (!String.IsNullOrEmpty(minit))
            {
                employees = employees.Where(s => s.minit.Contains(minit)).ToList();
            }
            if (!String.IsNullOrEmpty(lastname))
            {
                employees = employees.Where(s => s.lname.Contains(lastname)).ToList();
            }
            return View(employees.ToList());
        }

        // GET: employees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(employee);
        }

        // GET: employees/Create
        public ActionResult Create(string pub_id)
        {
            ViewBag.job_id = new SelectList(db.jobs, "job_id", "job_desc");

            if (pub_id != null)
                ViewBag.pub_id = new SelectList(db.publishers.Where(item => item.pub_id == pub_id), "pub_id", "pub_name");
            else
                ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name");

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View();
        }

        // POST: employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "emp_id,fname,minit,lname,job_id,job_lvl,pub_id,hire_date")] employee employee, string referUrl)
        {
            if (ModelState.IsValid)
            {
                db.employees.Add(employee);
                db.SaveChanges();

                return RedirectUrl(referUrl);
            }

            ViewBag.job_id = new SelectList(db.jobs, "job_id", "job_desc", employee.job_id);
            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", employee.pub_id);
            return View(employee);
        }

        // GET: employees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.job_id = new SelectList(db.jobs, "job_id", "job_desc", employee.job_id);
            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", employee.pub_id);

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(employee);
        }

        // POST: employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "emp_id,fname,minit,lname,job_id,job_lvl,pub_id,hire_date")] employee employee, string referUrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectUrl(referUrl);
            }
            ViewBag.job_id = new SelectList(db.jobs, "job_id", "job_desc", employee.job_id);
            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", employee.pub_id);
            return View(employee);
        }

        // GET: employees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(employee);
        }

        // POST: employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string referUrl)
        {
            employee employee = db.employees.Find(id);
            employee.Delete(db);
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
        public JsonResult VerifyEmployeeId(string emp_id, string editMode)
        {
            if (editMode == "edit")
                return Json(true, JsonRequestBehavior.AllowGet);

            if (emp_id != null && db.employees.Where(item => item.emp_id == emp_id).Count() != 0)
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
