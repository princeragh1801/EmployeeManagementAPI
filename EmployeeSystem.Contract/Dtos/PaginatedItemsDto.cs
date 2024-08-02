namespace EmployeeSystem.Contract.Dtos
{
    internal class PaginatedItemsDto<T>
    {
        public T ?Items { get; set; }
    }
}
