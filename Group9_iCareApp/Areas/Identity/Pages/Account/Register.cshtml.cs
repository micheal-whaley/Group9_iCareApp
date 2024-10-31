// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Group9_iCareApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Group9_iCareApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        SqlCommand com = new SqlCommand();

        SqlDataReader dr;
        SqlConnection con = new SqlConnection();
        private readonly SignInManager<iCAREUser> _signInManager;
        private readonly UserManager<iCAREUser> _userManager;
        private readonly IUserStore<iCAREUser> _userStore;
        private readonly IUserEmailStore<iCAREUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly iCAREDBContext _dbContext;

        public List<Location> locations = new List<Location>();

        public RegisterModel(
            UserManager<iCAREUser> userManager,
            IUserStore<iCAREUser> userStore,
            SignInManager<iCAREUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _dbContext = new iCAREDBContext();
            string connectionString = "Data Source=localhost\\MSSQLSERVER01;Initial Catalog=Group9_iCareDB;Integrated Security=True; Encrypt=True;Trust Server Certificate=True;";

            con.ConnectionString = connectionString;
        }


    //    var listdata = _dbcontext.UserDetails.ToList().Select(x => new Group9_iCareApp.Models.Location
    //    {
    //        userid = x.userid
    //                   }).ToList();

    //public string locationName;

    //    public void OnGet()
    //    {
    //        string connectionString = "Data Source=localhost\\\\MSSQLSERVER01;Initial Catalog=Group9_iCareDB;Integrated Security=True";
    //        SqlConnection con = new SqlConnection(connectionString);
    //        con.Open();

    //        string sqlQuery = "select name, Location from Group9_iCareDB where ID =1";

    //        SqlCommand cmd = new SqlCommand(sqlQuery, con);

    //        SqlDataReader dr = cmd.ExecuteReader();

    //        if (dr.Read())
    //        {
    //            locationName = dr["name"].ToString();
    //        }

    //        con.Close();
    //    }


        private void FetchData()
        {

            if (locations.Count > 0)
            {
                locations.Clear();
            }

            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [ID],[name],[Description] FROM [Group9_iCareDB].[dbo].[Location]";
                dr = com.ExecuteReader();
                Console.WriteLine("RAAAAH");
                while (dr.Read())
                {
                    Console.Write("AAAAH LOOPING");
                    locations.Add(new Location() { Id=  int.Parse(dr["ID"].ToString()), 
                        Name = dr["name"].ToString(),
                        Description = dr["description"].ToString() });
                }
                con.Close();
            } catch
            {
                throw;
            }
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [DataType(DataType.Text)]
            [Display(Name ="First Name")]
            public string Fname { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string Lname { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            FetchData();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Fname = Input.Fname;
                user.Lname = Input.Lname;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private iCAREUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<iCAREUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(iCAREUser)}'. " +
                    $"Ensure that '{nameof(iCAREUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<iCAREUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<iCAREUser>)_userStore;
        }
    }
}
