using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class LinksController : BaseController
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Links
        public ActionResult Index()
        {
            return View(db.Links.ToList());
        }

        // GET: Admin/Links/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelLink modelLink = db.Links.Find(id);
            if (modelLink == null)
            {
                return HttpNotFound();
            }
            return View(modelLink);
        }

        // GET: Admin/Links/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Links/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Slug,Type,TableId")] ModelLink modelLink)
        {
            if (ModelState.IsValid)
            {
                db.Links.Add(modelLink);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modelLink);
        }

        // GET: Admin/Links/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelLink modelLink = db.Links.Find(id);
            if (modelLink == null)
            {
                return HttpNotFound();
            }
            return View(modelLink);
        }

        // POST: Admin/Links/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Slug,Type,TableId")] ModelLink modelLink)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelLink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modelLink);
        }

        // GET: Admin/Links/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelLink modelLink = db.Links.Find(id);
            if (modelLink == null)
            {
                return HttpNotFound();
            }
            return View(modelLink);
        }

        // POST: Admin/Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelLink modelLink = db.Links.Find(id);
            db.Links.Remove(modelLink);
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
