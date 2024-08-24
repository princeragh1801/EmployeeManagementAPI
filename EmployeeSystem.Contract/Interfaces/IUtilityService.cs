namespace EmployeeSystem.Contract.Interfaces
{
    public interface IUtilityService
    {
        public IQueryable<T> GetOrdered<T>(IQueryable<T> query, string columnName, bool ascending = true);
    }
}
