using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonChat.DTO;

namespace ChatNetwork
{
    public interface IMessageSource
    {
        /// <summary>
        /// Отправляем сообщение
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <param name="ipEndPoint"></param>
        void SendMessage(ChatMessage chatMessage, IPEndPoint ipEndPoint);

        /// <summary>
        /// Получаем сообщение
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <returns></returns>
        ChatMessage Receive(ref IPEndPoint ipEndPoint);

        /// <summary>
        /// Полечаем новый IPEndPoint от клиента
        /// </summary>
        /// <returns></returns>
        IPEndPoint CreateNewIPEndPoint();
    }
}
