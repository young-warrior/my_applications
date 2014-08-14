using System.Collections.Generic;

namespace NewsManager.WebUI.Models
{
    public class CategoriesNewListModel
    {
        public List<CategoryNewsModel> Entities { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}