using System.Net;
using ChatDB;
using ChatNetwork;
using CommonChat.DTO;
using CommonChat.Model;

namespace ChatApp
{
    public class ChatServer
    {
        private IMessageSource _messageSource;
        public Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
        private bool _isWork = true;

        public ChatServer(IMessageSource iMessageSource)
        {
            _messageSource = iMessageSource;
        }

        /// <summary>
        /// Обработчик сообщений
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <param name="ipEndPoint"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task ProcessMessageAsync(ChatMessage chatMessage, IPEndPoint ipEndPoint)
        {
            switch (chatMessage.Command)
            {
                case Command.Message:
                    await ReplyMessageAsync(chatMessage);
                    break;
                case Command.Confirmation:
                    await ConfirmationAsync(chatMessage.Id);
                    break;
                case Command.Register:
                    await RegisterAsync(chatMessage, ipEndPoint);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Подтверждение
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ConfirmationAsync(int? id)
        {
            Console.WriteLine($"Message id {id}");
            using (var context = new ChatContext())
            {
                var message = context.Messages.FirstOrDefault(x => x.Id == id);
                if (message != null)
                {
                    message.IsReceived = true;
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Пересыл письма получателю
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        public async Task ReplyMessageAsync(ChatMessage chatMessage)
        {
            if (clients.TryGetValue(chatMessage.ToName, out IPEndPoint ipEndPoint))
            {
                using (var context = new ChatContext())
                {
                    var fromName = context.Users.FirstOrDefault(x => x.Name == chatMessage.FromName);
                    var toName = context.Users.FirstOrDefault(x => x.Name == chatMessage.ToName);
                    var message = new Message
                    {
                        Text = chatMessage.Text,
                        FromUser = fromName,
                        ToUser = toName,
                        IsReceived = false
                    };

                    await context.Messages.AddAsync(message);
                    await context.SaveChangesAsync();
                    chatMessage.Id = message.Id;
                }

                _messageSource.SendMessage(chatMessage, ipEndPoint);
                Console.WriteLine($"Send message from {chatMessage.FromName} to {chatMessage.ToName}");
            }
        }

        /// <summary>
        /// Регистрация пользователя в базе данных
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <param name="ipEndPoint"></param>
        /// <returns></returns>
        public async Task RegisterAsync(ChatMessage chatMessage, IPEndPoint ipEndPoint)
        {
            Console.WriteLine($"{chatMessage.FromName}, message register name");
            clients.Add(chatMessage.FromName, ipEndPoint);

            using (var context = new ChatContext())
            {
                var conAny = context.Users.Any(x => x.Name == chatMessage.FromName);
                if (conAny)
                {
                    Console.WriteLine("Пользователь уже существует в базе данных");
                    return;
                }

                await context.Users.AddAsync(new User()
                {
                    Name = chatMessage.FromName
                });
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Останавливаем сервер
        /// </summary>
        public void Stop()
        {
            _isWork = false;
        }

        /// <summary>
        /// Рабочий цикл чата
        /// </summary>
        /// <returns></returns>
        public async Task WorkAsync()
        {
            Console.WriteLine("Я сервер. Ожидаю сообщение от клиента.");
            while (_isWork)
            {
                try
                {
                    var remoteIpEndPoint = _messageSource.CreateNewIPEndPoint();
                    var chatMessage = _messageSource.Receive(ref remoteIpEndPoint);
                    if (chatMessage != null)
                    {
                        Console.WriteLine(chatMessage.FromName + " -> " + chatMessage.ToName + ": " + chatMessage.Text);
                        await ProcessMessageAsync(chatMessage, remoteIpEndPoint);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace + " " + ex.Message);
                }
            }
        }
    }
}