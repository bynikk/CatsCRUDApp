using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BLL.Entities
{
    public class Dog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        [BsonElement("Name")]
        public string? Name { get; set; }
        [BsonElement("Breed")]
        public string? Breed { get; set; }
        [BsonElement("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        public Dog()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
