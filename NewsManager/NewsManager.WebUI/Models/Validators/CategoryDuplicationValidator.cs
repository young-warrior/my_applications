namespace NewsManager.WebUI.Models.Validators
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using NewsManager.Domain.DAL;
    using NewsManager.Domain.Entities;

    public class CategoryDuplicationValidator : ValidationAttribute
    {
        private readonly ICategoryNewsRepository repo;

        public CategoryDuplicationValidator()
        {
            repo = new CategoryNewsRepository(new NewsRepository());
        }

        public override bool IsValid(object value)
        {
            var category = value as CategoryNewsModel;

            if (category == null || String.IsNullOrEmpty(category.Name)) 
                return true;
            
            return !this.repo.CategoryNewsEntities.Any(x => x.Name == category.Name 
                && x.CategoryNewsID != category.CategoryNewsID);
        }
    }
}