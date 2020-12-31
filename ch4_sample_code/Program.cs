using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace ch4_sample_code
{
    class Program
    {
        static void Main(string[] args)
        {
            #region service scope validation

            var rootProvider = new ServiceCollection()
                .AddScoped<ICarTire, CarTire>()
                .AddScoped<IEngine, Engine>()
                .AddSingleton<ICar, Car>()
                .AddSingleton<IUnRegistableService, UnRegistableService>()
                .BuildServiceProvider(true); // set true to validate scope
            // 因為Car依賴CarTire和Engine，理論上彼此所註冊的生命週期需要一致
            var childScope = rootProvider.CreateScope().ServiceProvider;
            //rootProvider.GetService<ICarTire>();
            //rootProvider.GetService<ICar>();
            //childScope.GetService<ICarTire>();
            //childScope.GetService<ICar>();
            var carTireService = ResolveService<ICarTire>(rootProvider);
            var carService = ResolveService<ICar>(rootProvider);
            var carTireServiceInScope = ResolveService<ICarTire>(childScope);
            var carServiceInScope = ResolveService<ICar>(childScope);
            var test = ResolveService<IUnRegistableService>(rootProvider);
            #endregion

            #region service lifetime

            //using (var rootProvider = new ServiceCollection()
            //    .AddScoped<ICarTire, CarTire>()
            //    .AddScoped<IEngine, Engine>()
            //    .AddSingleton<ICar, Car>()
            //    .BuildServiceProvider())
            //{
            //    using (var scope = rootProvider.CreateScope())
            //    {
            //        var scopeProvider = scope.ServiceProvider;
            //        scopeProvider.GetService<ICarTire>();
            //        scopeProvider.GetService<IEngine>();
            //        scopeProvider.GetService<ICar>();
            //        // since Icar has dependency ICarTire and IEngine , 
            //        // the singleton service ICarTire and IEngine will create for this Icar service
            //        Console.WriteLine("service scope is disposed");
            //    }
            //    Console.WriteLine("root scope is disposed");
            //}
            #endregion

            #region serviceDescriptor

            var serviceCollection = new ServiceCollection();
            var descriptor1 = new ServiceDescriptor(typeof(ICar), typeof(Car), ServiceLifetime.Scoped);
            var descriptor2 = ServiceDescriptor.Singleton<ICar, Car>();
            ServiceCollectionDescriptorExtensions.Add(serviceCollection, descriptor1);
            ServiceCollectionDescriptorExtensions.TryAdd(serviceCollection, descriptor2);
            #endregion

        }

        public static object ResolveService<T>(IServiceProvider serviceProvider)
        {
            try
            {
                var serviceInstance = serviceProvider.GetService<T>();
                return serviceInstance;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
        }
    }
}
