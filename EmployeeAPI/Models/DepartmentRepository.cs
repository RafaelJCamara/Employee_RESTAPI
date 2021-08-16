using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPI.Models
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _db;

        public DepartmentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Department> GetDepartment(int departmentId)
        {
            return await _db.Departments.FindAsync(departmentId);
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _db.Departments.ToListAsync();
        }
    }
}
