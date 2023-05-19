using RareCrew.Models;
using RareCrew.ModelView;

namespace RareCrew.Service
{
    public interface IEmployeeService
    {
        List<EmployeeModelView>? GetTotalWorkHoursForEmployees();       
    }
}
