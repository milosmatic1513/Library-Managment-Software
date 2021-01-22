using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClassProject.Models;

namespace ClassProject.Controllers
{
    public class pub_infoController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: pub_info
        public ActionResult Index()
        {
            var pub_info = db.pub_info.Include(p => p.publisher);
            return View(pub_info.ToList());
        }

        // GET: pub_info/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pub_info pub_info = db.pub_info.Find(id);
            if (pub_info == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(pub_info);
        }

        // GET: pub_info/Create
        public ActionResult Create(string pub_id)
        {
            if (pub_id != null)
                ViewBag.pub_id = new SelectList(db.publishers.Where(item => item.pub_id == pub_id), "pub_id", "pub_name");
            else
            {
                var publishers = db.publishers.Where(item => item.pub_info == null);
                ViewBag.pub_id = new SelectList(publishers, "pub_id", "pub_name");
            }

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View();
        }

        // POST: pub_info/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pub_id,logo,pr_info")] pub_info pub_info, HttpPostedFileBase imageFile, string referUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    pub_info.logo = GetImageBytes(imageFile);
                }
                db.pub_info.Add(pub_info);
                db.SaveChanges();

                return RedirectUrl(referUrl);
            }

            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", pub_info.pub_id);
            return View(pub_info);
        }

        // GET: pub_info/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pub_info pub_info = db.pub_info.Find(id);
            if (pub_info == null)
            {
                return HttpNotFound();
            }
            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", pub_info.pub_id);

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(pub_info);
        }

        // POST: pub_info/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pub_id,logo,pr_info")] pub_info pub_info, HttpPostedFileBase imageFile, string referUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    pub_info.logo = GetImageBytes(imageFile);
                }
                db.Entry(pub_info).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectUrl(referUrl);
            }
            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", pub_info.pub_id);
            return View(pub_info);
        }

        // GET: pub_info/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pub_info pub_info = db.pub_info.Find(id);
            if (pub_info == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReferUrl = ReferenceUrl.ReferUrl(Request);
            return View(pub_info);
        }

        // POST: pub_info/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string referUrl)
        {
            pub_info pub_info = db.pub_info.Find(id);
            pub_info.Delete(db);
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

        private byte[] GetImageBytes(HttpPostedFileBase file)
        {
            if (file == null)
                return null;

            byte[] fileBytes = null;
            using (Stream fileStream = file.InputStream)
            {
                var mStreamer = new MemoryStream();
                mStreamer.SetLength(fileStream.Length);
                fileStream.Read(mStreamer.GetBuffer(), 0, (int)fileStream.Length);
                mStreamer.Seek(0, SeekOrigin.Begin);
                fileBytes = mStreamer.GetBuffer();
            }
            return fileBytes;
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
