using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace ShopThoiTrang.Models
{
    [Table("Order")]
    public class ModelOrder
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ExportDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryPhone { get; set; }
        public string DeliveryEmail { get; set; }
        public DateTime? Update_At { get; set; }
        public int? Update_By { get; set; }
        public int Status { get; set; }

    }
}