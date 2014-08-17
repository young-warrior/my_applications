namespace NewsManager.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CategoryNews
    {
        public CategoryNews()
        {
            IsActive = true;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryNewsID { get; set; }

        public String Name { get; set; }

        public bool IsActive { get; set; }
    }
}