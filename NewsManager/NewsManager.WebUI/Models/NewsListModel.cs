using System.Collections.Generic;

namespace NewsManager.WebUI.Models
{
    public class NewsListModel
    {
        public IList<NewsModel> Entities { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string CurrentCategory { get; set; }
    }
}