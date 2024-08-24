using EmployeeSystem.Contract.Interfaces;
using System.Linq.Dynamic.Core;

namespace EmployeeSystem.Provider.Services
{
    public class UtilityService : IUtilityService
    {
        
        public IQueryable<T> GetOrdered<T>(IQueryable<T> query, string columnName, bool ascending = true)
        {

            // Find the matching property in a case-insensitive manner
            var property = typeof(T).GetProperties()
                .FirstOrDefault(p => p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

            if (property == null)
            {
                return query; // Property not found, return the original query
            }

            // Get the actual property name
            var actualPropertyName = property.Name;

            var sortingOrder = ascending ? "ascending" : "descending";
            var orderByString = $"{actualPropertyName} {sortingOrder}";

            return query.OrderBy(orderByString);
        }

    }
}
