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
using ProjectA.Services.Statistics;
using ProjectA.Services.Handlers;
using ProjectA.Services.StateProvider;
using System.Threading.Tasks;
using ProjectA.Factory;

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

            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<IPlayersRepository, PlayersRepository>();
            services.AddTransient<IPlayerSuggestionService, PlayerSuggestionService>();
            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<IHandlerTeamService, HandlerTeamService>();
            services.AddTransient<IStateFactory, StateFactory>();
            
            services.AddSingleton<ITelegramUpdateHandler, TelegramHandler>();
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(Configuration["TelegramBotToken"]));
          
            services.AddControllers();
            services.AddRefitClient<IFantasyPremierLeagueClient>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration.GetSection("FantasyPremierLeagueUrl").Value));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectA", Version = "v1" });
            });

            services.AddSingleton<ICosmosDbStateProviderService>(InitializeCosmosClientInstanceAsync(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());
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
  
        }

        private static async Task<CosmosDbStateProviderService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName"];
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];

            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/chat_id");
            var cosmosDbService = new CosmosDbStateProviderService(client, databaseName, containerName);
            return cosmosDbService;
        }
    }
}
