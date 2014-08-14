namespace NewsManager.WebUI.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CategoryNewsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryNewsID { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please enter a Category")]
        public String Name { get; set; }
    }
}