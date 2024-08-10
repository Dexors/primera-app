using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace PtoBackend.Models
{
    public class PtoRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        [Required(ErrorMessage = "El ID de usuario es requerido.")]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("startDate")]
        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        [Required(ErrorMessage = "La fecha de fin es requerida.")]
        public DateTime EndDate { get; set; }

        [BsonElement("status")]
        [Required(ErrorMessage = "El estado es requerido.")]
        public string Status { get; set; } = "Pendiente";

        [BsonElement("reason")]
        [MaxLength(500, ErrorMessage = "La razón no puede exceder los 500 caracteres.")]
        public string? Reason { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonIgnore]
        public int DurationInDays => (EndDate - StartDate).Days + 1;
    }
}
