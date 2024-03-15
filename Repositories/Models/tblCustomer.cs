using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class tblCustomer
    {
        public int c_ticketid{get;set;}
        public int c_customerid{get;set;}
        public string? c_trip{get;set;}
        public int c_price{get;set;}
        public int c_ticketstock{get;set;}
        public int c_currentstock{get;set;}
        public DateTime c_tripdate{get;set;}
        public int c_ticketqnt{get;set;}
        public int c_userid{get;set;}   

        public string c_bookingstatus{get;set;}
        public int c_totalcost{get;set;}

    }
}