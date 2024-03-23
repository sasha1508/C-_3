using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Урок_1_Data;


namespace Client
{
    internal class ClientHandler
    {
        internal void SendMessage ( string ip , int port , string from , string messageText )
        {
            using ( var client = new UdpClient( ) )
            {
                var remoteEndPoint = new IPEndPoint( IPAddress.Parse( ip ) , port );
                Message message = new( ) { Text = messageText , DateTime = DateTime.Now , NickNameFrom = from , NickNameTo = "Server" };
                string json = message.SerializeMessageToJson( );
                byte[ ] bytes = Encoding.UTF8.GetBytes( json );
                client.Send( bytes , bytes.Length , remoteEndPoint );//отправка
                var result = client.Receive( ref remoteEndPoint );//ожидание ответа
                Console.WriteLine( $"Ответ сервера : {Encoding.UTF8.GetString( result )}" );
            }
        }

    }

}
