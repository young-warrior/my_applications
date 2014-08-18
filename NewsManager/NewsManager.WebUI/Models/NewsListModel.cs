using System.Collections.Generic;

namespace NewsManager.WebUI.Models
{
    using System.Web.WebPages.Html;

    public class NewsListModel
    {
        public IList<NewsModel> Entities { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public IEnumerable<SelectListItem> CategoryID { get; set; }

        public int? CurrentCategory { get; set; }

        public string SortOrder { get; set; }
        public string SearchString { get; set; }

        public string SearchBy { get; set; }
    }
}