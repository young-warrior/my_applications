namespace NewsManager.WebUI.Models
{
    using System.Collections.Generic;
    public class CategoriesNewListModel
    {
        public IList<CategoriesModel> Entities { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}