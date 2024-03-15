using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class tblUser
    {
        public int c_userid { get; set; }
        public string c_username { get; set; }
        public string c_email { get; set; }
        public string c_password { get; set; }
    }
}