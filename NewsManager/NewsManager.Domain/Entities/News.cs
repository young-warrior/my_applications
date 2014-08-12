using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsManager.Domain.Entities
{
    public class News
    {
        public News()
        {
            // set initial status as Unread
            this.Status = NewsStatusType.Unread;
            
            // set initial date to current date time
            this.CreatedDate = DateTime.UtcNow;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsID { get; set; }

        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BodyNews { get; set; }
        public NewsStatusType Status { get; set; }

        public string Category { get; set; }
    }
}
