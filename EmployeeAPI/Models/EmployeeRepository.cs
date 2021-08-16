using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPI.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _db;

        public EmployeeRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            var result = await _db.Employees.AddAsync(employee);
            await _db.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteEmployee(int employeeId)
        {
            var foundEmployee = await _db.Employees.FindAsync(employeeId);
            if (foundEmployee != null)
            {
                 _db.Employees.Remove(foundEmployee);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Employee> GetEmployee(int employeeId)
        {
            return await _db.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            return await _db.Employees.Include(e=>e.Department).FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _db.Employees.ToListAsync();
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            _db.Entry(employee).State = EntityState.Modified;
            try
            {
                _db.SaveChanges();
            }
            catch
            {
                throw;
            }
            return employee;
        }
    }
}
