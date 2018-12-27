using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter.Models
{
    [Table("Person")]
    public class Person
    {
        public Person()
        {
        }

        [Key]
        public string userId { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime Joined { get; set; }

        public bool Active { get; set; }

        public string SCode { get; set; }

    }    
}