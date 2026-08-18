using System.Configuration;
using Microsoft.Data.SqlClient; //Bản thay thế cho System.Data.SqlClient, tương thích với .NET 10.0
//using System.Data.SqlClient; -- Phiên bản này đã cũ, không thể sử dụng cho project .NET 10.0 hiện tại

namespace DAL_HTQLCuaHangLaptop
{
    public class DBConnect
    {
        protected SqlConnection _conn;

        public DBConnect()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["QuanLyCuaHangLaptop"].ConnectionString;
            _conn = new SqlConnection(connStr);
        }

        protected void OpenConnection() => _conn.Open();

        protected void CloseConnection()
        {
            if (_conn.State == System.Data.ConnectionState.Open)
                _conn.Close();
        }
    }
}