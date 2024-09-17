namespace EmployeeSystem.Contract.Interfaces;
public interface ISalaryService
{
    /// <summary>
    /// Retrieves the salary details of the employee with the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the employee.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a list of SalaryDto with the salary details for the specified employee.
    /// </returns>
    public Task<List<SalaryDto>> GetEmployeeSalaryDetails(int id);


    /// <summary>
    /// Processes the payment of the salary for the specified employee.
    /// </summary>
    /// <param name="employeeId">The unique identifier of the employee whose salary is being paid.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a boolean value indicating whether the salary payment was successful.
    /// </returns>
    public Task<bool> Pay(int employeeId);


    /// <summary>
    /// Allows the super-admin to process salary payments for all employees at once.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing a boolean value indicating whether the salary payments were successfully processed for all employees.
    /// </returns>
    public Task<bool> PayAll();

}
