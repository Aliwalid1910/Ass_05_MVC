using Demo.BusinessLogic.DTOS.DepartmentDTOS;
using Demo.BusinessLogic.DTOS.EmployeeDTOS;
using Demo.BusinessLogic.Services.Classes;
using Demo.BusinessLogic.Services.Interfaces;
using Demo.DataAccess.Models.EmployeeModule;
using Demo.DataAccess.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    public class EmployeeController(IEmployeeService _employeeService
         , IWebHostEnvironment _env, ILogger<DepartmentController> _logger) : Controller
    {

        // Master Avtion
        // Baseurl/Employee/Index
        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employeeService.GetAllEmployees();
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                try
                {
                    int result = _employeeService.CreateEmployee(employeeDto);
                    if (result > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Employee Can not be Created !!");

                    }
                }
                catch (Exception ex)
                {
                    if (_env.IsDevelopment())
                    {
                        _logger.LogError($"Employee Con not be created becouse : {ex.Message}");

                    }
                    else
                    {
                        _logger.LogError($"Employee Con not be created becouse : {ex.Message}");
                        return View("Error view", ex);
                    }
                }
            }
            return View(employeeDto);

        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();
            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();

            var employeeDto = new UpdatedEmployeeDto()
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                IsActive = employee.IsActive,
                Email = employee.Email,
                Salary = employee.Salary,
                PhoneNumber = employee.PhoneNumber,
                HiringDate = employee.HiringDate,
                Gender = Enum.Parse<Gender>(employee.Gender),
                EmployeeType = Enum.Parse<EmployeeType>(employee.EmployeeType)
            };
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id ,  UpdatedEmployeeDto employeeDto)
        { 
            if(!id.HasValue || id != employeeDto.Id) return BadRequest();
            if (!ModelState.IsValid) return View(employeeDto);
            try
            {
                int result = _employeeService.UpdateEmployee(employeeDto);
                if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "Employee can not be updated");
                    return View(employeeDto);
                }
            }
            catch (Exception ex)
            {
                {
                    if (_env.IsDevelopment())
                    {
                        _logger.LogError($"Employee Con not be Updated becouse : {ex.Message}");
                        return View(employeeDto);
                    }
                    else
                    {
                        _logger.LogError($"Employee Con not be Updated becouse : {ex.Message}");
                        return View("Error view", ex);
                    }
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (id == 0) return BadRequest();
            try
            {
                bool IsDeleted = _employeeService.DeleteEmployee(id);
                if (IsDeleted)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "Employee Can not be Deleted");
                    return RedirectToAction(nameof(Delete), new { id });
                }

            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    _logger.LogError($"Employee Con not be created becouse : {ex.Message}");

                }
                else
                {
                    _logger.LogError($"Employee Con not be created becouse : {ex.Message}");
                    return View("Error view", ex);
                }
            }
            return RedirectToAction(nameof(Delete), new { id });
        }
    }



}


