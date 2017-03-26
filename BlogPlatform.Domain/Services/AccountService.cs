using System;
using System.Linq;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Services.Abstract;
using BlogPlatform.Infrastructure;
using BlogPlatform.Infrastructure.Cryptography;
using BlogPlatform.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Threading.Tasks;

namespace BlogPlatform.Domain.Services
{
    public class AccountService : IAccountService
    {
        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private BlogPlatformContext context;
        private PasswordHasher passwordHasher;

        public AccountService(BlogPlatformContext context, PasswordHasher passwordHasher)
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
        }

        public void DeleteAccount(string emailAddress)
        {
            var account = context.Accounts.FirstOrDefault(a => string.Equals(a.EmailAddress, emailAddress, StringComparison.OrdinalIgnoreCase));
            context.Accounts.Remove(account);
            context.SaveChanges();
        }

        public async Task<Account> GetAccountProfileAsync(string emailAddress)
        {
            return await context.Accounts.FirstOrDefaultAsync(a => string.Equals(a.EmailAddress, emailAddress, StringComparison.OrdinalIgnoreCase));
        }

        public AuthenticationStatus LogIn(string emailAddress, string password)
        {
            var account = context.Accounts.FirstOrDefault(a => string.Equals(a.EmailAddress, emailAddress, StringComparison.OrdinalIgnoreCase));
            if (account == null)
            {
                return AuthenticationStatus.NotFound;
            }

            if (!String.IsNullOrEmpty(password))
            {
                byte[] salt = account.Salt;
                byte[] encryptedPassword = Cryptographer.GenerateHash(password, salt);

                if (!encryptedPassword.SequenceEqual(account.Password))
                {
                    Logger.Warn("Authentication failure: invalid password for user {0}", emailAddress);
                    return AuthenticationStatus.InvalidPassword;
                }
            }
            else
            {
                Logger.Warn("Authentication failure: invalid password for user {0}",
                                   emailAddress);
                return AuthenticationStatus.InvalidPassword;
            }

            return AuthenticationStatus.Success;
        }

        public Account CreateAccount(string firstName, string lastName, string emailAddress, string nickname, string password)
        {
            if (CheckIfAccountExists(emailAddress))
            {
                Logger.Error("User already exists");
                throw new ServiceException("User already exists");
            }

            try
            {
                var salt = passwordHasher.GetRandomSalt();

                var account = new Account()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = emailAddress,
                    Nickname = nickname,
                    Salt = salt,
                    Password = passwordHasher.GetPassword(password, salt),
                    DateCreated = DateTime.Now
                };

                context.Accounts.Add(account);
                context.SaveChanges();

                return account;
            }
            catch (DbUpdateException exception)
            {
                Logger.Error(exception);
                throw new ServiceException(exception.Message, exception);
            }
        }

        public void UpdateAccount(Account account)
        {
            var accountFromDb = context.Accounts.FirstOrDefault(a => string.Equals(a.EmailAddress, account.EmailAddress, StringComparison.OrdinalIgnoreCase));

            accountFromDb.FirstName = account.FirstName;
            accountFromDb.LastName = account.LastName;
            accountFromDb.EmailAddress = account.EmailAddress;
            accountFromDb.Nickname = account.Nickname;

            context.SaveChanges();
        }

        public void ChangePassword(string emailAddress, string oldPassword, string newPassword, string confirmation)
        {
            var account = context.Accounts.FirstOrDefault(a => string.Equals(a.EmailAddress, emailAddress, StringComparison.OrdinalIgnoreCase));

            var oldPasswordMatch = account.Password.SequenceEqual(passwordHasher.GetPassword(oldPassword, account.Salt));
            if (!oldPasswordMatch)
            {
                Logger.Error("Old password doesn't match the new one.");
                throw new ServiceException("Old password doesn't match the new one.");
            }

            var newPasswordMatch = string.Equals(newPassword, confirmation, StringComparison.OrdinalIgnoreCase);
            if (!newPasswordMatch)
            {
                Logger.Error("New password mismatches the confirmation.");
                throw new ServiceException("New password mismatches the confirmation.");
            }

            account.Password = passwordHasher.GetPassword(newPassword, account.Salt);

            context.SaveChanges();
        }

        public bool CheckIfAccountExists(string emailAddress)
        {
            return context.Accounts.Any(a => string.Equals(a.EmailAddress, emailAddress, StringComparison.OrdinalIgnoreCase));
        }
    }
}
