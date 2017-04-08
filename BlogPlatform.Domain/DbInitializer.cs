using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Infrastructure.Cryptography;

namespace BlogPlatform.Domain
{
    public static class DbInitializer
    {
        private static BlogPlatformContext context;
        private const int ArticlesCount = 100;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            context = (BlogPlatformContext)serviceProvider.GetService(typeof(BlogPlatformContext));

            InitializeUsers();
            InitializeArticles();
        }

        private static void InitializeUsers()
        {
            if (!context.Accounts.Any())
            {
                var passwordHasher = new PasswordHasher();

                var salt = passwordHasher.GetRandomSalt();

                context.Accounts.Add(new Account()
                {
                    EmailAddress = "dima@dima.com",
                    FirstName = "Dmitry",
                    LastName = "Shiray",
                    Nickname = "dimitry",
                    Salt = salt,
                    Password = passwordHasher.GetPassword("123456", salt),
                    DateCreated = DateTime.Now
                });

                context.Accounts.Add(new Account()
                {
                    EmailAddress = "test@test.com",
                    FirstName = "Test",
                    LastName = "Test",
                    Nickname = "test",
                    Salt = salt,
                    Password = passwordHasher.GetPassword("123456", salt),
                    DateCreated = DateTime.Now
                });

                context.SaveChanges();
            }
        }

        private static void InitializeArticles()
        {
            if (!context.Articles.Any())
            {
                var dimaAccount = context.Accounts.Where(a => string.Equals(a.EmailAddress, "dima@dima.com", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                var testAccount = context.Accounts.Where(a => string.Equals(a.EmailAddress, "test@test.com", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                for (int i = 0; i < ArticlesCount; i += 2)
                {
                    context.Articles.Add(
                        new Article
                        {
                            DateCreated = DateTime.Now,
                            Title = "Article " + i,
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                            AccountId = dimaAccount.Id,
                            LastDateModified = DateTime.Now
                        });

                    context.Articles.Add(
                         new Article
                         {
                             DateCreated = DateTime.Now,
                             Title = "Article " + (i + 1),
                             Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                             AccountId = testAccount.Id,
                             LastDateModified = DateTime.Now
                         });
                }
                
                context.SaveChanges();
            }
        }
    }
}
