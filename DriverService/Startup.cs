using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using DriverService.Data;
using DriverService.Data.DriverProfiles;
using DriverService.Data.Orders;
using DriverService.Data.Drivers;
using DriverService.Helpers;
using DriverService.KafkaHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DriverService
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      Configuration = configuration;
      _env = env;
    }
    private readonly IWebHostEnvironment _env;

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      if (_env.IsProduction())
      {
        Console.WriteLine("--> using Sql server Db");
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
          Configuration.GetConnectionString("DatabaseConn")
        ));
      }
      else
      {
        Console.WriteLine("--> Using LocalDB");
        services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(Configuration.GetConnectionString("DatabaseConn")));
      }

      services.AddIdentity<IdentityUser, IdentityRole>(options =>
      {
        options.Password.RequiredLength = 8;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireDigit = true;
      }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

      services.AddControllers().AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
       .AddXmlDataContractSerializerFormatters();
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



      var kafkaConfig = Configuration.GetSection("KafkaConfig");
      services.Configure<KafkaConfig>(kafkaConfig);
      var appSettingSection = Configuration.GetSection("AppSettings");
      services.Configure<AppSettings>(appSettingSection);
      var appSettings = appSettingSection.Get<AppSettings>();
      var key = Encoding.ASCII.GetBytes(appSettings.Secret);
      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });
      services.AddTransient<DbInitializer>();
      services.AddTransient<TopicInitHandler>();
      services.AddScoped<IDriver, DriverDAL>();
      services.AddScoped<IDriverProfile, DriverProfileDAL>();
      services.AddScoped<IOrder, OrderDAL>();
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

      // services.AddHostedService<ConsumerHandler>();
      // services.AddSingleton<IHostedService, ConsumerHandler>();
      // services.AddSingleton<IHostedService, KafkaConsumerHandler>();
      // services.AddHostedService<ApacheKafkaConsumerService>();

      services.AddSingleton<IHostedService, ProcessOrdersService>();

      var producerConfig = new ProducerConfig();
      var consumerConfig = new ConsumerConfig();
      Configuration.Bind("producer", producerConfig);
      Configuration.Bind("consumer", consumerConfig);

      services.AddSingleton<ProducerConfig>(producerConfig);
      services.AddSingleton<ConsumerConfig>(consumerConfig);

      services.AddControllers();
      services.AddSwaggerGen(c =>
        {
          c.SwaggerDoc("v1", new OpenApiInfo { Title = "DriverService", Version = "v1" });
          var securitySchema = new OpenApiSecurityScheme
          {

            Description = "JWT Authorization header menggunakan bearer. \nPlease enter into field the word 'Bearer' following by space and JWT",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer"
            }
          };
          c.AddSecurityDefinition("Bearer", securitySchema);
          c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securitySchema, new[]{ "Bearer"} }
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app,
                          IWebHostEnvironment env,
                          AppDbContext context,
                          DbInitializer seeder,
                          UserManager<IdentityUser> userManager,
                          TopicInitHandler topicInitHandler)
    {
      context.Database.EnsureCreated();
      _ = topicInitHandler.TopicInit();
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService v1"));
      }
      _ = seeder.Initialize(userManager);

      app.UseHttpsRedirection();

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
