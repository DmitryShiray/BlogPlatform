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

namespace BlogPlatform.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private IAccountService accountService;

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel user)
        {
            GenericResult authenticationResult = null;
            try
            {
                var loginResult = accountService.LogIn(user.EmailAddress, user.Password);
                if (loginResult == AuthenticationStatus.Success)
                {
                    var claims = new List<Claim>()
                    {
                        {
                            new Claim(ClaimTypes.Role, "EmailAddress", ClaimValueTypes.String, user.EmailAddress)
                        }
                    };

                    await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));


                    authenticationResult = new GenericResult()
                    {
                        Succeeded = true,
                        Message = "Authenticated"
                    };
                }

                if (loginResult == AuthenticationStatus.Success)
                {

                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Failed to authenticate");

                authenticationResult = new GenericResult()
                {

                    Succeeded = false,
                    Message = "Failed to authenticate " + exception.Message
                };

            }

            return new ObjectResult(authenticationResult);
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
            GenericResult registrationResult = null;

            try
            {
                if (ModelState.IsValid)
                {
                    var account = accountService.CreateAccount(user.FirstName, user.LastName, user.EmailAddress, user.NickName, user.Password);

                    if (account != null)
                    {
                        registrationResult = new GenericResult()
                        {
                            Succeeded = true,
                            Message = "Registration succeeded"
                        };
                    }
                }
                else
                {
                    registrationResult = new GenericResult()
                    {
                        Succeeded = false,
                        Message = "Invalid fields"
                    };
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Failed to authenticate");

                registrationResult = new GenericResult()
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
