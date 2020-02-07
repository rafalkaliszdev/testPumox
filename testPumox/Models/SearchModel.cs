using System;

namespace testPumox.Domain
{
    public class SearchModel
    {
        public string Keyword { get; set; }
        public DateTime? EmployeeDateOfBirthFrom { get; set; }
        public DateTime? EmployeeDateOfBirthTo { get; set; }
        public JobTitle[] EmployeeJobTitles { get; set; }
    }
}