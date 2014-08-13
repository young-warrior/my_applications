namespace NewsManager.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CategoryNews
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryNewsID { get; set; }

        [Display(Name = "Category")]
        public String Name { get; set; }
    }
}