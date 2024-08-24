using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos
{
    public class PaginatedDto<T>
    {
        public int PageIndex { get; set; }

        public int PagedItemsCount { get; set; }

        public string OrderKey { get; set; } = string.Empty;

        public  SortedOrder SortedOrder {  get; set; } = SortedOrder.NoOrder;

        public string Search {  get; set; } = string.Empty;

        public DateRangeDto? DateRange { get; set; }

        public T? Status { get; set; }
    }
}
