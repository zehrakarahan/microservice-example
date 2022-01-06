namespace XUnitTestProject1
{
    public static class ServiceConatinerExtensions
    {
        public static IServiceContainerBuilder UseStructureMap(this IServiceContainerBuilder builder)
        {
            builder.ServiceContainerCreator = new StructureMapServiceContainerCreator();
            return builder;
        }

        public static IServiceContainerBuilder AddDefaultConventionRegistry(this IServiceContainerBuilder builder)
        {
            builder.AddRegistry<DefaultConventionRegistry>();

            return builder;
        }

        public static IServiceContainerBuilder SetCallContext(this IServiceContainerBuilder builder)
        {
            builder.BuildCompeleted += (container) =>
            {
                ServiceFactory.SetContainer(container);
            };

            return builder;
        }

        public static IServiceContainerBuilder AddService<TPluginType>(this IServiceContainerBuilder builder, TPluginType instance) where TPluginType : class
        {
            builder.Registries.Add(new UseInstanceRegistry<TPluginType>(instance));
            return builder;
        }

        public static IServiceContainerBuilder AddService<TPluginType, TConcreteType>(this IServiceContainerBuilder builder)
            where TConcreteType : TPluginType
        {
            builder.Registries.Add(new UseTypeRegistry<TPluginType, TConcreteType>());
            return builder;
        }
    }
}
