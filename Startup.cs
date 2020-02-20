using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Eduraise.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Eduraise
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
			/*services.AddDbContext<EduraiseContext>(opt =>
				opt.UseInMemoryDatabase("Eduraise"));
			services.AddControllers();*/

			services.AddDbContext<EduraiseContext>(options
				=> options.UseSqlServer(Configuration.GetConnectionString("AppDBConnection")));

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = AuthOptions.ISSUER,
						ValidateAudience = true,
						ValidAudience = AuthOptions.AUDIENCE,
						ValidateLifetime = true,
						IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
						ValidateIssuerSigningKey = true,
					};
				});
			services.AddControllersWithViews();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Eduraise Api", Version = "v1" });
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
			services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			//app.UseSwagger();
			//app.UseSwaggerUI(c =>
			//{
			//	c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			//});
			app.UseHttpsRedirection();
			app.UseCors(builder => builder.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
