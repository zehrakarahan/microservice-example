using System.Collections.Generic;

namespace XUnitTestProject1
{
    internal class DefaultConventionRegistry : StructureMapRegistry
    {
        public DefaultConventionRegistry()
        {
            Scan(x =>
            {
                x.AssembliesFromONE();
                x.WithDefaultConventions();
            });
        }

        public DefaultConventionRegistry(IEnumerable<string> whiteListNamespacePrefixList)
        {
            Scan(x =>
            {
                x.AssembliesFromONE(whiteListNamespacePrefixList);
                x.WithDefaultConventions();
            });
        }
    }
}
