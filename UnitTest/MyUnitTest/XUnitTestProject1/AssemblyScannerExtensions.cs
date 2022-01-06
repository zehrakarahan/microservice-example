using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StructureMap.Graph;

namespace XUnitTestProject1
{
    public static class AssemblyScannerExtensions
    {
        internal static readonly string[] _defaultWhiteListNamespacePrefixList;

        static AssemblyScannerExtensions()
        {
            // TODO: remove ACT

            _defaultWhiteListNamespacePrefixList = new string[] { "Moon", "M2", "ACT", "One" };
        }

        /// <summary>
        /// Filter Assemblies by prefix list for ONE applications.
        /// </summary>
        /// <param name="assemblyScanner">IAssemblyScanner</param>
        /// <param name="whiteListNamespacePrefixList">default value: {"Moon", "M2", "ACT"}</param>
        public static void AssembliesFromONE(
            this IAssemblyScanner assemblyScanner,
            IEnumerable<string> whiteListNamespacePrefixList = null)
        {
            if (whiteListNamespacePrefixList == null || whiteListNamespacePrefixList.Count() < 1)
            {
                whiteListNamespacePrefixList = _defaultWhiteListNamespacePrefixList;
            }

            assemblyScanner.AssembliesFromApplicationBaseDirectory((Assembly assembly) =>
            {
                if (whiteListNamespacePrefixList.Any(x => assembly.FullName.StartsWith(x)))
                {
                    return true;
                }
                return false;
            });
        }
    }
}
