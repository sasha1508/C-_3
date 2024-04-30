using ChatNetwork;
using CommonChat.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ChatApp
{
    public class ChatClient
    {
        MessageSource? messageSource;
        IPEndPoint? ipEndPoint;
        string neme;
        bool _isWork = true;

        /// <summary>
        /// Соединение с сервером
        /// </summary>
        public void Connect()
        {
            Console.Write("Введите ваше имя:");
            neme = Console.ReadLine() ?? "";

            Console.Write("Введите ваш номер порта:");
            int port = Convert.ToInt32(Console.ReadLine());

            messageSource = new(port);
            ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);

            ChatMessage chatMessage = new()
            {
                FromName = neme,
                Command = Command.Register
            };
            messageSource.SendMessage(chatMessage, ipEndPoint);
        }

        /// <summary>
        /// Отправка сообщений
        /// </summary>
        public void SendMessage()
        {
            string message = Console.ReadLine() ?? "";
            
            Console.Write("Введите имя получателя:");
            string ToName = Console.ReadLine() ?? "";            

            ChatMessage chatMessage = new()
            {
                Id = 6,
                FromName = neme,
                ToName = ToName,
                Text = message,
                Command = Command.Message
            };
            messageSource?.SendMessage(chatMessage, ipEndPoint);
        }

        public void ReadMessage()
        {
            Thread myThread = new Thread(WorkAsync); //Создаем новый объект потока (Thread)
            myThread.Start(); //запускаем поток
        }


        /// <summary>
        /// Цикл чтения чата
        /// </summary>
        /// <returns></returns>
        public void WorkAsync()
        {
            while (_isWork)
            {
                try
                {
                    var remoteIpEndPoint = messageSource?.CreateNewIPEndPoint();
                    var chatMessage = messageSource.Receive(ref remoteIpEndPoint);
                    if (chatMessage != null)
                    {
                        Console.WriteLine(chatMessage.FromName + ": " + chatMessage.Text);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace + " " + ex.Message);
                }
            }
        }

        public void Stop()
        {
            _isWork = false;
        }
    }
}
