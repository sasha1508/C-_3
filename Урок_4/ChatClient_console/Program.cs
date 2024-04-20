
using Microsoft.AspNetCore.SignalR.Client;
using SocketChat.Common.Entities;


// создаем подключение к хабу
HubConnection connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7217/chat")
    .Build();


System.Console.Write("Подключаемся к чату. Введи своё имя: ");
string nameUser = System.Console.ReadLine()?? "";


// регистрируем функцию Receive для получения данных
connection.On<string, string>("Receive", (user, message) =>
{
    Console.WriteLine(user + ": " + message);
});


try
{
    // подключемся к хабу
    await connection.StartAsync();
    Console.WriteLine("Вы вошли в чат (выход: \"exit\")");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

//Ожидаем ввода текста:
while (true)
{
    string message = System.Console.ReadLine() ?? "";
    if (message.ToLower() == "exit") return;
    if (message != "") await SendMesage(message);
}


// обработчик отправки сообщения
 async Task SendMesage(string message)
{
    try
    {
        // отправка сообщения
        SignalRMessage signalRMessage = new SignalRMessage();
        signalRMessage.Message = message;
        signalRMessage.FromUser = nameUser;

        await connection.SendAsync("Send", signalRMessage);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
