using ChemLab.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChemLab.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService) =>
            _accountService = accountService;

        [HttpGet]
        [AllowAnonymous]
        [Route("signin-google")]
        public IActionResult SignInGoogle()
        {
            var url = "account/handle-google-callback";

            var properties = _accountService.GetLoginProperties(url);
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }

        [HttpGet]
        [Route("handle-google-callback")]
        public async Task<IActionResult> HandleGoogleCallback(string remoteError = null)
        {
            var loginResult = await _accountService.HandleGoogleCallback();

            if (loginResult.Length > 0)
                RedirectToAction("SignInGoogle");

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();

            return RedirectToAction("Index", "Home");
        }
    }
}

