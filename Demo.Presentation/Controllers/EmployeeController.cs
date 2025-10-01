using Demo.BusinessLogic.DTOS.DepartmentDTOS;
using Demo.BusinessLogic.DTOS.EmployeeDTOS;
using Demo.BusinessLogic.Services.Classes;
using Demo.BusinessLogic.Services.Interfaces;
using Demo.DataAccess.Models.EmployeeModule;
using Demo.DataAccess.Models.Shared;
using Demo.Presentation.ViewModels;
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
        public IActionResult Create(EmployeeViewModel employeeviewmodel)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                try
                {
                    int result = _employeeService.CreateEmployee(new CreateEmployeeDto()
                    { 
                        Name = employeeviewmodel.Name,
                        Age = employeeviewmodel.Age,
                        Address = employeeviewmodel.Address,
                        IsActive = employeeviewmodel.IsActive,
                        DepartmentId = employeeviewmodel.DepartmentId,
                        Salary= employeeviewmodel.Salary,
                        Email= employeeviewmodel.Email,
                        EmployeeType= employeeviewmodel.EmployeeType,
                        Gender= employeeviewmodel.Gender,
                        PhoneNumber= employeeviewmodel.PhoneNumber,
                        HiringDate= employeeviewmodel.HiringDate,
                    
                    });
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
            return View(employeeviewmodel);

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

            var employeeviewmodel = new EmployeeViewModel()
            {
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                IsActive = employee.IsActive,
                Email = employee.Email,
                Salary = employee.Salary,
                PhoneNumber = employee.PhoneNumber,
                HiringDate = employee.HiringDate,
                Gender = Enum.Parse<Gender>(employee.Gender),
                EmployeeType = Enum.Parse<EmployeeType>(employee.EmployeeType),
                DepartmentId = employee.DepartmentId,
            };
            return View(employeeviewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id , EmployeeViewModel employeeviewmodel)
        { 
            if(!id.HasValue ) return BadRequest();
            if (!ModelState.IsValid) return View(employeeviewmodel);
            try
            {
                int result = _employeeService.UpdateEmployee(new UpdatedEmployeeDto() 
                { 
                    Address = employeeviewmodel.Address,
                    Age =employeeviewmodel.Age,
                    IsActive=employeeviewmodel.IsActive,
                    Email = employeeviewmodel.Email,
                    Gender = employeeviewmodel.Gender,
                    EmployeeType = employeeviewmodel.EmployeeType,
                    Name = employeeviewmodel.Name,
                    PhoneNumber = employeeviewmodel.PhoneNumber,
                    HiringDate=employeeviewmodel.HiringDate,
                    Salary = employeeviewmodel.Salary,
                    DepartmentId = employeeviewmodel.DepartmentId,
                    Id = id.Value
                });
                if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "Employee can not be updated");
                    return View(employeeviewmodel);
                }
            }
            catch (Exception ex)
            {
                {
                    if (_env.IsDevelopment())
                    {
                        _logger.LogError($"Employee Con not be Updated becouse : {ex.Message}");
                        return View(employeeviewmodel);
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


