using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Ban_Do_An_Nhanh.DAL 
{
    public static class DBConnection
    {
        private static string connectionString = @"Data Source=HUYDIEN;Initial Catalog=FastFoodDB;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
