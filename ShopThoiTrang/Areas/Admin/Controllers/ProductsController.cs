using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopThoiTrang.Library;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class ProductsController : BaseController
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Products
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var list = db.Product
                .Join(db.Categories,
                p => p.CatId,
                c => c.Id,
                (p, c) => new ProductCategory
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductImg = p.Img,
                    ProductStatus = p.Status,
                    CategoryName = c.Name
                }

                ).Where(m => m.ProductStatus != 0).OrderByDescending(m=>m.ProductId);
            return View("Index", list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Trash(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var list = db.Product
                .Join(db.Categories,
                p => p.CatId,
                c => c.Id,
                (p, c) => new ProductCategory
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductImg = p.Img,
                    ProductStatus = p.Status,
                    CategoryName = c.Name
                }

                ).Where(m => m.ProductStatus == 0).OrderByDescending(m => m.ProductId);
            return View("Trash", list.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelProducts modelProducts = db.Product.Find(id);
            if (modelProducts == null)
            {
                return HttpNotFound();
            }
            return View(modelProducts);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.ListCatId = new SelectList(db.Categories, "Id", "Name", 0);
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModelProducts modelProducts)
        {
            String baoloi = "";
            if (ModelState.IsValid)
            {                
                try
                {
                    var file = Request.Files["img"];
                    if (file == null)
                    {
                        ModelState.AddModelError("HINHANH", "Hình Chưa Được Chọn");
                    }
                    else
                    {
                        string[] FileExtentsions = new string[] { ".jpg", ".gif", ".png" };
                        if (!FileExtentsions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                        {
                            ModelState.AddModelError("HINHANH", "Kiểu Tập Tin " + string.Join(", ", FileExtentsions) + " Không Cho Phép!");
                        }
                        else
                        {
                            string strSlug = Mystring.str_slug(modelProducts.Name);
                            String fileName = strSlug + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                            modelProducts.Img = fileName;
                            String Strpath = Path.Combine(Server.MapPath("~/Public/Image/Product/"), fileName);
                            file.SaveAs(Strpath);
                            modelProducts.Slug = strSlug;
                            modelProducts.Created_By = (int ?) Session["UserIdAdmin"];
                            modelProducts.Created_At = DateTime.Now;
                            db.Product.Add(modelProducts);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }

                    }

                }
                catch (Exception ex)
                {
                    baoloi+="Thêm Không Thành Công";
                }

            }
            ViewBag.Error = baoloi;
            var listcatid = db.Categories.Where(m => m.Status != 0).ToList();
            ViewBag.ListCatId = new SelectList(listcatid, "Id", "Name", 0);
            return View(modelProducts);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            var listcatid = db.Categories.Where(m => m.Status != 0).ToList();
            ViewBag.ListCatId = new SelectList(listcatid, "Id", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelProducts modelProducts = db.Product.Find(id);
            if (modelProducts == null)
            {
                return HttpNotFound();
            }
            return View(modelProducts);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelProducts modelProducts)
        {
            string baoloi = "";
            if (ModelState.IsValid)
            {
                try
                {
                    string strSlug = Mystring.str_slug(modelProducts.Name);
                    var file = Request.Files["img"];
                if (file.FileName.Equals(""))
                {
                   
                    modelProducts.Slug = strSlug;
                    modelProducts.Update_By = (int?)Session["UserIdAdmin"];
                    modelProducts.Update_At = DateTime.Now;
                    db.Entry(modelProducts).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    string[] FileExtentsions = new string[] { ".jpg", ".gif", ".png" };
                    if (!FileExtentsions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        baoloi += "Kiểu Tập Tin " + string.Join(", ", FileExtentsions) + " Không Cho Phép!";
                    }
                    else
                    {
                        String fileName = strSlug + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                        modelProducts.Img = fileName;
                        String Strpath = Path.Combine(Server.MapPath("~/Public/Image/Product/"), fileName);
                        file.SaveAs(Strpath);
                        modelProducts.Slug = strSlug;
                        modelProducts.Update_By = (int?)Session["UserIdAdmin"];
                        modelProducts.Update_At = DateTime.Now;
                        db.Entry(modelProducts).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
                }
                catch (Exception ex)
                {
                    baoloi += "Cập Nhật Không Thành Công";
                }
            }
            var listcatid = db.Categories.Where(m => m.Status != 0).ToList();
            ViewBag.ListCatId = new SelectList(listcatid, "Id", "Name", 0);
            ViewBag.Error = baoloi;
            return View(modelProducts);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            var listcatid = db.Categories.Where(m => m.Status != 0).ToList();
            ViewBag.ListCatId = new SelectList(listcatid, "Id", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelProducts modelProducts = db.Product.Find(id);
            if (modelProducts == null)
            {
                return HttpNotFound();
            }
            return View(modelProducts);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelProducts modelProducts = db.Product.Find(id);
            db.Product.Remove(modelProducts);
            db.SaveChanges();
            return RedirectToAction("Trash");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //thay doi trang thai
        public ActionResult Status(int id)
        {
            ModelProducts row = db.Product.Find(id);
            if (row != null)
            {
                row.Status = (row.Status == 1) ? 2 : 1;
                db.Entry(row).State = EntityState.Modified;
                db.SaveChanges();
                TempData["thongbao"] = new XMessage("success", "Thành Công");
            }
            else
            {
                TempData["thongbao"] = new XMessage("dange", "Thất Bại");
            }
            return RedirectToAction("Index", "Products");
        }
        public ActionResult Deltrash(int id)
        {
            ModelProducts modelProducts = db.Product.Find(id);
            if (modelProducts != null)
            {
                modelProducts.Status = 0;
                db.Entry(modelProducts).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Products");
        }
        public ActionResult Restore(int id)
        {
            ModelProducts modelProducts = db.Product.Find(id);
            if (modelProducts != null)
            {
                modelProducts.Status = 1;
                db.Entry(modelProducts).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Trash", "Products");
        }
    }
}
