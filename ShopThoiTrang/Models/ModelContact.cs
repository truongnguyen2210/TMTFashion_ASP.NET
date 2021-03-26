using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace ShopThoiTrang.Models
{
    [Table("Contact")]
    public class ModelContact
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Detail { get; set; }
        public string ReplayDetail { get; set; }
        public int? ReplayID { get; set; }
        public DateTime? Created_At { get; set; }
        public DateTime? Update_At { get; set; }
        public int? Update_By { get; set; }
        public int? Status { get; set; }
    }
}