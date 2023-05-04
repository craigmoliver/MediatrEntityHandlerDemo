using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MediatrEntityHandlerDemo.Domain.Dtos;

namespace MediatrEntityHandlerDemo.Domain.Dtos
{
    public class EmployeeDto : IEntityDto
    {
        public int EmployeeID { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
        public string Title { get; set; }

        public string TitleOfCourtesy { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? HireDate { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string HomePhone { get; set; }

        public string Extension { get; set; }

        public byte[] Photo { get; set; }

        public string Notes { get; set; }

        public int? ReportsTo { get; set; }

        public string PhotoPath { get; set; }

        public EmployeeDto ReportsToEmployee { get; set; }

        public ICollection<EmployeeTerritoryDto> EmployeeTerritories { get; set; }

        public ICollection<OrderDto> Orders { get; set; }

        public ICollection<EmployeeDto> ReportingEmployees { get; set; }
    }
}
