using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Areas.Identity.Data
{
    public class EmployeeManagementUser : IdentityUser
    {
        [Required]
        [PersonalData]
        public string Name { get; set; }
        [Required]
        [PersonalData]
        public string Surname { get; set; }
        [Required]
        [PersonalData]
        public string Address { get; set; }
        [Required]
        [PersonalData]
        public string Qualification { get; set; }
        [Required]
        [PersonalData]
        public string ContactNumber { get; set; }
        [Required]
        [PersonalData]
        public string Roles { get; set; }
    }
}
