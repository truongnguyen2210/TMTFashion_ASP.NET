using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang.Library;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();
        // GET: Admin/Auth
        public ActionResult Login()
        {
            if (!Session["UserAdmin"].Equals("")) 
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Error = "";
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection field)
        {
            string strerror = "";
            string username = field["username"];
            string password = Mystring.ToMD5(field["password"]);
            ModelUser rowuser = db.Users.Where(m => m.Status == 1 && m.Access == 1 && (m.UserName == username || m.Email == username))
                .FirstOrDefault();
            if (rowuser == null)
            {
                strerror = "Tên đăng nhập không tồn tại!";
            }
            else
            {
                if (rowuser.PassWord.Equals(password))
                {
                    Session["UserAdmin"] = rowuser.UserName;
                    Session["UserIdAdmin"] = rowuser.Id;
                    Session["FullNameAdmin"] = rowuser.FullName;
                    Session["ImgAdmin"] = rowuser.Img;
                    Session["EmailAdmin"] = rowuser.Email;
                    Session["PhoneAdmin"] = rowuser.Phone;
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    strerror = "Mật khẩu không đúng!";
                }
            }
            ViewBag.Error = "<span class='text-danger'>"+strerror+"</span>";
            return View();
        }
        public ActionResult Logout()
        {
            Session["UserAdmin"] = "";
            Session["UserIdAdmin"] = "";
            Session["FullNameAdmin"] = "";
            Session["ImgAdmin"] = "";
            Session["EmailAdmin"] = "";
            Session["PhoneAdmin"] = "";
            return RedirectToAction("Login", "Auth");
        }
    }
}