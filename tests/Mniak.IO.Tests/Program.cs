using Mniak.Core;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace Mniak.IO.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestHybridStream();
            TestHybridStreamZip();
            //TestActionLock();

            Console.WriteLine("END");
            Console.ReadLine();
        }

        private static void TestHybridStreamZip()
        {
            byte[] bhs, bms;
            using (var hs = new HybridStream())
            using (var ms = new MemoryStream())
            {
                SaveZip(hs);
                SaveZip(ms);

                bhs = hs.ToArray();
                bms = ms.ToArray();
            }
            var diff = Enumerable.Range(0, Math.Max(bhs.Length, bms.Length))
                 .Where(i => i > bhs.Length || i > bms.Length || bhs[i] != bms[i]);

            if (diff.Count() > 0)
                Console.WriteLine("Differences:");
            else
                Console.WriteLine("No differences found!");

            foreach (var i in diff)
            {
                Console.WriteLine($"Byte {i} => H: {bhs[i]}, M: {bms[i]}");
            }



        }

        private static void SaveZip(Stream outStream)
        {
            using (var zip = new ZipArchive(outStream, ZipArchiveMode.Create, true))
            {
                var infoEntry = zip.CreateEntry("file.txt");
                using (var entryStream = infoEntry.Open())
                using (var fileStream = File.OpenRead(@"file.txt"))
                {
                    fileStream.CopyTo(entryStream);
                }
            }
        }

        private static void TestHybridStream()
        {
            using (var hs = new HybridStream(0))
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
        }
    }
}
