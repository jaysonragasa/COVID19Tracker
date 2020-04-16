using covid19phlib.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using System;

namespace covid19phlib.Services
{
    public class IoC : IIoC
    {
        public static IoC I { get; set; } = new IoC();
        public IoC()
        {

        }

        public void Reg<T>() where T : class
        {
            SimpleIoc.Default.Register<T>();
        }

        public void Reg<T, TImp>(bool createInstanceImmediately = false) where T : class where TImp : class, T
        {
            SimpleIoc.Default.Register<T, TImp>(createInstanceImmediately);
        }

        public T GI<T>() where T : class
        {
            return SimpleIoc.Default.GetInstance<T>();
        }

        public void Reg<T>(Func<T> factory) where T : class
        {
            SimpleIoc.Default.Register<T>(factory);
        }
    }
}
