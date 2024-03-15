using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Repositories.Models;
using Repositories.Repository;

namespace Repositories.Repository
{
    public class UserRepository : CommonRepository, IUserRepository
    {

        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public tblUser LoginDatabase(tblUser user)
        {
            tblUser isUserAuthenticated = null;
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM t_user_028 WHERE c_email = @email AND c_password = @password;", conn))
                {
                    cmd.Parameters.AddWithValue("email", user.c_email);
                    cmd.Parameters.AddWithValue("password", user.c_password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isUserAuthenticated = new tblUser();
                            isUserAuthenticated.c_userid = Convert.ToInt32(reader["c_userid"]);
                            isUserAuthenticated.c_email = reader["c_email"].ToString();
                            isUserAuthenticated.c_username = reader["c_username"].ToString();
                            isUserAuthenticated.c_password = reader["c_password"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return isUserAuthenticated;
        }

        public List<tblAdmin> GetAll(string id)
        {
            List<tblAdmin> admins = new List<tblAdmin>();
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT c_ticketid, c_trip, c_price, c_ticketstock, c_currentstock FROM t_tripadmin_114", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tblAdmin admin = new tblAdmin();
                            admin.c_ticketid = Convert.ToInt32(reader["c_ticketid"]);
                            admin.c_trip = reader["c_trip"].ToString();
                            admin.c_price = Convert.ToInt32(reader["c_price"]);
                            admin.c_ticketstock = Convert.ToInt32(reader["c_ticketstock"]);
                            admin.c_currentstock = Convert.ToInt32(reader["c_currentstock"]);
                            admins.Add(admin);
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
            return admins;
        }


        public void InsertTrip(tblAdmin admin)
        {
            try
            {
                conn.Open();

                using (var command = new NpgsqlCommand("INSERT INTO t_tripadmin_114 (c_trip, c_price, c_ticketstock, c_currentstock) VALUES (@name, @price, @ticketstock, @currentstock);", conn))
                {
                    command.Parameters.AddWithValue("@name", admin.c_trip);
                    command.Parameters.AddWithValue("@price", admin.c_price);
                    command.Parameters.AddWithValue("@ticketstock", admin.c_ticketstock);
                    command.Parameters.AddWithValue("@currentstock", admin.c_currentstock);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting trip: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public tblAdmin FetchTripDetails(int c_ticketid)
        {
            var admin = new tblAdmin();
            try
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT c_ticketid, c_trip, c_price, c_ticketstock, c_currentstock FROM t_tripadmin_114 WHERE c_ticketid = @c_ticketid;", conn))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@c_ticketid", c_ticketid);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            admin.c_ticketid = Convert.ToInt32(reader["c_ticketid"]);
                            admin.c_trip = reader["c_trip"].ToString();
                            admin.c_price = Convert.ToInt32(reader["c_price"]);
                            admin.c_ticketstock = Convert.ToInt32(reader["c_ticketstock"]);
                            admin.c_currentstock = Convert.ToInt32(reader["c_currentstock"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching trip details: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return admin;
        }

        public void DeleteTripDetails(int c_ticketid)
        {
            conn.Open();
            using (var command = new NpgsqlCommand("DELETE FROM t_tripadmin_114 WHERE c_ticketid = @c_ticketid;", conn))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@c_ticketid", c_ticketid);
                command.ExecuteNonQuery();
            }
            conn.Close();
        }

        public void UpdateExistingTrip(tblAdmin admin)
        {
            try
            {
                using var command = new NpgsqlCommand("UPDATE t_tripadmin_114 SET c_trip = @trip, c_price = @price, c_ticketstock = @c_ticketstock, c_currentstock = @c_currentstock WHERE c_ticketid = @c_ticketid;", conn);
                command.Parameters.AddWithValue("@c_ticketid", admin.c_ticketid);
                command.Parameters.AddWithValue("@trip", admin.c_trip);
                command.Parameters.AddWithValue("@price", admin.c_price);
                command.Parameters.AddWithValue("@c_ticketstock", admin.c_ticketstock);
                command.Parameters.AddWithValue("@c_currentstock", admin.c_currentstock);

                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating trip: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void InsertCustomerTrip(tblCustomer customer)
        {
            try
            {
                conn.Open();

                DateTime today = DateTime.Today;
                string bookingStatus = (customer.c_tripdate.Date < today) ? "Departed" :
                                        (customer.c_tripdate.Date == today) ? "Scheduled" : "Scheduled";

                using (var command = new NpgsqlCommand("INSERT INTO t_tripcustomer_028 (c_ticketid, c_userid, c_tripdate, c_trip, c_price, c_ticketqnt, c_totalcost, c_bookingstatus) VALUES (@ticketid, @userid, @date, @trip, @price, @qnt, @cost, @status)", conn))
                {
                    command.Parameters.AddWithValue("@ticketid", customer.c_ticketid);
                    command.Parameters.AddWithValue("@userid", customer.c_userid);
                    command.Parameters.AddWithValue("@date", customer.c_tripdate);
                    command.Parameters.AddWithValue("@trip", customer.c_trip);
                    command.Parameters.AddWithValue("@price", customer.c_price);
                    command.Parameters.AddWithValue("@qnt", customer.c_ticketqnt);
                    command.Parameters.AddWithValue("@cost", customer.c_totalcost);
                    command.Parameters.AddWithValue("@status", bookingStatus);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting trip: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        public List<tblCustomer> GetAllBookings(int id)
        {
            List<tblCustomer> customers = new List<tblCustomer>();
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT c_customerid, c_ticketid, c_userid, c_tripdate, c_trip, c_price, c_ticketqnt, c_totalcost, c_bookingstatus FROM t_tripcustomer_028 WHERE c_userid=4 ", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tblCustomer customer = new tblCustomer();
                            customer.c_customerid = Convert.ToInt32(reader["c_customerid"]);
                            customer.c_ticketid = Convert.ToInt32(reader["c_ticketid"]);
                            customer.c_userid = Convert.ToInt32(reader["c_userid"]);
                            customer.c_tripdate = Convert.ToDateTime(reader["c_tripdate"]);
                            customer.c_trip = reader["c_trip"].ToString();
                            customer.c_price = Convert.ToInt32(reader["c_price"]);
                            customer.c_ticketqnt = Convert.ToInt32(reader["c_ticketqnt"]);
                            customer.c_totalcost = Convert.ToInt32(reader["c_totalcost"]);
                            customer.c_bookingstatus = reader["c_bookingstatus"].ToString();
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

        public void UpdateBookingStatus(int customerId, string status)
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("UPDATE t_tripcustomer_028 SET c_bookingstatus = @status WHERE c_customerid = @customerId", conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }


    }
}