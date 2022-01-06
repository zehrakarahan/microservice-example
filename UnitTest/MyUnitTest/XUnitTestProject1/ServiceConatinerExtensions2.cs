using System;

namespace XUnitTestProject1
{
    public static class ServiceConatinerExtensions2
    {
        public static IServiceContainerBuilder AddRegistry(this IServiceContainerBuilder builder, IServiceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            builder.Registries.Add(registry);
            return builder;
        }

        public static IServiceContainerBuilder AddRegistry<T>(this IServiceContainerBuilder builder) where T: IServiceRegistry, new ()
        {
            builder.Registries.Add(new T());
            return builder;
        }
    }
}
