namespace FinalProject.Model
{
    public class City : Entity<short>
    {
        public string Name { get; set; }

        public short CountryId { get; set; }
    }
}
