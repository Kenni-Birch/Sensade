using DAL;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Class1 class1 = new Class1();

            class1.Insert(4, false, 201);

            Console.WriteLine("Hello, World!");
        }
    }
}