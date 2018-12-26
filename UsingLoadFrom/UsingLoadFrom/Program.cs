using System;
using System.Reflection;

namespace UsingLoadFrom
{
    class Class1
    {
        [STAThread]
        static void Main(string[] args)
        {
            Assembly privateAss = Assembly.LoadFrom("SimpleAssembly.dll");
            MethodInfo info = privateAss.GetTypes()[0].GetMethod("HelloWorld");
            Object obj = info.Invoke(null, null);
            Console.WriteLine("Результат выполнения метода: {0}", obj);
            Console.ReadKey();
        }
    }
}
