using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Repositories.Repository
{
    public class CommonRepository
    {
        protected NpgsqlConnection conn;
        public CommonRepository(IConfiguration myConfiguration)
        {
            conn = new NpgsqlConnection(myConfiguration.GetConnectionString("MyConnection"));
        }
    }
}