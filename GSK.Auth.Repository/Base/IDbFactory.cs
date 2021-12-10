using System;
using System.Collections.Generic;
using System.Text;

namespace GSK.Auth.Repository
{
    public interface IDbFactory
    {
        MyDbBase CreateClient(string name);
    }
}
