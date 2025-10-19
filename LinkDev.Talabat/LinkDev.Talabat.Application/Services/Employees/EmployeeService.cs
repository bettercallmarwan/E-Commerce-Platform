using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Employees;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Employee;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Employees;
using LinkDev.Talabat.Core.Domain.Specifications.Employees;

namespace LinkDev.Talabat.Core.Application.Services.Employees
{
    internal class EmployeeService(IUnitOfWork unitOfWork, IMapper mapper) : IEmployeeService
    {
        public async Task<EmployeeToReturnDto> GetEmployeeAsync(int id)
        {
            var spec = new EmployeeWithDepartmentSpecifications(id);

            var employee = unitOfWork.GetRepository<Employee, int>().GetWithSpecAsync(spec);

            var employeeToReturn = mapper.Map<EmployeeToReturnDto>(employee);

            return employeeToReturn;
        }



        public async Task<IEnumerable<EmployeeToReturnDto>> GetEmployeesAsync()
        {
            var spec = new EmployeeWithDepartmentSpecifications();

            var employees = unitOfWork.GetRepository<Employee, int>().GetAllWithSpecAsync(spec);

            var employeesToReturn = mapper.Map<IEnumerable<EmployeeToReturnDto>>(employees);

            return employeesToReturn;

        }
    }
}
