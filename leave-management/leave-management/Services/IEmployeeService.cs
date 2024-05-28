using leave_management.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace leave_management.Services
{
    public interface IEmployeeService
    {
        Task<bool> AddEmployeeAsync(Employee employee);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(string id);
    }
}
