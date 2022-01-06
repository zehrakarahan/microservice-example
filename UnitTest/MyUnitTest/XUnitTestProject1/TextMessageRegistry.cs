using Castle.DynamicProxy.Internal;
using MyUnitTest;
using NSubstitute;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using System;
using System.Linq;

namespace XUnitTestProject1
{
    public class TextMessageRegistry : StructureMapRegistry
    {
        public TextMessageRegistry()
        {
            //For<ICalculator>().Use(Substitute.For<ICalculator>()).Singleton();
            //For<ITextMessageProvider>().Use(Substitute.For<ITextMessageProvider>()).Singleton();
            //For<IUrlShortenerService>().Use(Substitute.For<IUrlShortenerService>()).Singleton();
            //For<IMongoDbContext>().Use(Substitute.For<IMongoDbContext>()).Singleton();
            //For<IMessageService>().Use(Substitute.For<IMessageService>());
            //For<IUrlService>().Use(Substitute.For<IUrlService>());

            Scan(scan =>
            {
                scan.Assembly("MyUnitTest");
                scan.With(new RepositoryRegistrationConvention());
                scan.With(new ManagerRegistraion());
            });
        }
    }
    public class RepositoryRegistrationConvention : IRegistrationConvention
    {
        public void ScanTypes(TypeSet types, StructureMap.Registry registry)
        {
            foreach (var type in types.AllTypes().Where(type => type.Name.EndsWith("Repository") && type.IsClass))
            {
                var interfaces = type.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (@interface.Name == "I" + type.Name)
                    {
                        registry.For(@interface).Add(Substitute.For(new Type[] { @interface }, new object[] { })).Transient();
                    }
                }

            }
        }
    }
    public class ManagerRegistraion : IRegistrationConvention
    {
        public void ScanTypes(TypeSet types, StructureMap.Registry registry)
        {
            foreach (var type in types.AllTypes().Where(type => type.Name.EndsWith("Service") && type.IsClass))
            {
                if (!type.Name.EndsWith("MessageService"))
                {
                    registry.For(type);
                    var interfaces = type.GetAllInterfaces();
                    foreach (var @interface in interfaces)
                    {
                        registry.For(@interface).Use(type).Transient();
                    }
                }
            }
        }
    }
}
