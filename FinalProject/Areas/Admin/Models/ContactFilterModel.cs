namespace FinalProject.Areas.Admin.Models
{
    public class ContactFilterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Page { get; set; } = 1;
    }
}
