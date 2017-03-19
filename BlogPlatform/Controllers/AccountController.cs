using BlogPlatform.Infrastructure.Result;
using BlogPlatform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using Microsoft.AspNetCore.Authorization;
using BlogPlatform.Infrastructure.Attributes;
using NLog;
using BlogPlatform.Domain.Services.Abstract;
using BlogPlatform.Infrastructure;
using BlogPlatform.Infrastructure.Constants;

namespace BlogPlatform.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private IAccountService accountService;
        private IAuthorizationService authorizationService;

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AccountController(IAuthorizationService authorizationService, IAccountService accountService)
        {
            this.accountService = accountService;
            this.authorizationService = authorizationService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel user)
        {
            BaseResult authenticationResult = null;
            try
            {
                var loginResult = accountService.LogIn(user.EmailAddress, user.Password);
                if (loginResult == AuthenticationStatus.Success)
                {
                    var claims = new List<Claim>()
                    {
                        {
                            new Claim(ClaimTypes.Role, Claims.ClaimsPolicyValue, ClaimValueTypes.String, user.EmailAddress)
                        }
                    };

                    await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                       new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                       new Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties { IsPersistent = user.RememberMe });

                    authenticationResult = new BaseResult()
                    {
                        Succeeded = true,
                        Message = "Authenticated"
                    };
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Failed to authenticate");

                authenticationResult = new BaseResult()
                {
                    Succeeded = false,
                    Message = "Failed to authenticate " + exception.Message
                };

            }

            return new ObjectResult(authenticationResult);
        }

        [HttpPost("isUserAuthenticated")]
        [AllowAnonymous]
        public IActionResult IsUserAuthenticated()
        {
            var authenticationResult = HttpContext.User.HasClaim(c => c.Value == Claims.ClaimsPolicyValue);
            return new ObjectResult(new { IsAuthenticated = authenticationResult });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.Authentication.SignOutAsync("Cookies");

                return Ok();
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Failed to authenticate");

                return BadRequest();
            }

        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[ServiceFilter(typeof(AngularAntiForgeryTokenAttribute), IsReusable = true)]
        public IActionResult Register([FromBody] RegistrationViewModel user)
        {
            var result = new ObjectResult(false);
            BaseResult registrationResult = null;

            try
            {
                if (ModelState.IsValid)
                {
                    var account = accountService.CreateAccount(user.FirstName, user.LastName, user.EmailAddress, user.NickName, user.Password);

                    if (account != null)
                    {
                        registrationResult = new BaseResult()
                        {
                            Succeeded = true,
                            Message = "Registration succeeded"
                        };
                    }
                }
                else
                {
                    registrationResult = new BaseResult()
                    {
                        Succeeded = false,
                        Message = "Invalid fields"
                    };
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Failed to authenticate");

                registrationResult = new BaseResult()
                {
                    Succeeded = false,
                    Message = "Failed to register " + exception.Message
                };

            }

            result = new ObjectResult(registrationResult);
            return result;
        }
    }
}
