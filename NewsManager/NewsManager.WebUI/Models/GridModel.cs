namespace NewsManager.WebUI.Models
{
    using System;

    public class GridModel
    {
        public GridModel()
        {
            // Define any default values here...
            this.PageSize = 7;
            this.NumericPageCount = 7;
        }
        

        // Sorting-related properties
        public string SortBy { get; set; }
        public bool SortAscending { get; set; }
        public string SortExpression
        {
            get
            {
                return this.SortAscending ? this.SortBy + " asc" : this.SortBy + " desc";
            }
        }
        
        // Paging-related properties
        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecordCount { get; set; }
        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling((decimal)this.TotalRecordCount / this.PageSize);
            }
        }
        public int NumericPageCount { get; set; }
    }
}