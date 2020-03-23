using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyBasket.Api.Infrastructure.Filters;
using MyBasket.Infrastructure.Extensions;
using MyBasket.Infrastructure.Redis;
using Swashbuckle.AspNetCore.Swagger;

namespace MyBasket.Api
{
    public class Startup
    {
        #region Ctor

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion Ctor

        #region Properties

        public IConfiguration Configuration { get; }

        #endregion Properties

        #region Methods

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.Filters.Add(typeof(GlobalExceptionFilter)); })
                .AddFluentValidation(fv =>
                {
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    fv.ImplicitlyValidateChildProperties = true;
                });

            services.Configure<RedisSettings>(options => Configuration.GetSection("RedisSettings").Bind(options));

            services.StartInfrastructure(Configuration);
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "My Basket HTTP API",
                    Version = "v1",
                    Description = "The My Basket Service HTTP API",
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Basket Api v1"); });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        #endregion Methods
    }
}