using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Repository
{
    public interface IUserRepository
    {
        public tblUser LoginDatabase(tblUser user);

        public List<tblAdmin> GetAll(string id);

        public void InsertTrip(tblAdmin admin);

        public tblAdmin FetchTripDetails(int c_ticketid);

        public void DeleteTripDetails(int c_ticketid);

        public void UpdateExistingTrip(tblAdmin admin);

        public void InsertCustomerTrip(tblCustomer customer);

        public List<tblCustomer> GetAllBookings(int id);

        public void UpdateBookingStatus(int customerId, string status);
    }
}