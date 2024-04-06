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
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Recieve(token);

            Console.WriteLine("Для остановки сервера наберите \"STOP\"");
            string stopString = "";
            while (stopString != "stop")
            {
                stopString = (Console.ReadLine()?? "").ToLower();
            }
            Console.WriteLine("Введена команда остановки сервера - останавливаем сервер, завершаем приложение...");
            cts.Cancel();
            Console.WriteLine("Сервер остановлен, приложение завершено");
        }
        static async void Recieve(CancellationToken cancellationToken)
        {
            UdpClient udpClient = new UdpClient(9999);
            Console.WriteLine("Сервер работает...");
            while (true)
            {
                var result = await udpClient.ReceiveAsync(cancellationToken);
                var json = Encoding.UTF8.GetString(result.Buffer);
                Message message = Message.DeserializeMessageFromJson(json);
                Console.WriteLine(message);
                int count = result.Buffer.Length;
                byte[] data = BitConverter.GetBytes(count);
                udpClient.Send(data, data.Length, result.RemoteEndPoint);
            }
        }
    }
}
