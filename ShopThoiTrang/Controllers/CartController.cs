using ShopThoiTrang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Controllers
{
    public class CartController : Controller
    {
        ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();
        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<ModelCart>();
            if (cart != null)
            {
                list = (List<ModelCart>)cart;
            }
            return View(list);
        }
        public ActionResult AddItem(int productId, int quantity = 1)
        {
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<ModelCart>)cart;
                if (list.Exists(m => m.ProductId == productId))
                {

                    foreach (var item in list)
                    {
                        if (item.ProductId == productId)
                        {
                            item.Quantity += quantity;
                        }
                    } 
                }
                else
                {
                    //tao doi tuong cart item
                    var item = new ModelCart();
                    item.ProductId = productId;
                    item.Quantity = quantity;
                    item.Product = db.Product.Find(productId);
                    list.Add(item);
                }
                Session[CartSession] = list;
            }
            else
            {
                //tao doi tuong cart item
                var item = new ModelCart();
                item.ProductId = productId;
                item.Quantity = quantity;
                item.Product = db.Product.Find(productId);
                var list = new List<ModelCart>();

                //gan vao session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteItem(int productId)
        {
            var cart = Session[CartSession];
            var list = (List<ModelCart>)cart;
            foreach (var item in list)
            {
                if (item.ProductId == productId)
                {
                    list.Remove(item);
                    Session[CartSession] = list;
                    break;
                }
            }
            return RedirectToAction("Index");
        }
      
    }
}