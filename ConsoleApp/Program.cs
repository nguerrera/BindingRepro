using ClassLibrary1;
using ClassLibrary2;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            // NOTE: Can't use app base at runtime to resolve assemblies of the same simple name.
            // Here we use assembly resolve event, could also be in GAC among other tricks
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            // NOTE: Must be in separate method or resolve failure will trigger during JIT of main before
            // our resolver is wired.
            PrintTypes();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void PrintTypes()
        {
            Console.WriteLine(typeof(Class1).AssemblyQualifiedName);
            Console.WriteLine(typeof(Class2).AssemblyQualifiedName);
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var asm1 = Assembly.LoadFrom(@"..\..\..\..\ClassLibrary1\bin\Debug\net46\ClassLibrary.dll");
            var asm2 = Assembly.LoadFrom(@"..\..\..\..\ClassLibrary2\bin\Debug\net46\ClassLibrary.dll");

            if (args.Name == asm1.FullName)
                return asm1;

            if (args.Name == asm2.FullName)
                return asm2;

            return null;
        }
    }
}
