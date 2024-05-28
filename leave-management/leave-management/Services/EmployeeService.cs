using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace leave_management.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(string id)
        {
            return await _context.Employees.FindAsync(id);
        }
    }
}
