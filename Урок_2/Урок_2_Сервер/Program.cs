using Client;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo consoleKeyInfo;
            UdpClient udpClient = new UdpClient(9999);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Сервер работает...");
            while (Console.KeyAvailable == false)
            { 
                if (udpClient.Available == 0)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                var json = Encoding.UTF8.GetString(buffer);
                Message message = Message.DeserializeMessageFromJson(json);
                Console.WriteLine(message);
                int count = buffer.Length;
                byte[] data = BitConverter.GetBytes(count);
                udpClient.Send(data, data.Length, iPEndPoint);
            } 
        }
    }
}
