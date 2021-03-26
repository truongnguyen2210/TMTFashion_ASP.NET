using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Controllers
{
    public class ModuleController : Controller
    {
        ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();
        // GET: Module
        public ActionResult MainMenu()
        {
            var list = db.Menu.Where(m => m.Status == 1 && m.ParentId == 0
            && m.Position.Equals("top"))
                .OrderByDescending(m => m.Created_At).ToList();
            return View("MainMenu", list);
        }
        public ActionResult SubMainMenu(int id)
        {
            var row_m = db.Menu.Find(id);
            ViewBag.row_m = row_m;
            var list = db.Menu.Where(m => m.Status == 1 && m.ParentId == id);
            if (list.Count()>0)
            {
                //co menu con
                return View("SubMainMenu2", list.ToList());
            }
            else
            {
                //khong co menu con
                return View("SubMainMenu1");
            }
        }
        public ActionResult Slideshow()
        {
            List<ModelSlider> list = db.Slider.Where(m=>m.Status==1 && m.Position.
            Equals("slideshow")).OrderByDescending(m=>m.Created_At).ToList();
            return View("Slideshow", list);
        }
        public ActionResult Banner()
        {
            List<ModelSlider> list = db.Slider.Where(m => m.Status == 1 && m.Position.
            Equals("banner")).OrderByDescending(m => m.Created_At).Take(2).ToList();
            return View("Banner", list);
        }
        public ActionResult ListCategory()
        {
            var list = db.Categories.Where(m => m.Status == 1 && m.ParentId == 0)
               .OrderByDescending(m => m.Created_At).ToList();
            return View("ListCategory", list);
        }
        public ActionResult SubListCategory(int id)
        {
            var row_m = db.Categories.Find(id);
            ViewBag.row_cr = row_m;
            var list = db.Categories.Where(m => m.Status == 1 && m.ParentId == id);
            if (list.Count() > 0)
            {
                //co menu con
                return View("SubListCategory2", list.ToList());
            }
            else
            {
                //khong co menu con
                return View("SubListCategory1");
            }
        }
        public ActionResult listTopic()
        {
            var list = db.Topic.Where(m => m.Status == 1 && m.ParentId == 0)
               .OrderByDescending(m => m.Created_At).ToList();
            return View("listTopic", list);
        }
        public ActionResult SubListTopic(int id)
        {
            var row_t = db.Topic.Find(id);
            ViewBag.row_top = row_t;
            var list = db.Topic.Where(m => m.Status == 1 && m.ParentId == id);
            if (list.Count() > 0)
            {
                //co menu con
                return View("SubListTopic2", list.ToList());
            }
            else
            {
                //khong co menu con
                return View("SubListTopic1");
            }
        }
        public ActionResult ProductBuy()
        {
            var list = db.Product.Where(m => m.Status == 1).OrderByDescending(m => m.Created_At).Take(4).ToList();
            return View("ProductBuy", list );
        }
        public ActionResult FashionNewspaper()
        {
            var list = db.Post
                 .Join(db.Topic,
                 p => p.TopId,
                 t => t.Id,
                 (p, c) => new PostTopic
                 {
                     PostTitle = p.Title,
                     PostImg = p.Img,
                     PostId = p.Id,
                     PostSlug = p.Slug,
                     PostTop = c.Name,
                     PostStatus = p.Status,
                     PostDayCreat = p.Created_At
                 }

                 ).Where(m => m.PostStatus != 0);
            return View("FashionNewspaper", list.ToList());
        }
    }
}