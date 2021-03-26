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
    public class MenusController : Controller
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Menus
        public ActionResult Index(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var list = db.Menu.Where(m => m.Status != 0).OrderByDescending(m => m.Created_At);
            return View("Index", list.ToPagedList(pageNumber, pageSize));
        }
        //trash
        public ActionResult Trash(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var list = db.Menu.Where(m => m.Status == 0).OrderByDescending(m => m.Created_At);
            return View("Trash", list.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Menus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelMenu modelMenu = db.Menu.Find(id);
            if (modelMenu == null)
            {
                return HttpNotFound();
            }
            return View(modelMenu);
        }

        // GET: Admin/Menus/Create
        public ActionResult Create()
        {
            ViewBag.ListParent = new SelectList(db.Menu.Where(m => m.Status != 0 && m.ParentId == 0 && m.Type == "category").ToList(), "Id", "Name", 0);
            return View();
        }

        // POST: Admin/Menus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModelMenu modelMenu)
        {
            String baoloi = "";
            if (ModelState.IsValid)
            {
               
                try
                {
                    if (modelMenu.ParentId == null)
                    {
                        modelMenu.ParentId = 0;
                    }
                    string link = Mystring.str_slug(modelMenu.Name);
                    modelMenu.Orders += 1;
                    modelMenu.Link = link;
                    modelMenu.Created_By = (int?)Session["UserIdAdmin"];
                    modelMenu.Created_At = DateTime.Now;
                    db.Menu.Add(modelMenu);
                    //them du lieu bang link
                    ModelLink modelLink = new ModelLink();
                    modelLink.Name = modelMenu.Name;
                    modelLink.Slug = link;
                    modelLink.TableId = modelMenu.Id;
                    modelLink.Type = modelMenu.Type;
                    db.Links.Add(modelLink);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }catch(Exception ex)
                {
                    baoloi += "khong thanh cong";
                }   
            }
            ViewBag.ListParent = new SelectList(db.Menu.Where(m => m.Status != 0 && m.ParentId == 0 && m.Type == "category").ToList(), "Id", "Name", 0);
            ViewBag.Error = baoloi;
            return View(modelMenu);
        }

        // GET: Admin/Menus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelMenu modelMenu = db.Menu.Find(id);
            if (modelMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListParent = new SelectList(db.Menu.Where(m => m.Status != 0 && m.ParentId == 0 && m.Type == "category").ToList(), "Id", "Name", 0);
            return View(modelMenu);
        }

        // POST: Admin/Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelMenu modelMenu)
        {
            String baoloi = "";
            if (ModelState.IsValid)
            {
               /* try
                {*/
                    if(modelMenu.ParentId == null)
                    {
                        modelMenu.ParentId = 0;
                    }
                    modelMenu.Update_At = DateTime.Now;
                    modelMenu.Update_By = (int?)Session["UserIdAdmin"];
                    db.Entry(modelMenu).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
               /* }
                catch(Exception ex)
                {
                    baoloi += "khong thanh cong";
                }  */    
            }
            ViewBag.Error = baoloi;
            ViewBag.ListParent = new SelectList(db.Menu.Where(m => m.Status != 0 && m.ParentId == 0 && m.Type == "category").ToList(), "Id", "Name", 0);
            return View(modelMenu);
        }

        // GET: Admin/Menus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelMenu modelMenu = db.Menu.Find(id);
            if (modelMenu == null)
            {
                return HttpNotFound();
            }
            return View(modelMenu);
        }

        // POST: Admin/Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelMenu modelMenu = db.Menu.Find(id);
            db.Menu.Remove(modelMenu);
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

        public ActionResult Status(int id)
        {
            ModelMenu modelMenu = db.Menu.Find(id);
            if (modelMenu != null)
            {
                modelMenu.Status = (modelMenu.Status == 1) ? 2 : 1;
                db.Entry(modelMenu).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Menus");
        }
        public ActionResult Deltrash(int id)
        {
            ModelMenu modelMenu = db.Menu.Find(id);
            if (modelMenu != null)
            {
                modelMenu.Status = 0;
                db.Entry(modelMenu).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Menus");
        }
        public ActionResult Restore(int id)
        {
            ModelMenu modelMenu = db.Menu.Find(id);
            if (modelMenu != null)
            {
                modelMenu.Status = 1;
                db.Entry(modelMenu).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Trash", "Menus");
        }
    }
}
