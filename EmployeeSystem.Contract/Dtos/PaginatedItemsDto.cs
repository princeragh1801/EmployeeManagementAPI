namespace EmployeeSystem.Contract.Dtos
{
    public class PaginatedItemsDto<T>
    {
        public T ?Data { get; set; }

        public int TotalPages { get; set; }

        public int TotalItems {  get; set; }
    }
}
