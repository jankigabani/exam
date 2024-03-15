using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Repositories.Models;

namespace Repositories.Repository
{
    public interface ITempRepository
    {
        public tblExecutiveLogin Login(tblExecutiveLogin login);

        public List<tblCustomerToken> GetAll(string id);
    }
}