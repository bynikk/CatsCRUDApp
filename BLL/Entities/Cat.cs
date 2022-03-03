namespace BLL.Entities
{
    public class Cat
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public Cat()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
