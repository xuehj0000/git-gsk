using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GSK.Auth.Repository
{
    public static class DapperExtensions
    {
        public static IDapperFactoryBuilder AddDapper(this IServiceCollection services, string name, Action<ConnOptions> configureClient)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (configureClient == null)
                throw new ArgumentNullException(nameof(configureClient));

            services.AddOptions();
            services.AddSingleton<DbFactory>();
            services.TryAddSingleton<IDbFactory>(serviceProvider => serviceProvider.GetRequiredService<DbFactory>());

            var builder = new DefaultDapperFactoryBuilder(services, name);
            builder.Services.Configure<DapperOptions>(builder.Name, options => options.DapperActions.Add(configureClient));
            return builder;
        }
    }


    #region 所有时间优化一下


    public interface IDapperFactoryBuilder
    {
        string Name { get; }

        IServiceCollection Services { get; }
    }

    internal class DefaultDapperFactoryBuilder : IDapperFactoryBuilder
    {
        public DefaultDapperFactoryBuilder(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public string Name { get; }

        public IServiceCollection Services { get; }
    }

    public class ConnOptions
    {
        public string ConnString { get; set; }
        public DbType DbType { get; set; } = DbType.SqlServer;
    }

    public enum DbType
    {
        SqlServer = 0
    }

    public class DapperOptions
    {
        public IList<Action<ConnOptions>> DapperActions { get; } = new List<Action<ConnOptions>>();
    }
    #endregion
}
