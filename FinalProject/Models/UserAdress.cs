namespace FinalProject.Model
{
    public class UserAddres : Entity<int>
    {
        public string Name { get; set; }
        public string Addres { get; set; }
        public string Phone { get; set; }
        public short CityId { get; set; }
        public Guid UserId { get; set; }

        public City City { get; set; }
    }
}
