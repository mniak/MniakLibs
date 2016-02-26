using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mniak.Network.Tests
{
    class Program
    {
        const int PORT = 6502;
        const int NUM_ENTRIES = 10 * 1000;
        static byte[] pseudoRandom = new byte[NUM_ENTRIES];
        static Random random;

        static Program()
        {
            random = new Random();
        }
        static void Main(string[] args)
        {
            random.NextBytes(pseudoRandom);

            var tserver = Task.Run(() => Server());
            var tclient = Task.Run(() => Client());
            Task.WaitAll(tserver, tclient);
            Console.WriteLine("END");
        }

        private static void Server()
        {
            Console.WriteLine("Starting Server...");
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            socket.Listen(10);
            do
            {
                var accepted = socket.Accept();
                ServerAccepted(accepted);
            } while (true);
        }

        private static void ServerAccepted(Socket socket)
        {
            Console.WriteLine("Starting Client...");
            var wrapper = new SimpleWrapper(socket);
            wrapper.OnDataReceived += (s, e) =>
            {
                Console.WriteLine("\tS <= {0}", BitConverter.ToString(e.DataBytes));
                wrapper.Send(e.DataBytes);
            };
            wrapper.Start();
        }

        private static void Client()
        {
            // Wait the server to startup
            Task.Delay(100).Wait();

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(IPAddress.Loopback, PORT));
            var wrapper = new SimpleWrapper(socket);
            wrapper.OnDataReceived += (s, e) =>
            {
                Console.WriteLine("\tC <= {0}", BitConverter.ToString(e.DataBytes));
            };
            wrapper.Start();
            for (int i = 0; i < NUM_ENTRIES; i++)
            {
                var b = pseudoRandom[i];
                //byte[] data = new byte[b];
                //random.NextBytes(data);

                //wrapper.Send(data);
                wrapper.Send(new byte[] { b });
            }
        }
    }
}
