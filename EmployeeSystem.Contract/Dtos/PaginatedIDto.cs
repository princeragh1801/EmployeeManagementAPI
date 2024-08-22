using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos
{
    public class PaginatedDto
    {
        public int PageIndex { get; set; }

        public int PagedItemsCount { get; set; }

        public string OrderKey { get; set; } = string.Empty;

        public  SortedOrder SortedOrder {  get; set; } = SortedOrder.NoOrder;

        public string Search {  get; set; } = string.Empty;

        public List<Tuple<string, int>> ?Filters { get; set; }

    }
}
