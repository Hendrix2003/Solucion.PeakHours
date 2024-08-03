namespace PeakHours_Blazor.Models
{
    public class DropDownData
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class AreaFilter
    {
        public string Area { get; set; }
        public int AreaDay { get; set; }
    }

    public class EmployeeFilter
    {
        public int EmployeeId { get; set; }
        public int EmployeeDay { get; set; }
    }

    public class HourTypeFilter
    {
        public string HourTypeDay { get; set; }
    }
}
