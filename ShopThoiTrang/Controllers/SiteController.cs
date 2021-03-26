using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang.Models;
using PagedList;
namespace ShopThoiTrang.Controllers
{
    public class SiteController : Controller
    {
        ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();
        // GET: Site
        private int? page;
        public ActionResult Index(string slug = null)
        {
            if(slug==null)
            {
                //trang chu
                return this.Home();
            }
            else
            {
                ModelLink rowlink = db.Links.Where(m => m.Slug == slug).FirstOrDefault();
                if (rowlink != null)
                {
                    //co slug trong link
                    string typeslug = rowlink.Type;
                    switch (typeslug)
                    {
                        case "category": return this.ProductCategory(slug, page);
                        case "topic": return this.PostTopic(slug, page);
                        case "page": return this.PostPage(slug);
                        default: return this.Error404(slug);
                    }

                }
                else
                {
                    //khoong co
                    int countproduct = db.Product.Where(m => m.Slug == slug && m.Status == 1).Count();
                    if (countproduct != 0)
                    {
                        return this.ProductDetail(slug);
                    }
                    else
                    {
                        int countpost = db.Post.Where(m => m.Slug == slug &&
                        m.Status == 1 && m.Type == "post").Count();
                        if (countpost != 0)
                        {
                            return this.PostDetail(slug);
                        }
                        else
                        {
                            return this.Error404(slug);
                        }
                    }
                }
            }  
        }

       /* private ActionResult ProductCategory(string slug, int? page)
        {
            throw new NotImplementedException();
        }*/

        //trang chu
        public ActionResult Home()
        {
            var listcat = db.Categories.Where(m => m.Status == 1 && m.ParentId == 0).ToList();
            return View("Home", listcat);
        }
        public ActionResult ProductHome(int catid, string namecat)
        {
            List<int> listcatid = db.Categories.Where(m => m.Status == 1 && m.ParentId == catid)
                .Select(m => m.Id).ToList();
            List<int> listcat1;
            foreach (var row in db.Categories.Where(m => m.Status == 1 && m.ParentId == catid).ToList())
            {
                listcat1 = db.Categories.Where(m => m.Status == 1 && m.ParentId == row.Id)
                .Select(m => m.Id).ToList();
                listcatid.AddRange(listcat1);
            }
            listcatid.Add(catid);
            ViewBag.NameCat = namecat;
            var listproduct = db.Product.Where(m => m.Status == 1 && listcatid.Contains(m.CatId))
                .OrderByDescending(m => m.Created_At).Take(6).ToList();
            return View("ProductHome", listproduct);
        }
        public ActionResult Products(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page??1);
            var list = db.Product.Where(m => m.Status == 1).OrderByDescending(m => m.Created_At);
            return View("Products", list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ProductCategory(string slug, int? page)
        {
            int pageSize = 18;
            int pageNumber = (page ?? 1);
            int cat1 = 0;
            int cat2 = 0;
            ViewBag.row_c = db.Categories.Where(m => m.Status != 0 && m.Slug == slug).ToList();
            foreach(var cat in ViewBag.row_c)
            {
                cat1 = cat.ParentId;
                cat2 = cat.Id;
            }
            if(cat1.Equals(0))
            {
                ViewBag.row_c2 = db.Categories.Where(m => m.Status != 0 && m.ParentId == cat2).ToList();
                foreach (var cat in ViewBag.row_c2)
                {
                    cat1 = cat.ParentId;
                }
                var list1 = db.Product.Where(m => m.Status != 0 && (m.CatId == cat1 || m.CatId == cat2)).OrderByDescending(m=>m.Created_At);
                return View("ProductCategory", list1.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var list2 = db.Product.Where(m => m.Status != 0 && m.CatId == cat2).OrderByDescending(m => m.Created_At);
                return View("ProductCategory", list2.ToPagedList(pageNumber, pageSize));
            }          
        }
        public ActionResult ProductDetail(string slug)
        {
            int cat = 0;
           ViewBag.row_p = db.Product.Where(m => m.Slug == slug && m.Status !=0);
            foreach(var row in ViewBag.row_p)
            {
                cat = row.CatId;
            }
            var list = db.Product.Where(m => m.CatId == cat && m.Status !=0).OrderByDescending(m=>m.Created_At).Take(6).ToList();
            return View("ProductDetail", list);
        }
        public ActionResult Newpapers(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            ViewBag.row_oneposnew = db.Post.Where(m => m.Status != 0 && m.Type == "post").OrderByDescending(m=>m.Created_At).Take(1).ToList();
            var list = db.Post.Where(m => m.Status != 0 && m.Type == "post").OrderByDescending(m=>m.Created_At).ToPagedList(pageNumber, pageSize);
            return View("Newpapers", list);
        }
        public ActionResult PostTopic(string slug, int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            int topid = 0;
            ViewBag.row_t = db.Topic.Where(m => m.Status != 0 && m.Slug == slug).ToList();
            foreach (var top in ViewBag.row_t)
            {
                topid = top.Id;
            }
            ViewBag.row_oneposnew = db.Post.Where(m => m.Status != 0 && m.TopId == topid && m.Type == "post").OrderByDescending(m => m.Created_At).Take(1).ToList();
            var list = db.Post.Where(m => m.Status != 0 && m.TopId == topid).OrderByDescending(m => m.Created_At);
            return View("PostTopic", list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult PostPage(string slug)
        {
            var list = db.Post.Where(m => m.Status != 0 && m.Slug == slug).ToList();
            return View("PostPage", list);
        }
        public ActionResult PostDetail(string slug)
        {
            int top = 0;
            ViewBag.row_pos = db.Post.
                Where(m => m.Status != 0 && m.Slug == slug).ToList();
            foreach(var row in ViewBag.row_pos)
            {
                top = row.TopId;
            }
            var list = db.Post.Where(m => m.Status != 0 && m.TopId == top).OrderByDescending(m=>m.Created_At).Take(6).ToList();
            return View("PostDetail", list);
        }
        public ActionResult Error404(string slug)
        {
            return View("Error404");
        }
    }
}