using AutoMapper;

namespace BlogPlatform.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ViewModelMappingsProfile>();
            });
        }
    }
}

