namespace EmployeeSystem.Contract.Interfaces
{
    public interface ITestService
    {
        public string Get();
        public string ConvertToTextAsync(Stream srcStream);
        public Stream ConvertDocToDocx(Stream inputStream);
    }
}
