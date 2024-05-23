using Microsoft.Data.SqlClient;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EMS.CoreLibrary.Models
{
    public class EmployeeV3
    {
        public int Id { get; set; }

        public string? EmployeeCode { get; set; }

        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 5, ErrorMessage = "FirstName should be of minimum 5 and maximum 250 characters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 5, ErrorMessage = "LastName should be of minimum 5 and maximum 250 characters.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 5, ErrorMessage = "Email should be of minimum 5 and maximum 250 characters.")]
        [EmailAddress]
        public string Email { get; set; }

        public bool? Active { get; set; }
    }
}
