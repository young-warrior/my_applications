using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsManager.Domain.Entities
{
    public class News
    {   
        public News()
        {
            // set initial status as Unread
            Status = NewsStatusType.inactive;

            // set initial date to current date time
            CreatedDate = DateTime.UtcNow;

            IsActive = true;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsID { get; set; }

        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public string BodyNews { get; set; }

        public NewsStatusType Status { get; set; }

        public virtual CategoryNews Category { get; set; }

        public bool IsActive { get; set; }
    }
}
