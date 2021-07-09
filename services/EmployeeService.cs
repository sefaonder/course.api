using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using course.database;
using course.database.model;
using course.helper;
using Microsoft.EntityFrameworkCore;

namespace course.services
{
    public interface IEmployeeService
    {
        Task<ApiResult> Add(EmployeeAddDto model);
        Task<IList<EmployeeGetDto>> Get();
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly CourseDbContext _context;
        public EmployeeService(CourseDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult> Add(EmployeeAddDto model)
        {
            Employee entity = new Employee
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                Salary = model.Salary
            };

            await _context.Employee.AddAsync(entity);
            await _context.SaveChangesAsync();

            return new ApiResult { Success = true, Data = entity.Id, Message = "Ok" };
        }

        public async Task<IList<EmployeeGetDto>> Get()
        {
            var result = await _context
                .Employee
                .Where(x => !x.IsDeleted)
                .Select(s => new EmployeeGetDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Phone = s.Phone,
                    BirthDate = s.BirthDate,
                    Gender = s.Gender,
                    Salary = s.Salary
                })
                .OrderBy(o => o.Name)
                .ToListAsync();

            return result;
        }
    }
}