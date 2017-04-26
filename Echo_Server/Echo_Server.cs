using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Echo_Server {
	class Echo_Server {//port = 7

		static void Main(string[] args) {
			Socket sock = null;
			var i = OpenSocket(ref sock);
			sock.Bind(new IPEndPoint(IPAddress.Any, 7));
			if (i == 1) {
				while(true) {
					Socket cli;
					do {
						sock.Listen(50);
						cli = sock.Accept();
						var buf = new byte[65535];
						Console.WriteLine("Receive from :" + cli.RemoteEndPoint);
						var length = cli.Receive(buf);
						Array.Resize(ref buf, length);
						cli.Send(buf);
					}
					while(sock.Connected);
					Console.WriteLine("Close :" + cli.RemoteEndPoint);
					cli.Close();
				}
			}
			if(i != 2) return;
			while(true) {
				while(sock.Available < 1) { }
				var buf = new byte[65535];
				EndPoint ep = new IPEndPoint(IPAddress.Any, 0);
				Console.Write("Receiving...");
				var length = sock.ReceiveFrom(buf, ref ep);
				Array.Resize(ref buf, length);
				Console.WriteLine($" from {ep}");
				sock.SendTo(buf, ep);
			}
		}

		private static int OpenSocket(ref Socket sock) {
			Console.WriteLine("Press 1 to select TCP or 2 to select UDP.");
			ProtocolType protocol;
			SocketType socket;
			int protocolN = 0;
			switch (Console.ReadKey().Key) {
				case ConsoleKey.D1:
				case ConsoleKey.NumPad1:
					protocol = ProtocolType.Tcp;
					socket = SocketType.Stream;
					protocolN = 1;
					Console.WriteLine("\nTCP Socket created!");
					break;
				case ConsoleKey.D2:
				case ConsoleKey.NumPad2:
					protocol = ProtocolType.Udp;
					socket = SocketType.Dgram;
					protocolN = 2;
					Console.WriteLine("\nUDP Socket created!");
					break;
				default:
					Console.WriteLine("Socket don't opened.");
					return protocolN;
			}
			sock = new Socket(AddressFamily.InterNetwork, socket, protocol);
			return protocolN;
		}
	}
}
