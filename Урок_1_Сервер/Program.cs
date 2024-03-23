namespace Урок_1_Data
{
    internal class Program
    {
        static void Main (  )
        {
            Console.WriteLine( "Сервер ожидает сообщение от Client..." );

            ServerHandler serverHandler = new( );

            while ( true )
            {
                serverHandler.AwaitMessage( );
            }
        }
    }
}
