using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var msg = new Message();
            msg.SenderName = args[0];
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(args[1]), 9999);
            UdpClient udpClient = new UdpClient();
            udpClient.Client.ReceiveTimeout = 1000;

            while (true)
            {
                Console.Write("Введите сообщение: ");
                msg.Text = Console.ReadLine();
                if (msg.Text?.ToLower() == "exit")
                {
                    break;
                }
                string json = msg.SerializeMessageToJson();
                Console.WriteLine(json);
                byte[] bufferOut = Encoding.UTF8.GetBytes(json);
                udpClient.Send(bufferOut,bufferOut.Length,iPEndPoint);

                bool messageReceived = false;
                try
                {
                    byte[] bufferIn = udpClient.Receive(ref iPEndPoint);
                    int count = BitConverter.ToInt32(bufferIn);
                    if (count == bufferOut.Length)
                    {
                        Console.WriteLine("Сообщение доставлено...");
                    }

                }
                catch
                {
                    Console.WriteLine("Сообщение не доставлено...");
                }
            }
        }
    }
}
