using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopThoiTrang
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "TrangChu",
                url: "trang-chu",
                defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LienHe",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "GioHang",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "GioHangThem",
                url: "them-gio-hang",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "KhachHang",
                url: "khach-hang",
                defaults: new { controller = "KhachHang", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "KhachHang/DangNhap",
                url: "khach-hang/dang-nhap",
                defaults: new { controller = "Customer", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "KhachHang/DangXuat",
               url: "khach-hang/dang-xuat",
               defaults: new { controller = "KhachHang", action = "DangXuat", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "TimKiem",
               url: "tim-kiem/{keyword}",
               defaults: new { controller = "TimKiem", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "SanPham",
               url: "san-pham",
               defaults: new { controller = "Site", action = "Products", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "SanPhamHaiCap",
               url: "san-pham/{keyword}",
               defaults: new { controller = "Site", action = "Products", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "ChiTietSanPham",
              url: "chi-tiet-san-pham/{keyword}",
              defaults: new { controller = "Site", action = "ProductDetail", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Tintuc",
              url: "tin-tuc",
              defaults: new { controller = "Site", action = "Newpapers", id = UrlParameter.Optional }
          );






            routes.MapRoute(
               name: "SiteSlug",
               url: "{slug}",
               defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
