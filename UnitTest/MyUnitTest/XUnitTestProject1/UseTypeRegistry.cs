namespace XUnitTestProject1
{
    internal class UseTypeRegistry<TPluginType, TConcreteType> : StructureMapRegistry where TConcreteType : TPluginType
    {
        public UseTypeRegistry()
        {
            For<TPluginType>().Use<TConcreteType>();
        }
    }
}
