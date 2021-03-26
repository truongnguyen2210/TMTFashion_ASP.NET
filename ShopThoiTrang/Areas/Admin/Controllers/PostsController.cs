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
    public class PostsController : BaseController
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Posts
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var list = db.Post
                .Join(db.Topic,
                p => p.TopId,
                t => t.Id,
                (p, t) => new PostTopic
                {
                    PostId = p.Id,
                    PostImg = p.Img,
                    PostTop = t.Name,
                    PostSlug = p.Slug,
                    PostTitle = p.Title,
                    PostDetail = p.Detail,
                    PostStatus = p.Status,
                    PostDayCreat = p.Created_At
                }
                )
                .Where(m => m.PostStatus != 0).OrderByDescending(m => m.PostDayCreat);
            return View("Index", list.ToPagedList(pageNumber, pageSize));
        }
      /*  trash*/
        public ActionResult Trash(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var list = db.Post
                .Join(db.Topic,
                p => p.TopId,
                t => t.Id,
                (p, t) => new PostTopic
                {
                    PostId = p.Id,
                    PostImg = p.Img,
                    PostTop = t.Name,
                    PostSlug = p.Slug,
                    PostTitle = p.Title,
                    PostDetail = p.Detail,
                    PostStatus = p.Status,
                    PostDayCreat = p.Created_At
                }
                )
                .Where(m => m.PostStatus == 0).OrderByDescending(m => m.PostDayCreat);
            return View("Trash", list.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelPost modelPost = db.Post.Find(id);
            if (modelPost == null)
            {
                return HttpNotFound();
            }
            return View(modelPost);
        }

        // GET: Admin/Posts/Create
        public ActionResult Create()
        {
            ViewBag.ListTopId = new SelectList(db.Topic, "Id", "Name", 0);
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModelPost modelPost)
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
                            string strSlug = Mystring.str_slug(modelPost.Title);
                            String fileName = strSlug + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                            modelPost.Img = fileName;
                            String Strpath = Path.Combine(Server.MapPath("~/Public/Image/Newspaper/"), fileName);
                            file.SaveAs(Strpath);
                            modelPost.Slug = strSlug;
                            modelPost.Created_By = (int?)Session["UserIdAdmin"];
                            modelPost.Created_At = DateTime.Now;
                            db.Post.Add(modelPost);
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
            var listtopid = db.Topic.Where(m => m.Status != 0).ToList();
            ViewBag.ListTopId = new SelectList(listtopid, "Id", "Name", 0);
            return View(modelPost);
        }

        // GET: Admin/Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            var listtopid = db.Topic.Where(m => m.Status != 0).ToList();
            ViewBag.ListTopId = new SelectList(listtopid, "Id", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelPost modelPost = db.Post.Find(id);
            if (modelPost == null)
            {
                return HttpNotFound();
            }
            return View(modelPost);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelPost modelPost)
        {
            String baoloi = "";
            if (ModelState.IsValid)
            {
               /* try
                {*/
                    var file = Request.Files["img"];
                    if (file.FileName.Equals(""))
                    {
                        string strSlug = Mystring.str_slug(modelPost.Title);
                        modelPost.Slug = strSlug;
                        modelPost.Update_By = (int?)Session["UserIdAdmin"];
                        modelPost.Update_At = DateTime.Now;
                        db.Entry(modelPost).State = EntityState.Modified;
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
                            string strSlug = Mystring.str_slug(modelPost.Title);
                            String fileName = strSlug + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                            modelPost.Img = fileName;
                            String Strpath = Path.Combine(Server.MapPath("~/Public/Image/Product/"), fileName);
                            file.SaveAs(Strpath);
                            modelPost.Slug = strSlug;
                            modelPost.Update_By = (int?)Session["UserIdAdmin"];
                            modelPost.Update_At = DateTime.Now;
                            db.Entry(modelPost).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }

                    }
                /*}
                catch (Exception ex)
                {
                    baoloi += "Cập Nhật Không Thành Công";
                }*/
            }
            ViewBag.Error = baoloi;
            var listtopid = db.Topic.Where(m => m.Status != 0).ToList();
            ViewBag.ListTopId = new SelectList(listtopid, "Id", "Name", 0);
            return View(modelPost);
        }

        // GET: Admin/Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelPost modelPost = db.Post.Find(id);
            if (modelPost == null)
            {
                return HttpNotFound();
            }
            return View(modelPost);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelPost modelPost = db.Post.Find(id);
            db.Post.Remove(modelPost);
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
        //thay doi trang thai
        public ActionResult Status(int id)
        {
            ModelPost row = db.Post.Find(id);
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
            return RedirectToAction("Index", "Posts");
        }
        public ActionResult Deltrash(int id)
        {
            ModelPost modelPost = db.Post.Find(id);
            if (modelPost != null)
            {
                modelPost.Status = 0;
                db.Entry(modelPost).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Posts");
        }
        public ActionResult Restore(int id)
        {
            ModelPost modelPost = db.Post.Find(id);
            if (modelPost != null)
            {
                modelPost.Status = 1;
                db.Entry(modelPost).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Trash", "Posts");
        }
    }
}
