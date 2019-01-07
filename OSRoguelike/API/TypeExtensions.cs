using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OSRoguelike
{
    public static class TypeExtensions
    {
        public static List<Type> GetAllDerivedTypes(this Type type)
        {
            return Assembly.GetAssembly(type).GetAllDerivedTypes(type);
        }

        public static List<Type> GetAllDerivedTypes(this Assembly assembly, Type type)
        {
            return assembly
                .GetTypes()
                .Where(t => t != type && type.IsAssignableFrom(t))
                .ToList();
        }
    }
}
