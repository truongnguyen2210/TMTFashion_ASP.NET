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
    public class OrderDetailsController : BaseController
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/OrderDetails
        public ActionResult Index()
        {
            return View(db.OrderDetail.ToList());
        }

        // GET: Admin/OrderDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelOrderDetail modelOrderDetail = db.OrderDetail.Find(id);
            if (modelOrderDetail == null)
            {
                return HttpNotFound();
            }
            return View(modelOrderDetail);
        }

        // GET: Admin/OrderDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderId,ProductId,Price,Quantity,Amount")] ModelOrderDetail modelOrderDetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetail.Add(modelOrderDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modelOrderDetail);
        }

        // GET: Admin/OrderDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelOrderDetail modelOrderDetail = db.OrderDetail.Find(id);
            if (modelOrderDetail == null)
            {
                return HttpNotFound();
            }
            return View(modelOrderDetail);
        }

        // POST: Admin/OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderId,ProductId,Price,Quantity,Amount")] ModelOrderDetail modelOrderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelOrderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modelOrderDetail);
        }

        // GET: Admin/OrderDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelOrderDetail modelOrderDetail = db.OrderDetail.Find(id);
            if (modelOrderDetail == null)
            {
                return HttpNotFound();
            }
            return View(modelOrderDetail);
        }

        // POST: Admin/OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelOrderDetail modelOrderDetail = db.OrderDetail.Find(id);
            db.OrderDetail.Remove(modelOrderDetail);
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
