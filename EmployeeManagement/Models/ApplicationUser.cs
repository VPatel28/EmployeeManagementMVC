using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class ApplicationUser : IdentityUser
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

        //public virtual ICollection<IdentityUserRole<int>> ApplicationUserRole { get; } = new List<IdentityUserRole<int>>();

        //public virtual ICollection<IdentityUserClaim<int>> Claims { get; } = new List<IdentityUserClaim<int>>();

        //public virtual ICollection<IdentityUserLogin<int>> Logins { get; } = new List<IdentityUserLogin<int>>();
    }
}
