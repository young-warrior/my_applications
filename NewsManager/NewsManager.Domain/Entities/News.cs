using System;
using System.ComponentModel.DataAnnotations;
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
        [StringLength(200)]
        [DataType(DataType.Text)]
        [Required]
        public string Title { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Input field")]
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(200)]
        public string BodyNews { get; set; }
        [Required]
        public NewsStatusType Status { get; set; }

        public virtual CategoryNews Category { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
