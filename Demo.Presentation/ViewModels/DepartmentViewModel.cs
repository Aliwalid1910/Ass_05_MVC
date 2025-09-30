using Microsoft.VisualBasic;

namespace Demo.Presentation.ViewModels
{
    public class DepartmentViewModel
    {
        public string Code { get; set; }=string.Empty;
        public string Name { get; set; }= string.Empty;
        public string? Description { get; set; }
        public DateOnly CreatedOn { get; set; }
    }
}
