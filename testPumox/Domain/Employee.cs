using System;
using System.ComponentModel.DataAnnotations;

namespace testPumox.Domain
{
    public class Employee : EntityBase
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public JobTitle JobTitle { get; set; }



        // Navigation properties.
        public long CompanyId { get; set; }

        public Company Company { get; set; } 
    }
}