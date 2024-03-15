using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class tblAdmin
    {
        public int c_ticketid{get;set;}
        public string? c_trip{get;set;}
        public int c_price{get;set;}
        public int c_ticketstock{get;set;}
        public int c_currentstock{get;set;}
        public int c_ticketqnt{get;set;}
    }
}