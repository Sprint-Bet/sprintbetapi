using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SprintBetApi.Auth.Requirements;
using SprintBetApi.Constants;
using SprintBetApi.Hubs;
using SprintBetApi.Services;
using System.Security.Claims;

namespace SprintBet
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("https://sprintbet.herokuapp.com",
                            "https://sprintbet-staging.herokuapp.com",
                            "https://localhost:8888",
                            "http://localhost:8888")
                        .AllowCredentials();
                });
            });

            services.AddSignalR();

            services.AddSingleton<IVoterService, VoterService>();
            services.AddSingleton<IRoomService, RoomService>();
            services.AddSingleton<IAuthService, AuthService>();

            services.AddAuthorization(options => {
                options.AddPolicy(Constants.VoterHasIdClaimTypePolicy, policy => policy.RequireClaim(ClaimTypes.NameIdentifier));
                options.AddPolicy(Constants.VoterIdMatchesRequestPolicy, policy => policy.Requirements.Add(new VoterIdMatchesRequestRequirement()));
            });

            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapHub<VoteHub>("/voteHub"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}
