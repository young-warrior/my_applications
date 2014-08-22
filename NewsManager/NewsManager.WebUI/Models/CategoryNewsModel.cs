namespace NewsManager.WebUI.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using NewsManager.WebUI.Models.Validators;

    [CategoryDuplicationValidator(ErrorMessage = "This category name is used. Please enter another name.")]
    public class CategoryNewsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int CategoryNewsID { get; set; }

        [Display(Name = "Category")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter a Category")]
        [StringLength(200)]
        public String Name { get; set; }


    }
}