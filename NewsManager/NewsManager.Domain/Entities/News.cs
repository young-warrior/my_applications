﻿using System;
using System.ComponentModel.DataAnnotations;
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
        
        
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a Title")]
        public string Title { get; set; }
        
        [DataType(DataType.Time)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Input field")]
        [Required(ErrorMessage = "Please enter a News")]
        [DataType(DataType.MultilineText)]
        
        [StringLength(Int32.MaxValue)]
        public string BodyNews { get; set; }
        public NewsStatusType Status { get; set; }

        public string Category { get; set; }
    }
}
