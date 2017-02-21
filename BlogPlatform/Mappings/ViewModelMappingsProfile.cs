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
               .ForMember(vm => vm.TotalComments, map => map.MapFrom(p => "/images/" + p.Comments.Count));

            CreateMap<ArticleViewModel, Article>();
        }
    }
}
