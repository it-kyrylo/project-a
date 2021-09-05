using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectA.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace ProjectA.HostedService
{
    public class TelegramBotHostedService :  IHostedService, IDisposable
    { 
        private readonly IServiceProvider _serviceProvider;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ITelegramUpdateHandler _telegramUpdateHandler;
        

        public TelegramBotHostedService(ITelegramBotClient telegramBotClient,IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _telegramBotClient = telegramBotClient;            
           this._telegramUpdateHandler = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ITelegramUpdateHandler>();
        }
    
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            
            await _telegramBotClient
                .ReceiveAsync(new DefaultUpdateHandler(_telegramUpdateHandler.HandleUpdateAsync, _telegramUpdateHandler.HandleErrorAsync), cancellationToken);
            return;
        }


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _telegramBotClient.CloseAsync(cancellationToken);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

      
    }
}
