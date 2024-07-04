using FinalProject.Model;

namespace FinalProject.Models
{
    public class Team:Entity<int>
    {
        
        public string profileImageUrl { get; set; }
        public string TeamMemberName { get; set; }
        public string TeamMemberDescription { get; set; }
        public string TeamMemberPosition { get; set; }
    }
}
