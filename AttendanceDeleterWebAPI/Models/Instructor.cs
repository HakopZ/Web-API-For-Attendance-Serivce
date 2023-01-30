using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class Instructor
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int ID { get; }

        
    }
}
