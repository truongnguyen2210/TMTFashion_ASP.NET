using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopThoiTrang.Models;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class ContactsController : BaseController
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Contacts
        public ActionResult Index(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var list = db.Contact.Where(m => m.Status != 0).OrderByDescending(m => m.Created_At);
            return View("Index", list.ToPagedList(pageNumber, pageSize));
        }
        //Trash
        public ActionResult Trash(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var list = db.Contact.Where(m => m.Status == 0).OrderByDescending(m => m.Created_At);
            return View("Trash", list.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelContact modelContact = db.Contact.Find(id);
            if (modelContact == null)
            {
                return HttpNotFound();
            }
            return View(modelContact);
        }

        // GET: Admin/Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,Email,Phone,Title,Detail,ReplayDetail,ReplayID,Created_At,Update_At,Update_By,Status")] ModelContact modelContact)
        {
            if (ModelState.IsValid)
            {
                db.Contact.Add(modelContact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modelContact);
        }
        //send mail
        public void SendEmail(string address, string subject, string message)
        {
            string email = "truongthanhnguyen2210@gmail.com";
            string password = "";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }
        // GET: Admin/Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelContact modelContact = db.Contact.Find(id);
            if (modelContact == null)
            {
                return HttpNotFound();
            }
            return View(modelContact);
        }

        // POST: Admin/Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelContact modelContact, FormCollection field)
        {
            String baoloi = "";
            if (ModelState.IsValid)
            {
                try
                {
                    Random repid = new Random();
                    /* String subject = field["subject"];*/
                    /* String address = modelContact.Email;*/
                    String message = field["message"];
                    modelContact.ReplayID = repid.Next(1, 10000);
                    modelContact.ReplayDetail = message;
                    modelContact.Update_At = DateTime.Now;
                    modelContact.Update_By = (int?)Session["UserIdAdmin"];
                    db.Entry(modelContact).State = EntityState.Modified;
                    db.SaveChanges();
                    /* this.SendEmail(address, subject, message);*/
                    return RedirectToAction("Index");
                }
                catch
                {
                    baoloi += "khong thanh cong";
                }        
            }
            ViewBag.Error = baoloi;
            return View(modelContact);
        }

        // GET: Admin/Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelContact modelContact = db.Contact.Find(id);
            if (modelContact == null)
            {
                return HttpNotFound();
            }
            return View(modelContact);
        }

        // POST: Admin/Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelContact modelContact = db.Contact.Find(id);
            db.Contact.Remove(modelContact);
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
        public ActionResult Deltrash(int id)
        {
            ModelContact modelContact = db.Contact.Find(id);
            if (modelContact != null)
            {
                modelContact.Status = 0;
                db.Entry(modelContact).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Contacts");
        }
        public ActionResult Restore(int id)
        {
            ModelContact modelContact = db.Contact.Find(id);
            if (modelContact != null)
            {
                modelContact.Status = 1;
                db.Entry(modelContact).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Trash", "Contacts");
        }

    }
}
