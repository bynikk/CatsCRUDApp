using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BLL.Entities
{
    public class Cat
    {
        [BsonId]
        public int Id { get; set; }
        [BsonElement("Name")]
        public string? Name { get; set; }
        [BsonElement("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        public Cat()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
