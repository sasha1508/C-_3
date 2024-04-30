using ChatApp;
using ChatDB;
using ChatNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Test
{
    internal class UnitTestClient
    {
        [Test]
        public void test1()
        {
           ChatClient chatClient = new();
        }

        [Test]
        public async Task test2()
        {
            var mock = new MockMessagesSource();
            var server = new ChatServer(mock);
            mock.InitializeServer(server);
            await server.WorkAsync();


            var chatClient = new ChatClient();
            chatClient.ReadMessage();


        }

        
    }
}
