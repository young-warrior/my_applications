using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsManager.Domain.Entities;

namespace NewsManager.WebUI.Models
{
    public class NewsModel
    {
        public IList<News> Entities { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}