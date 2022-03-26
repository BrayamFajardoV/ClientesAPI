using AutoMapper;
using ClientesAPI.Data;
using ClientesAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace ClientesAPI
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
            /*Inyeccion de dependecias */
            services.AddDbContext<ApplicationDbContext>
                (Options =>
                {
                    Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });
                
            IMapper mapper = MappingConfiguration.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            

            services.AddScoped<IClienteRepository, ClienteRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.ASCII.GetBytes(
                                Configuration.GetSection("AppSetings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                           
                    };

                });

            services.AddSwaggerGen(c=> 
            {

                c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Autorizacion standar , usar bearer, Ejmplo: \"bearer{token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                }); ;
            });

            services.AddCors();

            /*----------------------------------------------*/

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClientesAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClientesAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(x => x.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
