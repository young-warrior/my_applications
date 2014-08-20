using System.ComponentModel.DataAnnotations;

namespace NewsManager.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CategoryNews
    {
        public CategoryNews()
        {
            IsActive = true;
            IsKey = false;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int CategoryNewsID { get; set; }
        [Display(Name = "Category")]
        [Required]
        [DataType(DataType.MultilineText)]
        public String Name { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public bool IsKey { get; set; }

    }
}