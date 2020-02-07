using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using testPumox.Domain;
using testPumox.Persistence;

namespace testPumox.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class CompanyController : ControllerBase
    {
        private readonly IRepository<Company> companyRepository;
        private readonly IRepository<Employee> employeeRepository;
        private readonly EfDbContext dbContext;

        public CompanyController(
            IRepository<Company> companyRepository,
            IRepository<Employee> employeeRepository,
            EfDbContext dbContext)
        {
            this.companyRepository = companyRepository;
            this.employeeRepository = employeeRepository;
            this.dbContext = dbContext;
        }

        [HttpPost("company/create")]
        public IActionResult Create([FromBody]Company model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Invalid data");

            var created = companyRepository.Add(model);
            return Ok(new { Id = created.Id });
        }

        [HttpPut("company/update/{id}")]
        public IActionResult Update([FromBody]Company model, long id)
        {
            if (model == null || id == 0 || !ModelState.IsValid)
                return BadRequest("Invalid data");

            model.Id = id;
            companyRepository.Update(model);

            // I couldn't find elegant solution for add/update child entities.
            foreach (var e in model.Employees)
            {
                // Put.
                if (e.Id == 0)
                    employeeRepository.Add(e);
                // Update.
                else if (e.Id != 0)
                    employeeRepository.Update(e);
            }
            return NoContent();
        }

        [HttpDelete("company/delete/{id}")]
        public IActionResult Delete(long id)
        {
            if (id == 0)
                return BadRequest("Invalid id");

            companyRepository.Delete(companyRepository.GetById(id));

            return NoContent();
        }

        [HttpPost("company/search")]
        public IActionResult Search([FromBody]SearchModel model)
        {
            if (model == null)
                return BadRequest("SearchModel is null");

            // Sanitize input.
            var from = model.EmployeeDateOfBirthFrom ?? DateTime.MinValue;
            var to = model.EmployeeDateOfBirthTo ?? DateTime.MaxValue;
            var jobTitles = model.EmployeeJobTitles ?? new JobTitle[0];
            var keyword = model.Keyword == null ? string.Empty : model.Keyword.ToLower();
            
            List<Company> matchedCompanies = dbContext.Company
                .Include(a => a.Employees)
                .Select(c => new Company
                {
                    Id = c.Id,
                    Name = c.Name,
                    EstablishmentYear = c.EstablishmentYear,
                    // Filter out Employees.
                    Employees = c.Employees.Where(e =>
                        // Filter by keyword.
                        (
                            keyword == string.Empty ? true : e.FirstName.ToLower().Contains(keyword)
                            || keyword == string.Empty ? true : e.LastName.ToLower().Contains(keyword)
                        )
                        // Filter by job title.
                        && jobTitles.Length == 0 ? true : jobTitles.Contains(e.JobTitle)
                        // Filter by date between.
                        && e.DateOfBirth >= from
                        && e.DateOfBirth <= to)
                    .ToList()
                })
                .ToList();
            // Filter out Company.
            if (keyword != string.Empty)
                // Filter by keyword.
                matchedCompanies.RemoveAll(c => !c.Name.ToLower().Contains(keyword) && c.Employees.Count == 0);

            return Ok(matchedCompanies);
        }
    }
}
