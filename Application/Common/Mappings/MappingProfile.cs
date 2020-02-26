using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace TicTacToe.Application.Common.Mappings
{
    /// <summary>
    /// Register every mappings using reflection.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();
            foreach (Type type in types)
            {
                object instance = Activator.CreateInstance(type);
                MethodInfo methodInfo = type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
