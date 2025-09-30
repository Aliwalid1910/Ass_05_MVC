using Demo.BusinessLogic.DTOS;
using Demo.BusinessLogic.DTOS.DepartmentDTOS;
using Demo.BusinessLogic.Services.Classes;
using Demo.BusinessLogic.Services.Interfaces;
using Demo.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Demo.Presentation.Controllers
{
    public class DepartmentController(IDepartmentService _departmentService
        , IWebHostEnvironment _env, ILogger<DepartmentController> _logger) : Controller
    {
        // ViewData , ViewBag ==> ViewStorage ==> Deal With The Same Storage
        // ExtraInfo [Extra Data]
        // Controller --> View
        // View --> Partial view
        // View --> Layout
        // ViewData [Safe] [.Net 3.5]
        // ViewBag [UnSafe] [.Net 4.0] ==>Dynamic

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            //ViewData["Message"] = "Hello In Departments";
            //ViewBag.Message01 = "Hello From View Bag";
            var departments = _departmentService.GetAllDepartments();
            return View(departments);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //  ===> Action Filter
        public IActionResult Create(DepartmentViewModel departmentviewmodel)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                try
                {
                    int result = _departmentService.AddDepartment(new CreatedepartmentDto() 
                    {
                        Code = departmentviewmodel.Code,
                        Description = departmentviewmodel.Description,
                        DateOfCreation = departmentviewmodel.CreatedOn,
                        Name = departmentviewmodel.Name,
                    });
                    string message;
                    if (result > 0)
                        message = "Department Created Successfuly";
                    //{
                    //    return RedirectToAction(nameof(Index));
                    //}
                    else
                        message = "Department Can not be Created";
                    //{
                    //    ModelState.AddModelError(string.Empty, "Department Can not be Created !!");

                    //}
                    TempData["Message"] = message;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (_env.IsDevelopment())
                    {
                        _logger.LogError($"Department Con not be created becouse : {ex.Message}");

                    }
                    else
                    {
                        _logger.LogError($"Department Con not be created becouse : {ex.Message}");
                        //return View(departmentDto);
                        return View("Error view" ,ex);
                    }
                }
            }
            return View(departmentviewmodel);

        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null) return NotFound();
            return View(department);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null) return NotFound();
            //return View(department);
            var departmentVM = new DepartmentViewModel()
            {
                Code = department.Code,
                Description = department.Description,
                Name = department.Name,
                CreatedOn = department.CreatedOn.HasValue ? department.CreatedOn.Value : default
            };
            return View(departmentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int? id, DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!id.HasValue) return BadRequest();
                    var updatedDeptDto = new UpdatedDepartmentDto()
                    {
                        Id = id.Value,
                        Code = departmentVM.Code,
                        Description = departmentVM.Description,
                        Name = departmentVM.Name,
                        DateOfCreation = departmentVM.CreatedOn
                    };
                    int result = _departmentService.UpdateDepartment(updatedDeptDto);
                    if (result > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Department can not be updated");
                        
                    }
                }
                catch (Exception ex)
                {

                    if (_env.IsDevelopment())
                    {
                        _logger.LogError($"Department Con not be created becouse : {ex.Message}");

                    }
                    else
                    {
                        _logger.LogError($"Department Con not be created becouse : {ex.Message}");
                        return View("Error view" , ex);
                    }
                }
            }
            return View(departmentVM);
        }

        #endregion

        #region Delete
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (!id.HasValue) return BadRequest();
        //    var department = _departmentService.GetDepartmentById(id.Value);
        //    if (department is null) return NotFound();
        //    return View(department);

        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (id == 0) return BadRequest();
            try
            {
                bool IsDeleted = _departmentService.DeleteDepartment(id);
                if (IsDeleted)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "Department Can not be Deleted");
                    return RedirectToAction(nameof(Delete), new { id });
                }

            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    _logger.LogError($"Department Con not be created becouse : {ex.Message}");

                }
                else
                {
                    _logger.LogError($"Department Con not be created becouse : {ex.Message}");
                    return View("Error view", ex);
                }
            }
            return RedirectToAction(nameof(Delete), new { id });
        }
        #endregion
    }
}

