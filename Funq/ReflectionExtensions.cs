using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Funq
{
    static class PlatformExtensions
    {
        public static bool IsInterface(this Type type)
        {
            #if (NETFX_CORE || PCL)
            return type.GetTypeInfo().IsInterface;
            #else
            return type.IsInterface;
            #endif
        }

        public static bool IsGeneric(this Type type)
        {
            #if (NETFX_CORE || PCL)
            return type.GetTypeInfo().IsGenericType;
            #else
            return type.IsGenericType;
            #endif
        }

        public static Type BaseType(this Type type)
        {
            #if (NETFX_CORE || PCL)
            return type.GetTypeInfo().BaseType;
            #else
            return type.BaseType;
            #endif
        }

        public static Type[] GetTypeInterfaces(this Type type)
        {
            #if (NETFX_CORE || PCL)
            return type.GetTypeInfo().ImplementedInterfaces.ToArray();
            #else
            return type.GetInterfaces();
            #endif
        }

        public static PropertyInfo[] GetTypesPublicProperties(this Type subType)
        {
            #if (NETFX_CORE || PCL)
            var pis = new List<PropertyInfo>();
            foreach (var pi in subType.GetRuntimeProperties())
            {
                var mi = pi.GetMethod ?? pi.SetMethod;
                if (mi != null && mi.IsStatic) 
                    continue;
                pis.Add(pi);
            }
            return pis.ToArray();
            #else
            return subType.GetProperties(
                BindingFlags.FlattenHierarchy |
                BindingFlags.Public |
                BindingFlags.Instance);
            #endif
        }

        public static Type[] GetTypeGenericArguments(this Type type)
        {
            #if (NETFX_CORE || PCL)
            return type.GenericTypeArguments;
            #else
            return type.GetGenericArguments();
            #endif
        }
    }

    static class ReflectionExtensions
    {
        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            if (type.IsInterface())
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);

                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetTypeInterfaces())
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetTypesPublicProperties();

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetTypesPublicProperties()
                .Where(t => t.GetIndexParameters().Length == 0) // ignore indexed properties
                .ToArray();
        }

        public static Type FirstGenericTypeDefinition(this Type type)
        {
            var genericType = type.FirstGenericType();
            return genericType != null ? genericType.GetGenericTypeDefinition() : null;
        }

        public static Type FirstGenericType(this Type type)
        {
            while (type != null)
            {
                if (type.IsGeneric())
                    return type;

                type = type.BaseType();
            }
            return null;
        }
    }
}