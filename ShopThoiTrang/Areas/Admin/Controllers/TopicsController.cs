using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopThoiTrang.Library;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class TopicsController : BaseController
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Topics
        public ActionResult Index(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var list = db.Topic.Where(m => m.Status != 0).OrderByDescending(m => m.Created_At);
            return View("Index", list.ToPagedList(pageNumber, pageSize));
        }
        //
        public ActionResult Trash(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Title = "Danh Sách Loại Tin Tạm Xóa";
            //db.Categories.ToList()=>> select * from categories
            var model = db.Topic
                .Where(m => m.Status == 0)
                .OrderByDescending(m => m.Created_At)
                .ToPagedList(pageNumber, pageSize);
            return View("Trash", model);
        }
        // GET: Admin/Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelTopic modelTopic = db.Topic.Find(id);
            if (modelTopic == null)
            {
                return HttpNotFound();
            }
            return View(modelTopic);
        }

        // GET: Admin/Topics/Create
        public ActionResult Create()
        {
            ViewBag.listParent = new SelectList(db.Topic.Where(m => m.Status != 0 && m.ParentId == 0).ToList(), "Id", "Name", 0);
            return View();
        }

        // POST: Admin/Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModelTopic modelTopic)
        {
            ViewBag.listParent = new SelectList(db.Topic.Where(m => m.Status != 0 && m.ParentId == 0).ToList(), "Id", "Name", 0);
            String baoloi = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (modelTopic.ParentId==null)
                    {
                        modelTopic.ParentId = 0;
                    }
                    string slug = Mystring.str_slug(modelTopic.Name);
                    modelTopic.Slug = slug;
                    modelTopic.Created_At = DateTime.Now;
                    modelTopic.Created_By = (int?)Session["UserIdAdmin"];
                    db.Topic.Add(modelTopic);
                    //them du lieu bang link
                    ModelLink modelLink = new ModelLink();
                    modelLink.Name = modelTopic.Name;
                    modelLink.Slug = modelTopic.Slug;
                    modelLink.TableId = modelTopic.Id;
                    modelLink.Type = "topic";
                    db.Links.Add(modelLink);

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    baoloi += "Thêm Không Thành Công";
                }
            }
            ViewBag.Error = baoloi;
            return View(modelTopic);
        }

        // GET: Admin/Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelTopic modelTopic = db.Topic.Find(id);
            if (modelTopic == null)
            {
                return HttpNotFound();
            }
            ViewBag.listParent = new SelectList(db.Topic.Where(m => m.Status != 0 && m.ParentId == 0).ToList(), "Id", "Name", 0);
            return View(modelTopic);
        }

        // POST: Admin/Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelTopic modelTopic)
        {
            String baoloi = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (modelTopic.ParentId == null)
                {
                    modelTopic.ParentId = 0;
                }
                modelTopic.Update_At = DateTime.Now;
                    modelTopic.Update_By = (int?)Session["UserIdAdmin"];
                    db.Entry(modelTopic).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    baoloi += "Không Thành Công";
                }
            }
            ViewBag.Error = baoloi;
            return View(modelTopic);
        }

        // GET: Admin/Topics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelTopic modelTopic = db.Topic.Find(id);
            if (modelTopic == null)
            {
                return HttpNotFound();
            }
            ViewBag.listParent = new SelectList(db.Topic.Where(m => m.Status != 0 && m.ParentId == 0).ToList(), "Id", "Name", 0);
            return View(modelTopic);
        }

        // POST: Admin/Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelTopic modelTopic = db.Topic.Find(id);
            db.Topic.Remove(modelTopic);
            db.SaveChanges();
            return RedirectToAction("Trash", "Topics");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Status(int id)
        {
            ModelTopic modelTopic = db.Topic.Find(id);
            if (modelTopic != null)
            {
                modelTopic.Status = (modelTopic.Status == 1) ? 2 : 1;
                db.Entry(modelTopic).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Topics");
        }
        public ActionResult Deltrash(int id)
        {
            ModelTopic modelTopic = db.Topic.Find(id);
            if (modelTopic != null)
            {
                modelTopic.Status = 0;
                db.Entry(modelTopic).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Topics");
        }
        public ActionResult Restore(int id)
        {
            ModelTopic modelTopic = db.Topic.Find(id);
            if (modelTopic != null)
            {
                modelTopic.Status = 1;
                db.Entry(modelTopic).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Trash", "Topics");
        }
    }
}
