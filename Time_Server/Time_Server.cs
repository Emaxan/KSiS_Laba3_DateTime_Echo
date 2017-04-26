using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Time_Server {
	internal class Time_Server {
		//port = 13

		private static void Main(string[] args) {
			Socket sock = null;
			var i = OpenSocket(ref sock);
			sock.Bind(new IPEndPoint(IPAddress.Any, 13));
			if(i == 1)
				while(true) {
					sock.Listen(50);
					var cli = sock.Accept();
					var time = DateTime.UtcNow.ToString();
					Console.WriteLine("Receive from :" + cli.RemoteEndPoint);
					cli.Send(Encoding.UTF8.GetBytes(time));
					cli.Close();
				}
			if(i != 2) return;
			while(true) {
				while(sock.Available < 1) { }
				var buf = new byte[65535];
				EndPoint ep = new IPEndPoint(IPAddress.Any, 0);
				Console.Write("Receiving...");
				var length = sock.ReceiveFrom(buf, ref ep);
				Console.WriteLine($" from {ep}");
				if(length != 0) continue;
				var time = DateTime.UtcNow.ToString();
				sock.SendTo(Encoding.UTF8.GetBytes(time), ep);
			}
		}

		private static int OpenSocket(ref Socket sock) {
			Console.WriteLine("Press 1 to select TCP or 2 to select UDP.");
			ProtocolType protocol;
			SocketType socket;
			int protocolN = 0;
			switch(Console.ReadKey().Key) {
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
