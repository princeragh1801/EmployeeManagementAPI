namespace EmployeeSystem.Contract.Interfaces
{
    public interface ITestService
    {
        public string Get();
        public Task<string> ConvertToTextAsync(Stream srcStream);
    }
}
