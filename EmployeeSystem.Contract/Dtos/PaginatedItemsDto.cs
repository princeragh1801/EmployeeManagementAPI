using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Dtos
{
    internal class PaginatedItemsDto<T>
    {
        public T ?Items { get; set; }
    }
}
