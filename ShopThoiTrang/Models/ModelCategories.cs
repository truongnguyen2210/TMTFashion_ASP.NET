using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace ShopThoiTrang.Models
{
    [Table("Categories")]
    public class ModelCategories
    {       
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Tên loại sản phẩm không được để trống")]
        [DisplayName("Tên Loại sản phẩm")]
        public string Name { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public int Orders { get; set; }
        [Required(ErrorMessage = "Từ khóa không được để trống")]
        public string MetaKey { get; set; }
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string MetaDesc { get; set; }
        public DateTime? Created_At { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Update_At { get; set; }
        public int? Update_By { get; set; }
        public int Status { get; set; }
    }
}