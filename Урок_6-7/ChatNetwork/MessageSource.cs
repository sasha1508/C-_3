using CommonChat.DTO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatNetwork
{
    public class MessageSource : IMessageSource
    {
        private UdpClient _udpClient;

        public MessageSource(int port)
        {
            _udpClient = new UdpClient(port);
        }

        public IPEndPoint CreateNewIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Any, 0);
        }

        public ChatMessage Receive(ref IPEndPoint ipEndPoint)
        {
            byte[] data = _udpClient.Receive(ref ipEndPoint);
            string jsonMessage = Encoding.UTF8.GetString(data);
            return ChatMessage.FromJson(jsonMessage);
        }


        public void SendMessage(ChatMessage chatMessage, IPEndPoint ipEndPoint)
        {
            byte[] data = Encoding.UTF8.GetBytes(chatMessage.ToJson());
            _udpClient.Send(data, data.Length, ipEndPoint);
        }
    }
}