using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBasket.Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyBasket.Infrastructure.Extensions
{
    public static class WebExtensions
    {
        #region Methods

        public static IServiceCollection StartInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterDependencies(services, configuration);
            RegisterAutoMapper();
            RegisterCommands(services);
            RegisterValidators(services);

            return services;
        }

        #endregion Methods

        #region Utilities

        private static void RegisterDependencies(IServiceCollection services, IConfiguration configuration)
        {
            List<Type> dependencies;
            try
            {
                dependencies = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => typeof(IDependency).IsAssignableFrom(p) && !p.IsInterface).ToList();
            }
            catch (ReflectionTypeLoadException ex)
            {
                var exceptionMessageBuilder = new StringBuilder();
                foreach (var e in ex.LoaderExceptions)
                {
                    exceptionMessageBuilder.AppendLine(e.Message);
                }

                throw new Exception(exceptionMessageBuilder.ToString(), ex);
            }

            if (dependencies.Count.Equals(default(int))) return;
            {
                var instances = dependencies
                    .Select(item => (IDependency)Activator.CreateInstance(item))
                    .OrderBy(item => item.Order);

                foreach (var dependencyRegistrar in instances)
                {
                    dependencyRegistrar.Register(services, configuration);
                }
            }
        }

        private static void RegisterAutoMapper()
        {
            List<Type> mapperConfigurations;

            try
            {
                mapperConfigurations = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => typeof(AutoMapperProfile).IsAssignableFrom(p) && !p.IsAbstract).ToList();
            }
            catch (ReflectionTypeLoadException ex)
            {
                var exceptionMessageBuilder = new StringBuilder();
                foreach (var e in ex.LoaderExceptions)
                {
                    exceptionMessageBuilder.AppendLine(e.Message);
                }

                throw new Exception(exceptionMessageBuilder.ToString(), ex);
            }

            if (mapperConfigurations.Count.Equals(default(int))) return;
            {
                var instances = mapperConfigurations
                    .Select(startup => (AutoMapperProfile)Activator.CreateInstance(startup))
                    .OrderBy(startup => startup.Order);

                var config = new MapperConfiguration(cfg =>
                {
                    foreach (var instance in instances)
                    {
                        cfg.AddProfile(instance.GetType());
                    }
                });

                AutoMapperConfiguration.Init(config);
            }
        }

        private static void RegisterCommands(IServiceCollection services)
        {
            List<Type> commands;
            try
            {
                commands = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => p.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>) && !p.IsInterface))
                    .ToList();
            }
            catch (ReflectionTypeLoadException ex)
            {
                var exceptionMessageBuilder = new StringBuilder();
                foreach (var e in ex.LoaderExceptions)
                {
                    exceptionMessageBuilder.AppendLine(e.Message);
                }

                throw new Exception(exceptionMessageBuilder.ToString(), ex);
            }

            if (commands.Count.Equals(default(int))) return;
            {
                services.AddMediatR(commands.ToArray());
            }
        }

        private static void RegisterValidators(IServiceCollection services)
        {
            List<Type> validators;
            try
            {
                validators = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(t => t.BaseType != null && t.BaseType.IsGenericType && !t.IsGenericType &&
                                t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>)).ToList();
            }
            catch (ReflectionTypeLoadException ex)
            {
                var exceptionMessageBuilder = new StringBuilder();
                foreach (var e in ex.LoaderExceptions)
                {
                    exceptionMessageBuilder.AppendLine(e.Message);
                }

                throw new Exception(exceptionMessageBuilder.ToString(), ex);
            }

            if (validators.Count.Equals(default(int))) return;
            {
                foreach (var validator in validators.Where(validator => validator.BaseType != null))
                {
                    services.AddTransient(typeof(IValidator<>).MakeGenericType(validator.BaseType.GetGenericArguments().First()), validator);
                }
            }
        }

        #endregion Utilities
    }
}