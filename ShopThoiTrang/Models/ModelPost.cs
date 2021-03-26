using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace ShopThoiTrang.Models
{
    [Table("Post")]
    public class ModelPost
    {
        [Key]
        public int Id { get; set; }
        public int TopId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Detail { get; set; }
        public string Img { get; set; }
        [Required]
        public string Type { get; set; }
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