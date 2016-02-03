using Mniak.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mniak.IO.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var hs = new HybridStream(4))
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.Write($"Writing {i} ");
                    hs.WriteByte((byte)(i + 100));
                    Console.WriteLine($"advanced to {hs.Position}. Total length = {hs.Length}");
                }

                Console.WriteLine("Press <Enter> to advance to next step...");
                Console.ReadLine();

                var random = new Random();
                for (int i = 0; i < 20; i++)
                {
                    int num = random.Next(10);
                    hs.Position = num;
                    Console.WriteLine($"Stream at {num}({hs.Position}) = {hs.ReadByte()}");
                }
                Console.WriteLine("Press <Enter> to advance to next step...");
                Console.ReadLine();

                for (int i = 0; i < 20; i++)
                {
                    int num = random.Next(10);
                    hs.Seek(num, SeekOrigin.Begin);
                    Console.WriteLine($"Stream at {num}({hs.Position}) = {hs.ReadByte()}");
                }
                Console.WriteLine("Press <Enter> to advance to next step...");
                Console.ReadLine();

                for (int i = 0; i < 20; i++)
                {
                    int num = random.Next(10) - (int)hs.Position;
                    hs.Seek(num, SeekOrigin.Current);
                    Console.WriteLine($"Stream at +{num}({hs.Position}) = {hs.ReadByte()}");
                    hs.Position--;
                }
                Console.WriteLine("Press <Enter> to advance to next step...");
                Console.ReadLine();
            }

            Console.WriteLine("END");
            Console.ReadLine();
        }
    }
}
