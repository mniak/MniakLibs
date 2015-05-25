Mniak.Network; Library for .net Networking with ease
======================================================
The focus of the Mniak.Network library is to provide an easy way to work with sockets in .NET.

In many ways it has always difficult to work with sockets in .NET:
+ Receive data. (When not using byte-by-byte approach, in some times you get stuck)
+ Detect when the other side disconnected.
+ Work asynchronously
+ etc.

Wrappers
---------------------
###SimpleWrapper
The `SimpleWrapper` sends and receives the messages as plain as you provide. There will be no information about the size of the message, and therefore, there will be no control over the completeness of it.

###HeaderedWrapper
The `HeaderedWrapper` will send the size of the message before of it (called header). The header size can be changed by using the `HeaderSize` property

Installation
-----------------------
TODO: Create nuget package

Examples
----------------------
###Simple echo server
    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
    {
        socket.Connect(new IPEndPoint(IPAddress.Any, 4040));
        using (Wrapper wrapper = new HeaderedWrapper())
        {
            wrapper.OnReceived += (data) => {
                string message = Encoding.Default.GetString(data);
                string responseMessage = String.Format("Echo: '{0}'", message);
                wrapper.Send(Encoding.Default.GetBytes(responseMessage));
            };
            Console.WriteLine("Echo server Started. Press any key to exit.");
            Console.ReadKey();
        }
    }


###Simply send a message
    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
    {
        socket.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.20"), 4040));
        using (Wrapper wrapper = new HeaderedWrapper())
        {
            wrapper.Start(socket);
            wrapper.Send(Encoding.Default.GetBytes("Message sent"));
        }
    }
Contributing
---------------------
Contributions are always welcome!
Just fork this repository, make your changes and do a pull request.

###Mailing list
TODO: Create mailing list

TODO
----------------------
+ Remove de need of explicitly convert string to bytes and vice-versa;
+ Improve this README;
+ Create examples
+ Create a wrapper to work with terminated messages
+ Option to choose the endianness (byte order) of the header in the `HeaderedWrapper`.
+ Add SSL support
