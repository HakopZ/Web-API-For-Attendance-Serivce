using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class Name
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
