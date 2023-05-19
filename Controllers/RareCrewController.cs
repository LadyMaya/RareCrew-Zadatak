using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RareCrew.ModelView;
using RareCrew.Service;

namespace RareCrew.Controllers
{
    public class RareCrewController : Controller
    {
        private readonly ILogger<RareCrewController> _logger;
        private readonly IEmployeeService employeeService;

        public RareCrewController(ILogger<RareCrewController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            this.employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult GetTotalWorkTimes()
        {
            List<EmployeeModelView>? employeeModelViews = employeeService.GetTotalWorkHoursForEmployees();
            ViewBag.EmployeeModelViews = JsonConvert.SerializeObject(employeeModelViews);
 
            return View(employeeModelViews);
        }      
    }
}