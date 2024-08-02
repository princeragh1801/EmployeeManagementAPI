using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos
{
    public class PaginatedDto
    {
        public int PageIndex { get; set; }

        public int PagedItemsCount { get; set; }

        public string OrderKey { get; set; } = string.Empty;

        public  SortedOrder SortedOrder {  get; set; } = SortedOrder.Ascending;

        public string Search {  get; set; } = string.Empty;
    }
}
