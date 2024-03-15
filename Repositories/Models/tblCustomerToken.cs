using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class tblCustomerToken
    {
        public string? token_id{get;set;}
        public string? token_type{get;set;}
        public string? token_status{get;set;}
        public string? customer_name{get;set;}
        public string? customer_phone{get;set;}
    }
}