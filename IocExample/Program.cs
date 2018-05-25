using IocExample.Classes;

namespace IocExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var sqlConnectionFactory = new SqlConnectionFactory("SQL Connection", logger);
            var createUserHandler = new CreateUserHandler(new UserService(new QueryExecutor(sqlConnectionFactory), new CommandExecutor(sqlConnectionFactory), new CacheService(logger, new RestClient("API KEY"))), logger);

            createUserHandler.Handle();
        }
    }
}
