using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang.Models;
using ShopThoiTrang.Library;
using PagedList;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Categories/index
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Title = "Danh Sách Loại Sản Phẩm";
            //db.Categories.ToList()=>> select * from categories
            var model = db.Categories
                .Where(m=>m.Status!=0)
                .OrderByDescending(m=>m.Created_At)
                .ToPagedList(pageNumber, pageSize);
            return View("index", model);
        }
        // GET: Admin/Categories/index
        public ActionResult Trash(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Title = "Danh Sách Loại Sản Phẩm";
            //db.Categories.ToList()=>> select * from categories
            var model = db.Categories
                .Where(m => m.Status == 0)
                .OrderByDescending(m => m.Created_At)
                .ToPagedList(pageNumber, pageSize);
            return View("Trash", model);
        }
        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelCategories modelCategories = db.Categories.Find(id);
            if (modelCategories == null)
            {
                return HttpNotFound();
            }
            return View(modelCategories);
        }
        [HttpGet]
        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList( db.Categories.Where(m => m.Status != 0).ToList(), "Id","Name", 0);
            ViewBag.ListOrder = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Order", "Name", 0);
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModelCategories modelCategories)
        {
            ViewBag.ListCat = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Orders", "Name", 0);
            String baoloi = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (modelCategories.ParentId == null)
                    {
                        modelCategories.ParentId = 0;
                    }
                    string slug = Mystring.str_slug(modelCategories.Name);
                    modelCategories.Orders += 1;
                    modelCategories.Slug = slug;
                    modelCategories.Created_By = (int?)Session["UserIdAdmin"];
                    modelCategories.Created_At = DateTime.Now;
                    modelCategories.Update_By = 1;
                    modelCategories.Update_At = DateTime.Now;
                    db.Categories.Add(modelCategories);
                    //them du lieu bang link
                    ModelLink modelLink = new ModelLink();
                    modelLink.Name = modelCategories.Name;
                    modelLink.Slug = modelCategories.Slug;
                    modelLink.TableId = modelCategories.Id;
                    modelLink.Type = "category";
                    db.Links.Add(modelLink);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    baoloi = "thêm không thành công";
                }
            }
            ViewBag.Error = baoloi;
            return View(modelCategories);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelCategories modelCategories = db.Categories.Find(id);
            if (modelCategories == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCat = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Order", "Name", 0);
            return View(modelCategories);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelCategories modelCategories)
        {
            String baoloi = "";
            if (ModelState.IsValid)
            {
                try
                {
                    modelCategories.Update_By = (int?)Session["UserIdAdmin"];
                    modelCategories.Update_At = DateTime.Now;
                    db.Entry(modelCategories).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    baoloi += "Khong Thanh Cong";
                }
                
            }
            ViewBag.Error = baoloi;
            return View(modelCategories);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelCategories modelCategories = db.Categories.Find(id);
            if (modelCategories == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCat = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(db.Categories.Where(m => m.Status != 0).ToList(), "Order", "Name", 0);
            return View(modelCategories);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelCategories modelCategories = db.Categories.Find(id);
            db.Categories.Remove(modelCategories);
            db.SaveChanges();
            return RedirectToAction("Trash", "Categories");
        }

        public ActionResult Status(int id)
        {
            ModelCategories modelCategories = db.Categories.Find(id);
            if (modelCategories != null)
            {
                modelCategories.Status = (modelCategories.Status == 1) ? 2 : 1;
                db.Entry(modelCategories).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Categories");
        }
        public ActionResult Deltrash(int id)
        {
            ModelCategories modelCategories = db.Categories.Find(id);
            if (modelCategories != null)
            {
                modelCategories.Status = 0;
                db.Entry(modelCategories).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Categories");
        }
        public ActionResult Restore(int id)
        {
            ModelCategories modelCategories = db.Categories.Find(id);
            if (modelCategories != null)
            {
                modelCategories.Status = 1;
                db.Entry(modelCategories).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Trash", "Categories");
        }
    }
}
