using System;
using System.Linq;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Services.Abstract;
using BlogPlatform.Infrastructure;
using BlogPlatform.Infrastructure.Cryptography;
using BlogPlatform.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using NLog;

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

        public void DeleteAccount(int accountId)
        {
            var account = context.Accounts.FirstOrDefault(a => a.Id == accountId);
            context.Accounts.Remove(account);
            context.SaveChanges();
        }

        public Account GetAccountProfile(int accountId)
        {
            return context.Accounts.FirstOrDefault(a => a.Id == accountId);
        }

        public AuthenticationStatus LogIn(string emailAddress, string password)
        {
            var account = context.Accounts.FirstOrDefault(a => a.EmailAddress == emailAddress);
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

        public Account CreateAccount(string firstName, string lastName, string emailAddress, string NickName, string password)
        {
            if (CheckIfUserExists(emailAddress))
            {
                Logger.Error("User already exists");
                throw new ServiceException("User already exists");
            }

            try
            {
                var account = new Account()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = emailAddress,
                    NickName = NickName,
                    Password = passwordHasher.GetPassword(password),
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

        public void UpdateAccount(string firstName, string lastName, string emailAddress, string NickName, string password)
        {
            var account = context.Accounts.FirstOrDefault(a => a.EmailAddress == emailAddress);

            account.FirstName = firstName;
            account.LastName = lastName;
            account.EmailAddress = emailAddress;
            account.NickName = NickName;
            account.Password = passwordHasher.GetPassword(password);

            context.SaveChanges();
        }

        public bool CheckIfUserExists(string emailAddress)
        {
            return context.Accounts.Any(x => string.Equals(x.EmailAddress, emailAddress, StringComparison.OrdinalIgnoreCase));
        }
    }
}
