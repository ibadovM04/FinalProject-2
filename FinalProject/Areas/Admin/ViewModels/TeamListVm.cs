using FinalProject.DTOs;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class TeamListVm
    {
        public List<TeamDto> Teams { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
