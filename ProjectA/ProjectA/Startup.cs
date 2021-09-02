using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProjectA.Clients;
using ProjectA.Repositories.Teams;
using ProjectA.Services.Teams;
using ProjectA.Repositories.PlayersRepository;
using ProjectA.Services.PlayersSuggestion;
using Refit;
using System;
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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IPlayersRepository, PlayersRepository>();
            services.AddScoped<IPlayerSuggestionService, PlayerSuggestionService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IHandlerTeamService, HandlerTeamService>();
            services.AddScoped<Handler, Handler>();
            services.AddTelegramBotClient(Configuration);
            services.AddControllers();
            services.AddRefitClient<IFantasyPremierLeagueClient>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration.GetSection("FantasyPremierLeagueUrl").Value));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectA", Version = "v1" });
            });

            

           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,Handler handler,ITelegramBotClient botClient)
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

            
            var source = new CancellationTokenSource();

            botClient.StartReceiving(new DefaultUpdateHandler(handler.HandleUpdateAsync, handler.HandleErrorAsync), source.Token);
            
        }
        
    }
}
