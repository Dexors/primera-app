using System;
using System.ComponentModel.DataAnnotations;

namespace PtoBackend.Models
{
    public class PtoRequestDto
    {
        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida.")]
        public DateTime EndDate { get; set; }
    }
}
