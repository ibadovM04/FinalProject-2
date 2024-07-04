using FinalProject.DTOs;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class ContactListVm
    {
        public List<ContactDto> Contacts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
