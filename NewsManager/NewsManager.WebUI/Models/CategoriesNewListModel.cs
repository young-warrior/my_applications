using System.Collections.Generic;

namespace NewsManager.WebUI.Models
{
    public class CategoriesNewListModel : GridModel
    {
        public List<CategoryNewsModel> Entities { get; set; }
    }
}