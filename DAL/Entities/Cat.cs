namespace DAL.Entities
{
    public class Cat
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
