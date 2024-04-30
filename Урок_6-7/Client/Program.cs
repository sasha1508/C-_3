using ChatApp;
using ChatNetwork;
using CommonChat.DTO;
using CommonChat.Model;
using System;
using System.Net;
using System.Xml.Linq;


//Подключаемся к серверу:
//MessageSource messageSource = new(8889);
//IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);

ChatClient chatClient = new ChatClient();
chatClient.Connect();



chatClient.ReadMessage();

while (true)
{
    chatClient.SendMessage();
}

