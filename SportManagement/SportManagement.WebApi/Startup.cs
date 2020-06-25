using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SportManagement.Data.DataAccess;
using SportManagement.Data.Repository;
using SportManagement.WebApi.Services;

namespace SportManagement.WebApi
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
            // add asp net core mvc
            services.AddMvc(o => o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // var connection=Configuration["DBConnection:Connection"];

      
services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
    });
});



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,    
                ValidateLifetime=true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
        });

             services.Configure<FormOptions>(options =>
             {
                 options.ValueCountLimit = 1000000; 
                 options.ValueLengthLimit = 1024 * 1024 * 100; // 100MB max len form data
             });



            services.AddDbContext<DomainContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SportManagementDatabase")).UseLazyLoadingProxies());

            //services.AddDbContext<DomainContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SportManagementDatabase")));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISportService, SportService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ILogInService, LogInService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           
            app.UseCors("EnableCORS");

            app.UseAuthentication();

            app.UseMvc();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "Default",
                    template: "api/{controller}/{id?}"
                );
            });
        }
    }
}
