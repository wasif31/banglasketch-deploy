using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Model;
using Repositories;
using AutoMapper;
using MySQL.Data.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Utilities;
using Services;
using Services.AuthService;
using Services.FileService;
using WebService.Helpers;
using DataContext = Repositories.DataContext;
using Services.SearchingService;
using Services.TranslateService;

namespace WebService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // *** To Migration Go to Repositories.DataContext and follow instructions ***
            services.AddDbContext<DataContext>(options =>
                options.UseMySQL(
                    Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("Migrations")));

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //MySql
            //services.AddTransient<MySqlDatabase>(_ => new MySqlDatabase("server=localhost; port=3306; database=banglaSketch; uid=root; pwd=password;"));
            services.AddAutoMapper(typeof(Startup));
            //services.AddDbContext<DataContext>(x => x.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            
            //MySql
            //services.AddTransient<MySqlDatabase>(_ => new MySqlDatabase("server=localhost; port=3306; database=banglasketch; uid=root; pwd=admin;"));
            
            services.AddCors(options =>
            {
                options.AddPolicy(
                   name: "AllowOrigin",
                   builder => {
                       builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
                   });
            });

            services.AddScoped<IAuthService,AuthService>();
            services.AddScoped<IFileService,FileService>();
            services.AddScoped<ISearchingService, SearchingService>();
            services.AddScoped<ITranslateService, TranslateService>();
            services.AddTransient<IMailService, MailService>();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddHttpClient();

           // services.AddScoped<IAuthRepository,AuthRepository>();
            services.AddScoped<IBanglaSketchRepository,BanglaSketchRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenSettings:TokenSecretKey").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
                app.UseCors("AllowOrigin");
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowOrigin");

            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
