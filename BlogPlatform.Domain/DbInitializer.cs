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
                    EmailAddress = "dshiray@gmail.com",
                    FirstName = "Dmitry",
                    LastName = "Shiray",
                    NickName = "dimitry",
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
                context.Articles.Add(
                    new Article
                    {
                        DateCreated = DateTime.Now,
                        Title = "Article 1",
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        AccountId = 1,
                        LastDateModified = DateTime.Now
                    });

                context.Articles.Add(
                    new Article
                    {
                        DateCreated = DateTime.Now,
                        Title = "Article 2",
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        AccountId = 1,
                        LastDateModified = DateTime.Now
                    });

                context.Articles.Add(
                    new Article
                    {
                        DateCreated = DateTime.Now,
                        Title = "Article 3",
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        AccountId = 1,
                        LastDateModified = DateTime.Now
                    });

                context.Articles.Add(
                    new Article
                    {
                        DateCreated = DateTime.Now,
                        Title = "Article 4",
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        AccountId = 1,
                        LastDateModified = DateTime.Now
                    });

                context.SaveChanges();
            }
        }
    }
}
