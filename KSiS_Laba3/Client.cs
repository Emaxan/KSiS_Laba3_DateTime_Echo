using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace KSiS_Laba3 {
	internal class Client {
		private static void Main(string[] args) {
			int prot = 0;
			int type = 0;
			Console.WriteLine("Time(1) or Echo(2)?");
			switch(Console.ReadKey().Key) {
				case ConsoleKey.D1:
				case ConsoleKey.NumPad1:
					prot = 1;
					break;
				case ConsoleKey.D2:
				case ConsoleKey.NumPad2:
					prot = 2;
					break;
				default:
					Console.WriteLine("Wrong input.");
					Console.ReadKey();
					return;
			}
			Console.WriteLine("TCP(1) or UDP(2)?");
			switch(Console.ReadKey().Key) {
				case ConsoleKey.D1:
				case ConsoleKey.NumPad1:
					type = 1;
					break;
				case ConsoleKey.D2:
				case ConsoleKey.NumPad2:
					type = 2;
					break;
				default:
					Console.WriteLine("Wrong input.");
					Console.ReadKey();
					return;
			}

			if(prot == 1) {
				if(type == 1) {
					var sock1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					sock1.Connect(GetMyIp(), 13);
					var buf1 = new byte[65535];
					var length1 = sock1.Receive(buf1);
					Console.WriteLine("\n" + Encoding.UTF8.GetString(buf1).Substring(0, length1));
					Console.ReadKey();
					return;
				}
				var sock2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				sock2.Bind(new IPEndPoint(GetMyIp(), 0));
				sock2.SendTo(Encoding.UTF8.GetBytes(""), new IPEndPoint(GetMyIp(), 13));
				var buf2 = new byte[65535];
				var length2 = sock2.Receive(buf2);
				Console.WriteLine("\n" + Encoding.UTF8.GetString(buf2).Substring(0, length2));
				Console.ReadKey();
				return;
			}
			if(type == 1) {
				var sock3 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				sock3.Connect(GetMyIp(), 7);
				string s3;
				while((s3 = Console.ReadLine())!="close") {
					if(s3 != null)
						sock3.Send(Encoding.UTF8.GetBytes(s3));
					var buf3 = new byte[s3.Length];
					var length3 = sock3.Receive(buf3);
					Console.WriteLine("\n" + Encoding.UTF8.GetString(buf3).Substring(0, length3));
				}
				sock3.Close();
				Console.ReadKey();
				return;
			}
			var sock4 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			sock4.Bind(new IPEndPoint(GetMyIp(), 0));
			string s4;
			while((s4 = Console.ReadLine()) != "close") {
				if(s4 != null)
					sock4.SendTo(Encoding.UTF8.GetBytes(s4), new IPEndPoint(GetMyIp(), 7));
				var buf4 = new byte[65535];
				var length4 = sock4.Receive(buf4);
				Console.WriteLine("\n" + Encoding.UTF8.GetString(buf4).Substring(0, length4));
			}
			Console.ReadKey();
			return;
		}

		public static IPAddress GetMyIp() {
			return Dns.GetHostAddresses(Dns.GetHostName())
					.Where(address => address.AddressFamily == AddressFamily.InterNetwork)
					.ToArray()[0];
		}
	}
}
