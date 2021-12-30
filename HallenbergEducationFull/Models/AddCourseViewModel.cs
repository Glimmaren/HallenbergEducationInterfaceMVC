using System.ComponentModel.DataAnnotations;

namespace HallenbergEducationFull.Models
{
    public class AddCourseViewModel
    {
        
        [Required(ErrorMessage = "Ogiltligt kursnamn")]
        public string CourseName { get; set; }
        [Required(ErrorMessage = "Ogiltligt antal timmar")]
        public int Hours { get; set; }
        [Required(ErrorMessage = "Du måste välja en Lärare")]
        public int TeacherId { get; set; }
    }
}
