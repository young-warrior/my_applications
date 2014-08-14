namespace NewsManager.WebUI.Models
{
    using System.Collections.Generic;

    using NewsManager.Domain.Entities;

    public class NewsListModel
    {
        public IList<NewsModel> Entities { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string CurrentCategory { get; set; }
    }
}