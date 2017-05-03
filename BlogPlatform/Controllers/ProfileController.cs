using BlogPlatform.Infrastructure.Result;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using NLog;
using BlogPlatform.Domain.Services.Abstract;
using BlogPlatform.ViewModels;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Infrastructure.Exceptions;
using AutoMapper;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;

namespace BlogPlatform.Controllers
{
    [Route("api/[controller]")]
    public class ProfileController : BaseController
    {
        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ProfileController(IAuthorizationService authorizationService, IAccountService accountService, IMemoryCache memoryCache)
            : base(accountService, authorizationService, memoryCache)
        {
        }

        [HttpGet("getUserProfile/{emailAddress?}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserProfile(string emailAddress)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(emailAddress))
                {
                    var getUserProfileResult = new BaseResult()
                    {
                        Succeeded = false,
                        Message = "Cannot get profile. Please, contact customer support."
                    };

                    return new ObjectResult(getUserProfileResult);
                }

                var account = await accountService.GetAccountProfileAsync(emailAddress);
                var profile = Mapper.Map<ProfileViewModel>(account);
                profile.IsCurrentUserProfile = IsCurrentUserProfile(profile.EmailAddress);

                return new ObjectResult(profile);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Failed to get user profile");
                return BadRequest();
            }
        }

        [HttpPost("updateProfile")]
        public IActionResult UpdateProfile([FromBody] ProfileViewModel profile)
        {
            BaseResult updateOperationResult = null;

            try
            {
                if (!IsCurrentUserProfile(profile.EmailAddress))
                {
                    updateOperationResult = new BaseResult()
                    {
                        Succeeded = false,
                        Message = "Cannot update profile that doesn't belong to current user"
                    };
                }

                if (accountService.CheckIfAccountExists(profile.EmailAddress))
                {
                    var account = Mapper.Map<Account>(profile);
                    accountService.UpdateAccount(account);

                    updateOperationResult = new BaseResult()
                    {
                        Succeeded = true,
                        Message = "Profile has been successfully updated"
                    };
                }
            }
            catch (ServiceException exception)
            {
                Logger.Error(exception, "Failed to update user profile");

                updateOperationResult = new BaseResult()
                {
                    Succeeded = false,
                    Message = "Failed to update profile " + exception.Message
                };
            }

            return new ObjectResult(updateOperationResult);
        }

        [HttpPost("changePassword")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] ChangePasswordViewModel changePasswordViewModel)
        {
            BaseResult updateOperationResult = null;

            try
            {
                if (!IsCurrentUserProfile(changePasswordViewModel.EmailAddress))
                {
                    updateOperationResult = new BaseResult()
                    {
                        Succeeded = false,
                        Message = "Cannot change password for profile that doesn't belong to current user"
                    };
                }

                if (accountService.CheckIfAccountExists(changePasswordViewModel.EmailAddress))
                {
                    accountService.ChangePassword(changePasswordViewModel.EmailAddress, changePasswordViewModel.OldPassword,
                                                  changePasswordViewModel.NewPassword, changePasswordViewModel.ConfirmNewPassword);

                    updateOperationResult = new BaseResult()
                    {
                        Succeeded = true,
                        Message = "Password has been successfully changed"
                    };
                }
            }
            catch (ServiceException exception)
            {
                Logger.Error(exception, "Failed to change password");

                updateOperationResult = new BaseResult()
                {
                    Succeeded = false,
                    Message = "Failed to change password " + exception.Message
                };
            }

            return new ObjectResult(updateOperationResult);
        }

        [HttpDelete("deleteProfile/{emailAddress}")]
        [Authorize]
        public IActionResult DeleteUserProfile(string emailAddress)
        {
            BaseResult deleteOperationResult = null;

            try
            {
                if (!IsCurrentUserProfile(emailAddress))
                {
                    deleteOperationResult = new BaseResult()
                    {
                        Succeeded = false,
                        Message = "Cannot change password for profile that doesn't belong to current user"
                    };
                }

                if (accountService.CheckIfAccountExists(emailAddress))
                {
                    accountService.DeleteAccount(emailAddress);
                }

                deleteOperationResult = new BaseResult()
                {
                    Succeeded = true,
                    Message = string.Format("Account {0} has been successfully deleted", emailAddress)
                };
            }
            catch (ServiceException exception)
            {
                Logger.Error(exception, "Failed to delete user profile");

                deleteOperationResult = new BaseResult()
                {
                    Succeeded = false,
                    Message = string.Format("Failed to delete user profile {0}. {1}", emailAddress, exception.Message)
                };
            }

            return new ObjectResult(deleteOperationResult);
        }

        private bool IsCurrentUserProfile(string emailAddress)
        {
            var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null && claim.Value == emailAddress;
        }
    }
}
