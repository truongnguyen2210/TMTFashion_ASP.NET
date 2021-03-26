using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopThoiTrang
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int ProductStatus { get; set; }
        public string ProductImg { get; set; }
    }
}