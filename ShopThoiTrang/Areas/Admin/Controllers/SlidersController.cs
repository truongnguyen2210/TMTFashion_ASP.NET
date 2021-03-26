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
    public class SlidersController : Controller
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Sliders
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var list = db.Slider.Where(m => m.Status != 0).OrderByDescending(m=>m.Created_At);
            return View("index", list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Trash(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var list = db.Slider.Where(m => m.Status == 0).OrderByDescending(m=>m.Created_At);
            return View("Trash",list.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Sliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelSlider modelSlider = db.Slider.Find(id);
            if (modelSlider == null)
            {
                return HttpNotFound();
            }
            return View(modelSlider);
        }

        // GET: Admin/Sliders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModelSlider modelSlider)
        {
            string baoloi = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var file = Request.Files["img"];
                    if (file.FileName.Equals(""))
                    {
                        baoloi += "Hình Chưa Được Chọn";
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
                            string strName = Mystring.str_slug(modelSlider.Name);
                            String fileName = strName + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                            modelSlider.Img = fileName;
                            String Strpath = Path.Combine(Server.MapPath("~/Public/Image/Slider"), fileName);
                            file.SaveAs(Strpath);
                            modelSlider.Created_By = (int?)Session["UserIdAdmin"];
                            modelSlider.Created_At = DateTime.Now;
                            modelSlider.Update_By = 1;
                            modelSlider.Update_At = DateTime.Now;
                            db.Slider.Add(modelSlider);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }

                    }

                }
                catch (Exception ex)
                {
                    baoloi += "Thêm Không Thành Công";
                }
            }
            ViewBag.Error = baoloi;        
            return View(modelSlider);
        }

        // GET: Admin/Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelSlider modelSlider = db.Slider.Find(id);
            if (modelSlider == null)
            {
                return HttpNotFound();
            }
            return View(modelSlider);
        }

        // POST: Admin/Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelSlider modelSlider)
        {
            string baoloi = "";
            if (ModelState.IsValid)
            {
                string strName = Mystring.str_slug(modelSlider.Name);
                var file = Request.Files["img"];
                try
                {
                    if(file.FileName.Equals(""))
                    {  
                        modelSlider.Update_At = DateTime.Now;
                        modelSlider.Update_By = (int?)Session["UserIdAdmin"];
                        db.Entry(modelSlider).State = EntityState.Modified;
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
                            String fileName = strName + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                            modelSlider.Img = fileName;
                            String Strpath = Path.Combine(Server.MapPath("~/Public/Image/Slider"), fileName);
                            file.SaveAs(Strpath);
                            modelSlider.Update_At = DateTime.Now;
                            modelSlider.Update_By = (int?)Session["UserIdAdmin"];
                            db.Entry(modelSlider).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    } 
                }
                catch(Exception ex)
                {
                    baoloi = "Thêm Không Thành Công";
                }
            }
            ViewBag.Error = baoloi;
            return View(modelSlider);
        }

        // GET: Admin/Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelSlider modelSlider = db.Slider.Find(id);
            if (modelSlider == null)
            {
                return HttpNotFound();
            }
            return View(modelSlider);
        }

        // POST: Admin/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelSlider modelSlider = db.Slider.Find(id);
            db.Slider.Remove(modelSlider);
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
            ModelSlider row = db.Slider.Find(id);
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
            return RedirectToAction("Index", "Sliders");
        }
        //thay doi trang thai
        public ActionResult Deltrash(int id)
        {
            ModelSlider modelSliders = db.Slider.Find(id);
            if (modelSliders != null)
            {
                modelSliders.Status = 0;
                db.Entry(modelSliders).State = EntityState.Modified;
                db.SaveChanges();
            }          
            return RedirectToAction("Index", "Sliders");
        }
        public ActionResult Restore(int id)
        {
            ModelSlider modelSliders = db.Slider.Find(id);
            if (modelSliders != null)
            {
                modelSliders.Status = 1;
                db.Entry(modelSliders).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Trash", "Sliders");
        }
    }
}
