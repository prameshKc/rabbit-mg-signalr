using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace angularDotnet.DAL
{
    public class UserRepository : IUser
    {
        private readonly IConfiguration configuration;

        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void EditUser(UserViewModel model)
        {
            using (var conn = new SqlConnection(configuration.GetConnectionString("dbCon")))
            {
                try
                {
                    conn.Open();
                    conn.Execute("sp_user_update", model, commandType: System.Data.CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                    throw;
                }


            }
        }

        public List<UserViewModel> GetUsers()
        {
            using (var conn = new SqlConnection(configuration.GetConnectionString("dbCon")))
            {
                conn.Open();
                var data = conn.Query<UserViewModel>("select * from tblUsers", commandType: System.Data.CommandType.Text).ToList();
                return data;
               
            }
        }

        public void PostUser(UserViewModel model)
        {
            using (var conn = new SqlConnection(configuration.GetConnectionString("dbCon")))
            {
                try
                {
                    conn.Open();
                    conn.Execute("sp_inser_user",  model, commandType: System.Data.CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                    throw;
                }
               

            }
        }
    }
}
