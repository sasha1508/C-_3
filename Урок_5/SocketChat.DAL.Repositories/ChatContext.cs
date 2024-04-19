using Microsoft.EntityFrameworkCore;
using SocketChat.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketChat.DAL.Repositories
{
    public class ChatContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SignalRMessage> Messages { get; set; }

        public ChatContext() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=socket_chat_db_1;Username=test;Password=Test1234");
        }
    }
}
