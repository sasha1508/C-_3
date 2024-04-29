using CommonChat.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatDB
{
    public class ChatContext : DbContext
    {
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public ChatContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder
                .LogTo(Console.WriteLine)
                .UseLazyLoadingProxies()
                .UseNpgsql("Host=localhost;Port=5432;Database=socket_chat_db_2;Username=test;Password=Test1234");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.Id).HasName("message_pkey");
                entity.ToTable("messages");
                entity.Property(m => m.Text).HasMaxLength(255).HasColumnName("text");
                entity.Property(m => m.FromUserId).HasColumnName("from_user_id");
                entity.Property(m => m.ToUserId).HasColumnName("to_user_id");

                entity.HasOne(m => m.FromUser).WithMany(u => u.FromMessages).HasForeignKey(m => m.FromUserId);

                entity.HasOne(m => m.ToUser).WithMany(u => u.ToMessages).HasForeignKey(m => m.ToUserId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}