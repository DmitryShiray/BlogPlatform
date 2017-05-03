using AutoMapper;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Services.Abstract;
using BlogPlatform.ViewModels;

namespace BlogPlatform.Mappings
{
    public class ViewModelMappingsProfile : Profile
    {
        private IArticleRatingService articleRatingService;

        public ViewModelMappingsProfile(IArticleRatingService articleRatingService) : base()
        {
            this.articleRatingService = articleRatingService;
        }

        public ViewModelMappingsProfile()
        {
            CreateMap<Article, ArticleViewModel>()
               .ForMember(vm => vm.TotalComments, map => map.MapFrom(p => p.Comments.Count))
               .ForMember(vm => vm.Rating, map => map.MapFrom(p => articleRatingService.ComputeRating(p.Ratings)));
            CreateMap<ArticleViewModel, Article>();

            CreateMap<Account, AccountViewModel>();
            CreateMap<AccountViewModel, Account>();

            CreateMap<Account, ProfileViewModel>()
               .ForMember(vm => vm.RegistrationDate, map => map.MapFrom(p => p.DateCreated));

            CreateMap<ProfileViewModel, Account>()
               .ForMember(vm => vm.DateCreated, map => map.MapFrom(p => p.RegistrationDate));

            CreateMap<Comment, CommentViewModel>()
              .ForMember(vm => vm.Text, map => map.MapFrom(p => p.Value))
              .AfterMap((s, d) => { d.Author = new AccountViewModel(); })
              .AfterMap((s, d) => { d.Author.Id = s.Account.Id; })
              .AfterMap((s, d) => { d.Author.FirstName = s.Account.FirstName; })
              .AfterMap((s, d) => { d.Author.LastName = s.Account.LastName; })
              .AfterMap((s, d) => { d.Author.Nickname = s.Account.Nickname; })
              .AfterMap((s, d) => { d.Author.EmailAddress = s.Account.EmailAddress; });

            CreateMap<CommentViewModel, Comment>()
              .ForMember(vm => vm.Value, map => map.MapFrom(p => p.Text))
              .AfterMap((s, d) => { d.Account = new Account(); })
              .AfterMap((s, d) => { d.Account.Id = s.Author.Id; })
              .AfterMap((s, d) => { d.Account.FirstName = s.Author.FirstName; })
              .AfterMap((s, d) => { d.Account.LastName = s.Author.LastName; })
              .AfterMap((s, d) => { d.Account.Nickname = s.Author.Nickname; })
              .AfterMap((s, d) => { d.Account.EmailAddress = s.Author.EmailAddress; });
        }
    }
}
