using EmployeeSystem.Contract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
