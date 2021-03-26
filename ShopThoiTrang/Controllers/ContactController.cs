using ShopThoiTrang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Controllers
{   
    public class ContactController : Controller
    {
        ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ModelContact modelContact)
        {
            String baoloi = "";
            if (ModelState.IsValid)
            {
                try
                {
                    modelContact.Created_At = DateTime.Now;
                    modelContact.Status = 1;
                    db.Contact.Add(modelContact);
                    db.SaveChanges();
                    ViewBag.thanhcong = 1;
                    return View("Index");
                }
                catch(Exception ex)
                {
                    baoloi += "Gửi Không Thành Công";
                }
               
            }
            ViewBag.Error = baoloi;
            return View(modelContact);
        }
    }
}