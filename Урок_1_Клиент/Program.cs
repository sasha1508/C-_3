namespace Client
{
    internal class Program
    {
        static void Main (  )
        {
            ClientHandler clientHandler = new ( );

            string? messageText;
            do
            {
                Console.Clear( );
                Console.WriteLine( "Введите текст : " );
                messageText = Console.ReadLine( );
            }
            while ( string.IsNullOrEmpty( messageText ) );

            clientHandler.SendMessage( "127.0.0.1" , 1234 , "Client" , messageText );

        }
    }
}
