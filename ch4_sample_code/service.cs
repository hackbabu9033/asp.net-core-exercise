using System;
using System.Collections.Generic;
using System.Text;

namespace ch4_sample_code
{
    public interface ICar { }
    public interface ICarTire { }
    public interface IEngine { }

    public interface IUnRegistableService { }

    public class Base : IDisposable
    {
        public Base()
        {
            Console.WriteLine($"{this.GetType().Name} service is created");
        }
        public void Dispose()
        {
            Console.WriteLine("Car service is disposed");
        }
    }

    public class Car : Base, ICar, IDisposable
    {
        public ICarTire CarTire { set; get; }
        public IEngine Engine { set; get; }

        public Car(ICarTire carTire, IEngine engine)
        {
            CarTire = carTire;
            Engine = engine;
        }
    }
    public class CarTire : Base, ICarTire, IDisposable { }


    public class Engine : Base, IEngine, IDisposable { }

    public class UnRegistableService : Base, IUnRegistableService
    { 
        private UnRegistableService()
        {

        }

        public static readonly UnRegistableService instance = new UnRegistableService();
    }


}
