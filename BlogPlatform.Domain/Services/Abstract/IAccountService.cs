using BlogPlatform.Infrastructure;
using BlogPlatform.Domain.Entities;

namespace BlogPlatform.Domain.Services.Abstract
{
    public interface IAccountService
    {
        AuthenticationStatus LogIn(string emailAddress, string password);
        void DeleteAccount(int accountId);
        void UpdateAccount(string firstName, string lastName, string emailAddress, string NickName, string password);
        Account CreateAccount(string firstName, string lastName, string emailAddress, string NickName, string password);
        Account GetAccountProfile(int accountId);
    }
}
