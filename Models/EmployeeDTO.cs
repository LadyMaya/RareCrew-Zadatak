namespace RareCrew.Models
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime StarTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public string? EntryNotes { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
