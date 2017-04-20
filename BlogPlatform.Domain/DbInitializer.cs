using System;
using System.Linq;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Infrastructure.Cryptography;

namespace BlogPlatform.Domain
{
    public static class DbInitializer
    {
        private static BlogPlatformContext context;
        private static RandomDateTimeGenerator randomDateTimeGenerator;
        private const int ArticlesCount = 50;
        private const int CommentsCount = 15;
        private const int MinRating = 1;
        private const int MaxRating = 5;
        private const int HoursInDay = 24;
        private const int MinutesInHour = 24;
        private const int SecondsInMinute = 24;
        private const int StartDateYear = 2016;
        private const int StartDateMonth = 1;
        private const int StartDateDay = 1;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            context = (BlogPlatformContext)serviceProvider.GetService(typeof(BlogPlatformContext));
            randomDateTimeGenerator = new RandomDateTimeGenerator();

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
                var accounts = context.Accounts.ToList();
                foreach (var account in accounts)
                {
                    for (int i = 0; i < ArticlesCount; i++)
                    {
                        var article = context.Articles.Add(
                            new Article
                            {
                                DateCreated = randomDateTimeGenerator.Next(),
                                Title = "Article " + i,
                                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                                AccountId = account.Id,
                                LastDateModified = randomDateTimeGenerator.Next()
                            });

                        AddComments(article.Entity, account);
                        AddRatings(article.Entity, account);
                    }
                }

                context.SaveChanges();
            }
        }

        private static void AddComments(Article article, Account account)
        {
            for (int i = 0; i < CommentsCount; i += 1)
            {
                context.Comments.Add(new Comment
                {
                    AccountId = account.Id,
                    ArticleId = article.Id,
                    DateAdded = randomDateTimeGenerator.Next(),
                    Value = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
                });
            }
        }

        private static void AddRatings(Article article, Account account)
        {
            Random randomGenerator = new Random();
            context.Ratings.Add(new Rating
            {
                AccountId = account.Id,
                ArticleId = article.Id,
                DateAdded = randomDateTimeGenerator.Next(),
                Value = (byte)randomGenerator.Next(MinRating, MaxRating)
            });
        }

        private class RandomDateTimeGenerator
        {
            private Random randomGenerator;
            private DateTime startDate;
            private int range;

            public RandomDateTimeGenerator()
            {
                randomGenerator = new Random();
                startDate = new DateTime(StartDateYear, StartDateMonth, StartDateDay);
                range = (DateTime.Today - startDate).Days;
            }

            public DateTime Next()
            {
                return startDate.AddDays(randomGenerator.Next(range))
                    .AddHours(randomGenerator.Next(0, HoursInDay))
                    .AddMinutes(randomGenerator.Next(0, MinutesInHour))
                    .AddSeconds(randomGenerator.Next(0, SecondsInMinute));
            }
        }
    }
}
