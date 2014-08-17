namespace NewsManager.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.WebPages.Html;

    using NewsManager.Domain.Entities;

    using SelectListItem = System.Web.Mvc.SelectListItem;

    public class NewsModel  
    {
        public NewsModel()
        {
            // set initial status as Unread
            this.Status = NewsStatusType.inactive;
            
            // set initial date to current date time
            this.CreatedDate = DateTime.UtcNow;
        }
        
        public int NewsID { get; set; }
        
        [StringLength(200)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter a Title")]
        public string Title { get; set; }
        
        [DataType(DataType.DateTime)]
//        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Input field")]
        [Required(ErrorMessage = "Please enter a News")]
        [DataType(DataType.MultilineText)]
        
        [StringLength(200)]
        public string BodyNews { get; set; }
        
        public NewsStatusType Status { get; set; }

        [Display(Name = "Category")]
        public int? CategoryID { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        
        public CategoryNewsModel Category { get; set; }
    }
}