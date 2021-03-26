using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopThoiTrang
{
    public class XMessage
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public XMessage() { }
        public XMessage(string type, string msg)
        {
            this.Type = type;
            this.Message = msg;
        }
    }
}