using System;
using System.Security.Claims;
using ChemLab.Data.Entity;
using ChemLab.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace ChemLab.Services
{
    
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public AuthenticationProperties GetLoginProperties(string url)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", url);
            return properties;
        }
        public async Task<string[]> HandleGoogleCallback()
        {
            List<string> errors = new List<string>();

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                errors.Add("Error loading external login information.");

                return errors.ToArray();
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return errors.ToArray();
            }

            if (result.IsLockedOut)
            {
                errors.Add("IsLockedOut");
                return errors.ToArray();
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = new ApplicationUser { UserName = email, Email = email, PictureUrl = info.Principal.FindFirstValue("picture") };

                var createUserResult = await _userManager.CreateAsync(user);
                if (!createUserResult.Succeeded)
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        errors.Add(error.Description);
                    }

                    return errors.ToArray();
                }

                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (!addLoginResult.Succeeded)
                {
                    foreach (var error in addLoginResult.Errors)
                    {
                        errors.Add(error.Description);
                    }

                    return errors.ToArray();
                }

                await _signInManager.SignInAsync(user, isPersistent: false);

                return errors.ToArray();
            }
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}

