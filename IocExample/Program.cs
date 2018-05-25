using IocExample.Classes;
using Ninject;
using System;
namespace IocExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //var logger = new ConsoleLogger();
            //var sqlConnectionFactory = new SqlConnectionFactory("SQL Connection", logger);
            //var createUserHandler = new CreateUserHandler(new UserService(new QueryExecutor(sqlConnectionFactory), new CommandExecutor(sqlConnectionFactory), new CacheService(logger, new RestClient("API KEY"))), logger);

            
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<RestClient>().ToSelf()
                                     .WithConstructorArgument("apiKey");
            ninjectKernel.Bind<ILogger>().To<ConsoleLogger>();
            ninjectKernel.Bind<CacheService>().ToSelf();
            ninjectKernel.Bind<IConnectionFactory>()
                                     .ToConstructor(x => new SqlConnectionFactory("SQL Connection", x.Inject<ILogger>()))
                                     .InSingletonScope();
            ninjectKernel.Bind<QueryExecutor>().ToSelf();
            ninjectKernel.Bind<CommandExecutor>().ToSelf();
            ninjectKernel.Bind<UserService>().ToSelf();
            ninjectKernel.Bind<CreateUserHandler>().ToSelf();
            var createUserHandler = ninjectKernel.Get<CreateUserHandler>();
            createUserHandler.Handle();
            ninjectKernel.Bind<RestClient>();
            createUserHandler.Handle();

            MyDependencyResolver resolver = new MyDependencyResolver();            
            resolver.BindToType(typeof(ILogger), typeof(ConsoleLogger));            
            resolver.BindToType(typeof(QueryExecutor), typeof(QueryExecutor));
            resolver.BindToType(typeof(CommandExecutor), typeof(CommandExecutor));
            resolver.BindToType(typeof(CacheService), typeof(CacheService));
            resolver.BindToType(typeof(UserService), typeof(UserService));
            resolver.BindToObj(typeof(IConnectionFactory), new SqlConnectionFactory("SQL Connection", resolver.Get<ILogger>()));
            resolver.BindToObj(typeof(RestClient), new RestClient("apiKey"));
            resolver.BindToType(typeof(CreateUserHandler), typeof(CreateUserHandler));
            var createUserHandler = resolver.Get<CreateUserHandler>();           
            createUserHandler.Handle();
        }
    }
}


