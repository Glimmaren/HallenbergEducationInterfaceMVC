namespace HallenbergEducationFull.Models
{
    public class AddCourseTeacherViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Alias => FirstName[..1].ToUpper() + "." + LastName;
    }
}
