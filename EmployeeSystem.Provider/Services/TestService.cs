using EmployeeSystem.Contract.Interfaces;

namespace EmployeeSystem.Provider.Services
{
    public class TestService : ITestService
    {
        private readonly ApplicationDbContext _context;
        int id;
        public TestService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public string Get()
        {
            Console.WriteLine("Context : " + _context);
            id++;
            return $"Hello {id}";
        }

        public async Task<string> ConvertToTextAsync(Stream srcStream)
        {
            try
            {
                var reader = new StreamReader(srcStream);
                var content = await reader.ReadToEndAsync();
                return content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
