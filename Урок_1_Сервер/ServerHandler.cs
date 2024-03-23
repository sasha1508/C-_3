using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Урок_1_Data;

namespace Урок_1_Data
{
    internal class ServerHandler 
    {
        public void AwaitMessage ( )
        {
            using ( var client = new UdpClient( 1234 ) )
            {
                IPEndPoint anyEndPoint = new ( IPAddress.Any , 0 );
                byte[ ] result = client.Receive( ref anyEndPoint );
                if ( result != null )
                {
                    string messageText = Encoding.UTF8.GetString( result );
                    var message = Message.DeserializeFromJsonToMessage( messageText );
                    message?.PrintMessage( );

                    string responseMessage = $"Сервер получил сообщение:\n{message?.DateTime.ToLongTimeString()}\nот {message?.NickNameFrom}\nдля {message?.NickNameTo}";
                    byte[ ] response = Encoding.UTF8.GetBytes( responseMessage );
                    client.Send( response , response.Length , anyEndPoint );
                }
            }
        }
                                                                                                                                                      

    }
}
