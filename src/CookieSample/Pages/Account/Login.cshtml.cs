using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CookieSample.Data;

using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;

namespace CookieSample.Pages.Account;

public class LoginModel(ILogger<LoginModel> logger) : PageModel
{
    private readonly ILogger<LoginModel> _logger = logger;

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        // This check is required to use shared cookies beetween Edge and IE Mode.
        if (User.Identity.IsAuthenticated)
        {
            // Redirect to home page if the user is authenticated.
            _logger.LogInformation($"Already authenticated");
            return LocalRedirect(Url.GetLocalUrl(returnUrl));
        }

        // Clear the existing external cookie
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        ReturnUrl = returnUrl;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl;

        if (ModelState.IsValid)
        {
            // Use Input.Email and Input.Password to authenticate the user
            // with your custom authentication logic.
            //
            // For demonstration purposes, the sample validates the user
            // on the email address maria.rodriguez@contoso.com with 
            // any password that passes model validation.

            var user = await AuthenticateUser(Input.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Email),
                new("FullName", user.FullName),
                new(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logger.LogInformation("User {Email} logged in at {Time}.",
                user.Email, DateTime.UtcNow);

            return LocalRedirect(Url.GetLocalUrl(returnUrl));
        }

        // Something failed. Redisplay the form.
        return Page();
    }

    private static async Task<ApplicationUser> AuthenticateUser(string email)
    {
        // For demonstration purposes, authenticate a user
        // with a static email address. Ignore the password.
        // Assume that checking the database takes 500ms

        await Task.Delay(500);

        if (email == "maria.rodriguez@contoso.com")
        {
            return new ApplicationUser()
            {
                Email = "maria.rodriguez@contoso.com",
                FullName = "Maria Rodriguez"
            };
        }
        else
        {
            return null;
        }
    }
}
