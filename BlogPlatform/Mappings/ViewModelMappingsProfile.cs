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
               .ForMember(vm => vm.Rating, map => map.MapFrom(p => articleRatingService.ComputeRating(p.Ratings)))
               .AfterMap((s, d) => { s.Account.Id = d.Account.Id; })
               .AfterMap((s, d) => { s.Account.FirstName = d.Account.FirstName; })
               .AfterMap((s, d) => { s.Account.LastName = d.Account.LastName; })
               .AfterMap((s, d) => { s.Account.Nickname = d.Account.Nickname; })
               .AfterMap((s, d) => { s.Account.EmailAddress = d.Account.EmailAddress; });
            CreateMap<ArticleViewModel, Article>()
               .AfterMap((s, d) => { s.Account.Id = d.Account.Id; })
               .AfterMap((s, d) => { s.Account.FirstName = d.Account.FirstName; })
               .AfterMap((s, d) => { s.Account.LastName = d.Account.LastName; })
               .AfterMap((s, d) => { s.Account.Nickname = d.Account.Nickname; })
               .AfterMap((s, d) => { s.Account.EmailAddress = d.Account.EmailAddress; });

            CreateMap<Account, AccountViewModel>();
            CreateMap<AccountViewModel, Account>();

            CreateMap<Account, ProfileViewModel>()
               .ForMember(vm => vm.RegistrationDate, map => map.MapFrom(p => p.DateCreated));

            CreateMap<ProfileViewModel, Account>()
               .ForMember(vm => vm.DateCreated, map => map.MapFrom(p => p.RegistrationDate));

            CreateMap<Comment, CommentViewModel>()
              .ForMember(vm => vm.Text, map => map.MapFrom(p => p.Value))
              .AfterMap((s, d) => { s.Account.Id = d.Account.Id; })
              .AfterMap((s, d) => { s.Account.FirstName = d.Account.FirstName; })
              .AfterMap((s, d) => { s.Account.LastName = d.Account.LastName; })
              .AfterMap((s, d) => { s.Account.Nickname = d.Account.Nickname; })
              .AfterMap((s, d) => { s.Account.EmailAddress = d.Account.EmailAddress; });
            CreateMap<CommentViewModel, Comment>()
               .AfterMap((s, d) => { s.Account.Id = d.Account.Id; })
               .AfterMap((s, d) => { s.Account.FirstName = d.Account.FirstName; })
               .AfterMap((s, d) => { s.Account.LastName = d.Account.LastName; })
               .AfterMap((s, d) => { s.Account.Nickname = d.Account.Nickname; })
               .AfterMap((s, d) => { s.Account.EmailAddress = d.Account.EmailAddress; });
        }
    }
}
