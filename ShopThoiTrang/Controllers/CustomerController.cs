using ShopThoiTrang.Library;
using ShopThoiTrang.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Controllers
{
    public class CustomerController : Controller
    {
        ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();
        // GET: Customer
        public ActionResult Login()
        {
            if(Session["UserCustomer"].Equals(""))
            {
                ViewBag.Error = "";
                return View();
            }
            return RedirectToAction("Index", "Site");
        }
        [HttpPost]
        public ActionResult Login(FormCollection field)
        {
            string strerror = "";
            string username = field["email"];
            string password = Mystring.ToMD5(field["pass"]);
            ModelUser rowuser = db.Users.Where(m => m.Status == 1 && (m.Phone == username || m.Email == username))
                .FirstOrDefault();
            if (rowuser == null)
            {
                strerror = "Tên đăng nhập không tồn tại!";
            }
            else
            {
                if (rowuser.PassWord.Equals(password))
                {
                    Session["UserCustomer"] = rowuser.UserName;
                    Session["UserId"] = rowuser.Id;
                    Session["FullName"] = rowuser.FullName; 
                    Session["Img"] = rowuser.Img;
                    Session["Email"] = rowuser.Email;
                    Session["Phone"] = rowuser.Phone;
                    return RedirectToAction("Index", "Site");
                }
                else
                {
                    strerror = "Mật khẩu không đúng!";
                }
            }
            ViewBag.Error = "<span class='text-danger'>" + strerror + "</span>";
            return View();
        }
        public ActionResult Logout()
        {
            Session["UserCustomer"] = "";
            Session["UserId"] = "";
            Session["FullName"] = "";
            Session["Img"] = "";
            Session["Email"] = "";
            Session["Phone"] = "";
            return RedirectToAction("Index", "Site");
        }
        public ActionResult Signin()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signin(ModelUser modelUser)
        {
            string strerror = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var file = Request.Files["img"];
                    if (file == null)
                    {
                        strerror = "Hình Chưa Được Chọn";
                    }
                    else
                    {
                        string[] FileExtentsions = new string[] { ".jpg", ".gif", ".png" };
                        if (!FileExtentsions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                        {
                            strerror = "Kiểu Tập Tin " + string.Join(", ", FileExtentsions) + " Không Cho Phép!";
                        }
                        else
                        {
                            string strName = Mystring.str_slug(modelUser.FullName);
                            String fileName = strName + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                            modelUser.Img = fileName;
                            String Strpath = Path.Combine(Server.MapPath("~/Public/Image/User"), fileName);
                            file.SaveAs(Strpath);
                            string matkhau = Mystring.ToMD5(modelUser.PassWord);
                            modelUser.Status = 1;
                            modelUser.PassWord = matkhau;
                            modelUser.Created_By = 1;
                            modelUser.Created_At = DateTime.Now;
                            modelUser.Update_By = 1;
                            modelUser.Update_At = DateTime.Now;
                            db.Users.Add(modelUser);
                            db.SaveChanges();
                            Session["UserCustomer"] = modelUser.UserName;
                            Session["UserId"] = modelUser.Id;
                            Session["FullName"] = modelUser.FullName;
                            Session["Img"] = modelUser.Img;
                            Session["Email"] = modelUser.Email;
                            Session["Phone"] = modelUser.Phone;
                            return RedirectToAction("Login");
                        }
                    }

                }
                catch (Exception ex)
                {
                    strerror = "Thêm Không Thành Công";
                }

            }
            ViewBag.Error = strerror;
            return View(modelUser);
        }
    }
}