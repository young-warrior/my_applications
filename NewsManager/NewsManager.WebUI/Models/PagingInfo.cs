﻿namespace NewsManager.WebUI.Models
{
    using System;

    public class PagingInfo
    {
        public int TotalItems { get; set; }

        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)this.TotalItems / this.ItemsPerPage);
            }
        }
       
    }
}