using System;
using System.ComponentModel.DataAnnotations;

namespace NewsManager.WebUI.Models
{
    public class CategoriesModel
    {
        public CategoriesModel()
        {
            // set initial date to current date time
            CreatedDate = DateTime.UtcNow;
        }
        
        [DataType(DataType.Time)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        public virtual CategoryNewsModel Category { get; set; }
    }
}