using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class tblExecutiveLogin
    {
        public string? c_userid{get;set;}
        public string? c_password{get;set;}
        public string? token_type{get;set;}
    }
}