using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
namespace ShopThoiTrang.Models
{
    [Table("Product")]
    public class ModelProducts
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CatId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Img { get; set; }
        [Required]
        public string Detail { get; set; }
        public int Number { get; set; }
        public float Price { get; set; }
        public float PriceSale { get; set; }
        [Required]
        public string MetaKey { get; set; }
        public string MetaDesc { get; set; }
        public DateTime Created_At { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Update_At { get; set; }
        public int? Update_By { get; set; }
        public int Status { get; set; }
    }
}