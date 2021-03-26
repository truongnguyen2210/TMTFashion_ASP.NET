using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShopThoiTrang.Models
{
    public class ShopThoiTrangDBContext : DbContext
    {
        public ShopThoiTrangDBContext():base("name=ChuoiKN")
        {

        }
        public virtual DbSet<ModelCategories> Categories { get; set; }
        public virtual DbSet<ModelContact> Contact { get; set; }
        public virtual DbSet<ModelLink> Links { get; set; }
        public virtual DbSet<ModelMenu> Menu { get; set; }
        public virtual DbSet<ModelOrder> Order { get; set; }
        public virtual DbSet<ModelOrderDetail> OrderDetail { get; set; }
        public virtual DbSet<ModelPost> Post { get; set; }
        public virtual DbSet<ModelProducts> Product { get; set; }
        public virtual DbSet<ModelSlider> Slider { get; set; }
        public virtual DbSet<ModelTopic> Topic { get; set; }
        public virtual DbSet<ModelUser> Users { get; set; }

    }
}