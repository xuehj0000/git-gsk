using System;
using System.Linq;
using Microsoft.Extensions.Options;

namespace GSK.Auth.Repository
{
    public class DbFactory : IDbFactory
    {
        private readonly IOptionsMonitor<DapperOptions> _optionsMonitor;

        public DbFactory(IOptionsMonitor<DapperOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
        }

        public MyDbBase CreateClient(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var client = new MyDbBase(new ConnConfig { });
            var option = _optionsMonitor.Get(name).DapperActions.FirstOrDefault();
            if (option != null)
                option(client.ConnConfig);
            else
                throw new ArgumentNullException(nameof(option));
            return client;
        }
    }
}
