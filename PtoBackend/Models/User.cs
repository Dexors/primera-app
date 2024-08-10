using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PtoBackend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [Required]
        [BsonElement("name")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [BsonElement("mail")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
