using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace testPumox.Domain
{
    public class Company : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Required 4 digit format for EstablishmentYear")]
        public int EstablishmentYear { get; set; }



        // Navigation properties.
        public ICollection<Employee> Employees { get; set; }
    }
}
