namespace XUnitTestProject1
{
    internal class UseInstanceRegistry<TPluginType> : StructureMapRegistry where TPluginType : class
    {
        public UseInstanceRegistry(TPluginType instance)
        {
            For<TPluginType>().Use(instance);
        }
    }
}
