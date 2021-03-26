using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopThoiTrang.Models
{
    [Table("Cart")]
    public class ModelCart
    {
        public int ProductId  { get; set; }
        public ModelProducts Product { get; set; }
        public int Quantity { get; set; }
    }
}
