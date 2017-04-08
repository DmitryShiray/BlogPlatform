using AutoMapper;
using System;
using System.Linq;

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

        /// <summary>
        /// To map multiple types on one entity.
        /// </summary>
        public static class EntityMapper
        {
            public static TDistinationBase DefaultDistinationMap<TSourceBase, TDistinationBase>(TSourceBase source) where TDistinationBase : class
            {
                TypeMap map = Mapper.Configuration.GetAllTypeMaps().FirstOrDefault(
                    typeMap => typeMap.SourceType.Equals(source.GetType()) && typeMap.DestinationType.Equals(typeof(TDistinationBase)));
                TDistinationBase result;

                if (map != null)
                {
                    result = Activator.CreateInstance(map.DestinationType) as TDistinationBase;
                    Mapper.Map(source, result, typeof(TSourceBase), typeof(TDistinationBase));
                }
                else
                    result = Mapper.Map<TDistinationBase>(source);

                return result;
            }

            public static T Map<T>(params object[] sources) where T : class
            {
                if (!sources.Any())
                {
                    return default(T);
                }

                var initialSource = sources[0];
                var mappingResult = Map<T>(initialSource);

                // Now map the remaining source objects
                if (sources.Count() > 1)
                {
                    Map(mappingResult, sources.Skip(1).ToArray());
                }

                return mappingResult;
            }

            private static void Map(object destination, params object[] sources)
            {
                if (!sources.Any())
                {
                    return;
                }

                var destinationType = destination.GetType();

                foreach (var source in sources)
                {
                    var sourceType = source.GetType();
                    Mapper.Map(source, destination, sourceType, destinationType);
                }
            }

            private static T Map<T>(object source) where T : class
            {
                var destinationType = typeof(T);
                var sourceType = source.GetType();

                var mappingResult = Mapper.Map(source, sourceType, destinationType);

                return mappingResult as T;
            }
        }
    }
}