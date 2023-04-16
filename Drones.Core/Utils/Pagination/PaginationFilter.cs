namespace Drones.Core.Utils.Pagination
{
    public sealed class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Filters { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 0;
            this.PageSize = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 0 ? 0 : pageNumber;
            this.PageSize = pageSize;
        }

        public PaginationFilter(int pageNumber, int pageSize, string filters)
        {
            this.PageNumber = pageNumber < 0 ? 0 : pageNumber;
            this.PageSize = pageSize;
            this.Filters = filters;
        }
    }
}
