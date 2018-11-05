using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EmployeeManagement.Areas.Identity.Data;
using EmployeeManagement.DAL;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
              RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


            [Required]
            [Display(Name = "First Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string Surname { get; set; }

            [Required]
            public string Address { get; set; }


            [Required]
            public string Qualification { get; set; }

            [Required]
            [StringLength(10, ErrorMessage = "The {0} must be at least 10 characters.", MinimumLength = 10)]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Contact Number")]
            public string ContactNumber { get; set; }

            [Required]
            public string Roles { get; set; }

        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Name,
                    Address = Input.Address,
                    Qualification = Input.Qualification,
                    Surname = Input.Surname,
                    ContactNumber = Input.ContactNumber,
                    Roles = Input.Roles,
                    PhoneNumber = Input.ContactNumber
                };

                IdentityResult roleResult;
                bool RoleExists = await _roleManager.RoleExistsAsync(Input.Roles);
                if (!RoleExists)
                {
                    _logger.LogInformation("Adding role" + Input.Roles);
                    roleResult = await _roleManager.CreateAsync(new IdentityRole(Input.Roles));
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var isInRole = await _userManager.IsInRoleAsync(user, Input.Roles);
                    if (!isInRole)
                    {
                        await _userManager.AddToRoleAsync(user, Input.Roles);
                    }

                    var userId = await _userManager.GetUserIdAsync(user);

                    Registration_Details registration_Details = new Registration_Details();

                    registration_Details.Name = Input.Name;
                    registration_Details.Email = Input.Email;
                    registration_Details.Surname = Input.Surname;
                    registration_Details.Address = Input.Address;
                    registration_Details.ContactNumber = Input.ContactNumber;
                    registration_Details.Qualification = Input.Qualification;
                    registration_Details.Department = Input.Roles;
                    var options = new DbContextOptions<EmployeeContext>();
                    EmployeeContext employeeContext = new EmployeeContext(options: options);
                    employeeContext.registration_Details.Add(registration_Details);
                    employeeContext.SaveChanges();


                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }


}
