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
    public class OrdersController : BaseController
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            return View(db.Order.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelOrder modelOrder = db.Order.Find(id);
            if (modelOrder == null)
            {
                return HttpNotFound();
            }
            return View(modelOrder);
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,UserId,CreateDate,ExportDate,DeliveryAddress,DeliveryName,DeliveryPhone,DeliveryEmail,Update_At,Update_By,Status")] ModelOrder modelOrder)
        {
            if (ModelState.IsValid)
            {
                db.Order.Add(modelOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modelOrder);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelOrder modelOrder = db.Order.Find(id);
            if (modelOrder == null)
            {
                return HttpNotFound();
            }
            return View(modelOrder);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,UserId,CreateDate,ExportDate,DeliveryAddress,DeliveryName,DeliveryPhone,DeliveryEmail,Update_At,Update_By,Status")] ModelOrder modelOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modelOrder);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelOrder modelOrder = db.Order.Find(id);
            if (modelOrder == null)
            {
                return HttpNotFound();
            }
            return View(modelOrder);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelOrder modelOrder = db.Order.Find(id);
            db.Order.Remove(modelOrder);
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
