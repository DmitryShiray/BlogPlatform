using BlogPlatform.Infrastructure;
using BlogPlatform.Domain.Entities;
using System.Threading.Tasks;

namespace BlogPlatform.Domain.Services.Abstract
{
    public interface IAccountService
    {
        AuthenticationStatus LogIn(string emailAddress, string password);
        void UpdateAccount(Account account);
        void ChangePassword(string emailAddress, string oldPassword, string newPassword, string confirmation);
        Account CreateAccount(string firstName, string lastName, string emailAddress, string NickName, string password);
        Task<Account> GetAccountProfileAsync(string emailAddress);
        void DeleteAccount(string emailAddress);
        bool CheckIfAccountExists(string emailAddress);
    }
}
