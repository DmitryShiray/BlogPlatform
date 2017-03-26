using AutoMapper;
using BlogPlatform.Domain.Entities;
using BlogPlatform.ViewModels;

namespace BlogPlatform.Mappings
{
    public class ViewModelMappingsProfile : Profile
    {
        public ViewModelMappingsProfile()
        {
            CreateMap<Article, ArticleViewModel>()
               .ForMember(vm => vm.TotalComments, map => map.MapFrom(p => p.Comments.Count));
            CreateMap<ArticleViewModel, Article>();


            CreateMap<Account, ProfileViewModel>()
               .ForMember(vm => vm.RegistrationDate, map => map.MapFrom(p => p.DateCreated));

            CreateMap<ProfileViewModel, Account>()
               .ForMember(vm => vm.DateCreated, map => map.MapFrom(p => p.RegistrationDate));
        }
    }
}
