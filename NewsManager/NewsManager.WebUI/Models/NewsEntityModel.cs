namespace NewsManager.WebUI.Models
{
    using System.ComponentModel.DataAnnotations;

    using NewsManager.Domain.Entities;

    public class NewsEntityModel : News, INews
    {
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
    }
}