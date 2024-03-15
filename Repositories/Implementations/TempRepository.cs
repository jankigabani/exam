using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Repositories.Repository;
using Repositories.Models;
using Npgsql;

namespace Repositories.Repository
{
    public class TempRepository: CommonRepository, ITempRepository
    {
        public TempRepository(IConfiguration myConfiguration):base(myConfiguration)
        {

        }

        public tblExecutiveLogin Login(tblExecutiveLogin login)
        {
            tblExecutiveLogin loggedInUser = null;
            try
            {
                conn.Open();
                var cmd = new NpgsqlCommand("SELECT c_userid, c_password, token_type FROM t_token_login_114 WHERE c_userid=@c_userid AND c_password=@c_password", conn);
                cmd.Parameters.AddWithValue("@c_userid", login.c_userid);
                cmd.Parameters.AddWithValue("@c_password", login.c_password);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                        loggedInUser = new tblExecutiveLogin
                        {
                            c_userid = reader.GetString(0),
                            c_password = reader.GetString(1),
                            token_type = reader.GetString(2)
                        };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                conn.Close();
            }
            return loggedInUser;
        }


        public List<tblCustomerToken> GetAll(string id)
        {
            List<tblCustomerToken> customers = new List<tblCustomerToken>();
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT token_id,token_type, token_status, customer_name, customer_phone FROM t_customer_token_114 WHERE token_type = @type AND token_status = 'Pending' ", conn))
                {
                    cmd.Parameters.AddWithValue("@type", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tblCustomerToken customer = new tblCustomerToken();
                            customer.token_id = reader.GetString(0);
                            customer.token_type = reader.GetString(1);
                            customer.token_status = reader.GetString(2);
                            customers.Add(customer);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
            return customers;
        }

    }
}