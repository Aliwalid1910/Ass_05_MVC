using AutoMapper;
using Demo.BusinessLogic.DTOS.EmployeeDTOS;
using Demo.BusinessLogic.Services.Interfaces;
using Demo.DataAccess.Models.EmployeeModule;
using Demo.DataAccess.Repositories.Interfaces;

namespace Demo.BusinessLogic.Services.Classes
{
    public class EmployeeService(IEmployeeRepositorie _employeeRepository , IMapper _mapper) : IEmployeeService
    {


        public IEnumerable<EmployeeDto> GetAllEmployees(bool withTracking = false)
        {
            //    var employeeDto = _employeeRepository.GetIQueryable().Where(e => e.IsDeleted == false)
            //        .Select(e => new EmployeeDto()
            //        {
            //            Id = e.Id,
            //            Name = e.Name,
            //            Salary = e.Salary,
            //            Age = e.Age,
            //        });
            //    return employeeDto.ToList();

            var employees = _employeeRepository.GetAll(withTracking);
        var employeesDto = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(employees);
            //var employeesDto = employees.Select(E => new EmployeeDto()
            //{
            //    Email = E.Email,
            //    Age = E.Age,
            //    Id = E.Id,
            //    Name = E.Name,
            //    Salary = E.Salary,
            //    IsActive = E.IsActive,
            //    Gender = E.Gender.ToString(),
            //    EmployeeType = E.EmployeeType.ToString(),

                //});
                return employeesDto;

            }

        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetById(id);
            return employee is null ? null : _mapper.Map<Employee,EmployeeDetailsDto>(employee);
            //{
            //    Email = employee.Email,
            //    Age = employee.Age,
            //    Id = employee.Id,
            //    Name = employee.Name,
            //    Address = employee.Address,
            //    PhoneNumber = employee.PhoneNumber,
            //    IsActive = employee.IsActive,
            //    Salary = employee.Salary,
            //    HiringDate = DateOnly.FromDateTime(employee.HiringDate),
            //    CreatedOn = employee.CreatedOn,
            //    ModifiedOn = employee.ModifiedOn,
            //    ModifiedBy = 1,
            //    CreatedBy = 1,
            //    EmployeeType = employee.EmployeeType.ToString(),
            //    Gender = employee.Gender.ToString()
            //};
            
        }

        public int CreateEmployee(CreateEmployeeDto employeeDto)
        {
            var employee = _mapper.Map<CreateEmployeeDto, Employee>(employeeDto);
            return _employeeRepository.Add(employee);

        }

        public bool DeleteEmployee(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee is null) return false;
            else
                employee.IsDeleted = true;
               return _employeeRepository.Update(employee) > 0 ?true : false;
         }


        public int UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
           return _employeeRepository.Update(_mapper.Map<UpdatedEmployeeDto, Employee>(employeeDto));
        }
    }
}
