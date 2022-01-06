using NSubstitute;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Logging;
namespace XUnitTestProject1
{
    public class ContainerTest
    {
        private readonly static IServiceContainer _rootContainer = Configure();
        private readonly List<Type> _scopedMockTypes = new List<Type>();

        static ContainerTest()
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            ConfigureLogger();
        }

        public static IServiceContainer Configure()
        {
            var container = new ServiceContainerBuilder()
                .UseStructureMap()
                .AddRegistry<TextMessageRegistry>()
                .Build();

            return container;
        }

        protected T Instance<T>() where T : class
        {
            return _rootContainer.GetInstance<T>();
        }

        protected void EnsureMock<TInterface>(TInterface obj)
               where TInterface : class
        {
            _rootContainer.Configure(new UseInstanceRegistry<TInterface>(obj));
            _scopedMockTypes.Add(typeof(TInterface));
        }

        protected void EnsureMock<T>(bool mockForParts = false) where T : class
        {
            var type = typeof(T);

            if (!_scopedMockTypes.Contains(type))
            {
                if (type.IsInterface)
                {
                    _rootContainer.Configure(new UseInstanceRegistry<T>(Substitute.For<T>()));
                }
                else if (type.IsClass)
                {
                    ConstructorInfo constructor = type.GetConstructors().FirstOrDefault();
                    var arguments = new List<object>();
                    if (constructor != null)
                    {
                        ParameterInfo[] parameters = constructor.GetParameters();
                        foreach (var p in parameters)
                        {
                            arguments.Add(_rootContainer.GetInstance(p.ParameterType));
                        }
                    }
                    var instance = mockForParts ? Substitute.ForPartsOf<T>(arguments.ToArray()) : Substitute.For<T>(arguments.ToArray());

                    _rootContainer.Configure(new UseInstanceRegistry<T>(instance));
                }
                _scopedMockTypes.Add(typeof(T));
            }
        }

        protected void EnsureMock<TInterface, UClass>() where UClass : TInterface
        {
            _rootContainer.Configure(new UseTypeRegistry<TInterface, UClass>());
            _scopedMockTypes.Add(typeof(TInterface));
        }

        protected void EnsureMock<TInterface, UClass>(UClass obj)
            where UClass : class, TInterface
            where TInterface : class
        {
            _rootContainer.Configure(new UseInstanceRegistry<TInterface>(obj));
            _scopedMockTypes.Add(typeof(TInterface));
        }

        public void EnsureMockForClass<TClass>()
            where TClass : class
        {
            var instance = Substitute.For<TClass>();

            _rootContainer.Configure(new UseInstanceRegistry<TClass>(instance));
            _scopedMockTypes.Add(typeof(TClass));
        }

        protected T InstanceForMock<T>() where T : class
        {
            return _rootContainer.GetInstance<T>();
        }

        protected T InstanceForMock<T>(string instanceKey) where T : class
        {
            return _rootContainer.GetInstance<T>(instanceKey);
        }

        private static void ConfigureLogger()
        {
            var loggerFactory = Substitute.For<ILoggerFactory>();
            Logger.SetLoggerFactory(loggerFactory);
        }
    }
}
