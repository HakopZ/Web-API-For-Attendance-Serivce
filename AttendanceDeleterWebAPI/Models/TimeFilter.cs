using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class TimeFilter
    {
        [Required]
        public DateOnly Start { get; set; }

        [Required]
        public DateOnly End { get; set; }

        public TimeFilter(DateOnly start, DateOnly end)
        {
            Start = start;
            End = end;
        }
    }
}
