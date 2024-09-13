using EmployeeSystem.Contract.Enums;
namespace EmployeeSystem.Contract.Dtos.Info
{
    public class EmployeeLoginInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public bool IsManager { get; set; } = false;
    }
}
