using System.Runtime.Serialization;

namespace RareCrew.ModelView
{
    [DataContract]
    public class EmployeeModelView
    {
        [DataMember(Name = "Name")]
        public string? Name { get; set; }
        public string? TotalTime { get; set; }
        [DataMember(Name = "TotalTimeInSeconds")]
        public double TotalTimeInSeconds { get; set; }
        [DataMember(Name = "TotalTimePercents")]
        public double TotalTimePercents { get; set; }
    }
}
