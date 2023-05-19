using RareCrew.Infrastructure;
using RareCrew.Models;
using RareCrew.ModelView;

namespace RareCrew.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRestClient restClient;
        private readonly IConfiguration configuration;

        public EmployeeService(IRestClient restClient, IConfiguration configuration)
        {
            this.restClient = restClient;
            this.configuration = configuration;
        }
        public List<EmployeeModelView>? GetTotalWorkHoursForEmployees()
        {
            try
            {
                List<EmployeeDTO>? employees = GetData();

                List<EmployeeModelView>? employeesGroupByName = employees?
                    .GroupBy(x => x.EmployeeName)
                    .Select(x => new EmployeeModelView { 
                        Name = x.Key ?? "Undefined employee", TotalTime = SumOfTimes(x), TotalTimeInSeconds = x.Sum(y => (y.EndTimeUtc - y.StarTimeUtc).TotalSeconds) })
                    .ToList();

                double totalTimeAllEmployee = 1;

                if (employeesGroupByName is not null || employeesGroupByName?.Count > 0) totalTimeAllEmployee = (double)employeesGroupByName.Sum(x => x.TotalTimeInSeconds);
                else return employeesGroupByName;

                if (double.IsNaN(totalTimeAllEmployee)) return null;

                employeesGroupByName?.ForEach(x => x.TotalTimePercents = Math.Round((x.TotalTimeInSeconds / totalTimeAllEmployee * 100), 2));              

                return employeesGroupByName;       
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        private List<EmployeeDTO>? GetData()
        {
            try
            {
                List<EmployeeDTO>? employees = new();
                var headerValues = new Dictionary<string, string>();
                var queryValues = new Dictionary<string, string>();

                List<KeyValuePair<string, object>> parameters = new()
                {
                    new KeyValuePair<string, object>("Code", configuration["RareCrew:Code"]?.ToString() ?? string.Empty)
                };

                employees = restClient.Get<EmployeeDTO>("gettimeentries", parameters, headerValues, configuration["RareCrew:BaseUrl"]?.ToString() ?? string.Empty);

                if (employees is not null) return employees?.OrderByDescending(x => x.EmployeeName).ToList();
                else return employees;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
                     
        }        
        private static string SumOfTimes(IGrouping<string?, EmployeeDTO> groupedNames)
        {
            var totaltime = groupedNames
                .Where(y => y.DeletedOn is null || y.EndTimeUtc > y.StarTimeUtc)
                .Sum(y => (y.EndTimeUtc - y.StarTimeUtc).TotalSeconds);          
            return string.Format("{0:D2}H:{1:D2}m", (int)TimeSpan.FromSeconds(totaltime).TotalHours, TimeSpan.FromSeconds(totaltime).Minutes);
        }

    }
}
