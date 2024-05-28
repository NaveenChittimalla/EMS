using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EMS.WebApi.EfCore.Models;
public class Employee : BaseModel
{
    [MaxLength(255)]
    public string? Code { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string FirstName { get; set; }
    
    [MaxLength(255)]
    public string? LastName { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    
    [MaxLength(1000)]
    public string? Address { get; set; }
    
    public bool? Active { get; set; }
}
