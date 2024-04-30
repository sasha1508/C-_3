

using ChatApp;
using ChatNetwork;


//Запускаем сервер:

MessageSource messageSource = new(8888);
ChatServer chatServer = new(messageSource);
await chatServer.WorkAsync();