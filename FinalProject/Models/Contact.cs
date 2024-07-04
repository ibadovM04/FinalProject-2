using FinalProject.Model;

namespace FinalProject.Models
{
    public class Contact:Entity<int>
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
         public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
