using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProjectA.Clients;
using ProjectA.Repositories.Teams;
using ProjectA.Services.Teams;
using ProjectA.Repositories;
using ProjectA.Repositories.PlayersRepository;
using ProjectA.Services.PlayersSuggestion;
using Refit;
using System;
using SimpleInjector;
using Telegram.Bot;
using ProjectA.Handlers;
using System.Threading;
using Telegram.Bot.Extensions.Polling;
using ProjectA.Services.Statistics;
using ProjectA.Services.Handlers;

namespace ProjectA
{
    public class Startup
    {
        private readonly Container _container = new Container();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<ITeamRepository, TeamRepository>();
            services.AddSingleton<ITeamService, TeamService>();
            services.AddSingleton<IPlayersRepository, PlayersRepository>();
            services.AddSingleton<IPlayerSuggestionService, PlayerSuggestionService>();
            services.AddSingleton<IStatisticsService, StatisticsService>();
            services.AddSingleton<IHandlerTeamService, HandlerTeamService>();
            services.AddSingleton<Handler, Handler>();

            services.AddControllers();
            services.AddRefitClient<IFantasyPremierLeagueClient>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration.GetSection("FantasyPremierLeagueUrl").Value));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectA", Version = "v1" });
            });

            services.AddSimpleInjector(_container);

            InitializeContainer();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectA v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            _container.Verify();

            InitializeTelegramListener();
        }
        private void InitializeContainer()
        {

            _container.RegisterInstance(CreateClient(Configurations.BotToken));
        }
        private ITelegramBotClient CreateClient(string apiKey) => new TelegramBotClient(apiKey);

        private void InitializeTelegramListener()
        {
            var handler = _container.GetInstance<Handler>();

            var source = new CancellationTokenSource();

            _container.GetInstance<ITelegramBotClient>().StartReceiving(new DefaultUpdateHandler(handler.HandleUpdateAsync, handler.HandleErrorAsync), source.Token);
        }
    }
}
