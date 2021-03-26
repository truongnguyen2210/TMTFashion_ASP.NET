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
    public class UsersController : BaseController
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Users
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.tieude = "Danh Sách Thành Viên";
            var list = db.Users.Where(m => m.Status != 0 && m.Access != null).OrderByDescending(m=>m.Created_At);
            return View("Index", list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Customer(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.tieude = "Danh Sách Khách Hàng";
            var list = db.Users.ToList().Where(m => m.Status != 0 && m.Access == null).OrderByDescending(m=>m.Created_At);
            return View("Customer", list.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Users/Details/5
        public ActionResult Trash(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.tieude = "Danh Sách Thành Viên Bị Khóa Tài Khoản";
            var list = db.Users.ToList().Where(m => m.Status == 0 && m.Access != null).OrderByDescending(m=>m.Created_At);
            return View("Trash", list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult TrashCustomer(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.tieude = "Danh Sách Khách Hàng Bị Khóa Tài Khoản";
            var list = db.Users.ToList().Where(m => m.Status == 0 && m.Access == null).OrderByDescending(m=>m.Created_At);
            return View("TrashCustomer", list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelUser modelUser = db.Users.Find(id);
            if (modelUser == null)
            {
                return HttpNotFound();
            }
            return View(modelUser);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModelUser modelUser)
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
                            string strName = Mystring.str_slug(modelUser.FullName);
                            String fileName = strName + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                            modelUser.Img = fileName;
                            String Strpath = Path.Combine(Server.MapPath("~/Public/Image/User"), fileName);
                            file.SaveAs(Strpath);
                            string matkhau = Mystring.ToMD5(modelUser.PassWord);
                            modelUser.PassWord = matkhau;
                            modelUser.Created_By = (int?)Session["UserIdAdmin"];
                            modelUser.Created_At = DateTime.Now;
                            modelUser.Update_By = 1;
                            modelUser.Update_At = DateTime.Now;
                            db.Users.Add(modelUser);
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
            return View(modelUser);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelUser modelUser = db.Users.Find(id);
            if (modelUser == null)
            {
                return HttpNotFound();
            }
            return View(modelUser);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelUser modelUser)
        {
            String baoloi = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var file = Request.Files["img"];
                    if (!file.FileName.Equals(""))
                    {
                        modelUser.Update_By = (int?)Session["UserIdAdmin"];
                        modelUser.Update_At = DateTime.Now;
                        db.Entry(modelUser).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["FullNameAdmin"] = modelUser.FullName;
                        int id = modelUser.Id;
                        Session["UserIdAdmin"] = id;
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
                            string strName = Mystring.str_slug(modelUser.FullName);
                            String fileName = strName + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                            modelUser.Img = fileName;
                            String Strpath = Path.Combine(Server.MapPath("~/Public/Image/User"), fileName);
                            file.SaveAs(Strpath);
                            modelUser.Update_By = (int?)Session["UserIdAdmin"];
                            modelUser.Update_At = DateTime.Now;
                            db.Entry(modelUser).State = EntityState.Modified;
                            db.SaveChanges();
                            Session["FullNameAdmin"] = modelUser.FullName;                         
                            Session["UserIdAdmin"] = modelUser.Id;
                            Session["ImgAdmin"] = modelUser.Img;
                            return RedirectToAction("Index");
                        }

                    }
                }
                catch(Exception ex)
                {
                    baoloi += "Không Thành Công";
                }
            }
            ViewBag.Error = baoloi;
            return View(modelUser);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelUser modelUser = db.Users.Find(id);
            if (modelUser.Access == null)
            {
                @ViewBag.tieude = "Xóa Khách Hàng"; 
            }
            else
            {
                @ViewBag.tieude = "Xóa Thành Viên";
            }
            return View(modelUser);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelUser modelUser = db.Users.Find(id);
            db.Users.Remove(modelUser);
            db.SaveChanges();
            if(modelUser.Access==null)
            {
                 return RedirectToAction("TrashCustomer");
            }
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
           ModelUser row = db.Users.Find(id);
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
            if(row.Access==null)
            {
                return RedirectToAction("Customer");
            }
            return RedirectToAction("Index", "Users");
        }
        //thay doi trang thai
        public ActionResult Deltrash(int id)
        {
            ModelUser modelUsers = db.Users.Find(id);
            if (modelUsers != null)
            {
                modelUsers.Status = 0;
                db.Entry(modelUsers).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (modelUsers.Access == null)
            {
                return RedirectToAction("Customer");
            }
            return RedirectToAction("Index", "Users");
        }
        public ActionResult Restore(int id)
        {
            ModelUser modelUsers = db.Users.Find(id);
            if (modelUsers != null)
            {
                modelUsers.Status = 1;
                db.Entry(modelUsers).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (modelUsers.Access == null)
            {
                return RedirectToAction("TrashCustomer");
            }
            return RedirectToAction("Trash", "Users");
        }
    }
}
