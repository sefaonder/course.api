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
        Task<ApiResult> Update(EmployeeUpdateDto model);
        Task<ApiResult> Delete(Guid id);
        Task<IList<EmployeeListDto>> Get();
        Task<ApiResult> GetById(Guid id);


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
                Phone = model.Phone,
                Gender = model.Gender,
                Salary = model.Salary
            };

            await _context.Employee.AddAsync(entity);
            await _context.SaveChangesAsync();

            return new ApiResult { Success = true, Data = entity.Id, Message = "Ok" };
        }

        public async Task<ApiResult> Update(EmployeeUpdateDto model)
        {
            var entity = await _context.Employee
                .Where(x => !x.IsDeleted && x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (entity == null)
                return new ApiResult { Success = false, Data = model.Id, Message = ApiResultMessages.PCE01 };

            entity.Name = model.Name;
            entity.Email = model.Email;
            entity.Phone = model.Phone;
            entity.BirthDate = model.BirthDate;
            entity.Gender = model.Gender;
            entity.Salary = model.Salary;
            entity.IsDeleted = model.IsDeleted;
            await _context.SaveChangesAsync();

            return new ApiResult { Data = entity.Id, Message = ApiResultMessages.Ok };
        }

        public async Task<ApiResult> Delete(Guid id)
        {
            var entity = await _context.Employee
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null)
                return new ApiResult { Success = false, Data = id, Message = ApiResultMessages.PCE01 };

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();

            return new ApiResult { Success = true, Data = entity.Id, Message = ApiResultMessages.Ok };
        }


        public async Task<IList<EmployeeListDto>> Get()
        {
            var result = await _context
                .Employee
                .Where(x => !x.IsDeleted)
                .Select(s => new EmployeeListDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Phone = s.Phone,

                })
                .OrderBy(o => o.Name)
                .ToListAsync();

            return result;
        }

        public async Task<ApiResult> GetById(Guid id)
        {
            var result = await _context
                .Employee
                .Where(x => !x.IsDeleted && x.Id == id)
                .Select(s => new EmployeeGetDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    BirthDate = s.BirthDate,
                    Phone = s.Phone,
                    Gender = s.Gender,
                    Salary = s.Salary,
                })

                .FirstOrDefaultAsync();

            if (result == null)
                return new ApiResult { Success = false, Data = id, Message = ApiResultMessages.PCE01 };

            return new ApiResult { Success = true, Data = result, Message = ApiResultMessages.Ok };
        }
    }
}