using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CoreLibrary.Models
{
    public class Department : BaseModel
    {
        public string? Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool? Active { get; set; }
    }
}
