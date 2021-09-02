using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectA.Handlers;

using Telegram.Bot;

namespace ProjectA
{
    public static class TelegramBotExtensions
    {
        
        public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var client = new TelegramBotClient(configuration.GetSection("TelegramBotToken").Value);
          

            serviceCollection.AddScoped<ITelegramBotClient>(x => client);
           
            return serviceCollection;
        }
    }
}
