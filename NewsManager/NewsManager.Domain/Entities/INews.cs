namespace NewsManager.Domain.Entities
{
    using System;

    public interface INews
    {
        int NewsID { get; set; }

        string Title { get; set; }

        DateTime CreatedDate { get; set; }

        string BodyNews { get; set; }

        NewsStatusType Status { get; set; }

        CategoryNews Category { get; set; }
    }
}