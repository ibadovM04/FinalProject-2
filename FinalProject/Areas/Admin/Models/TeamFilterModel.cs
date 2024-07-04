namespace FinalProject.Areas.Admin.Models
{
    public class TeamFilterModel
    {
        public string TeamMemberName { get; set; }
        public string TeamMemberDescription { get; set; }
        public string TeamMemberPosition { get; set; }
        public int Page { get; set; } = 1;

    }
}
