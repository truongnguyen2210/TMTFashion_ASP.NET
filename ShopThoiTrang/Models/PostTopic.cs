using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopThoiTrang
{
    public class PostTopic
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostSlug { get; set; }
        public string PostImg { get; set; }
        public string PostDetail { get; set; }
        public string PostTop { get; set; }
        public DateTime PostDayCreat { get; set; }
        public int PostStatus { get; set; }
    }
}