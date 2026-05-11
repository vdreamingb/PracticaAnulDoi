using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Practica
{
    public class DataBase
    {
        private readonly string connectionString =
            "Data Source=Home-PC\\SQLEXPRESS;Initial Catalog=Beneficiari;Integrated Security=True;TrustServerCertificate=True";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public DataTable SelectData(string tableName)
        {
            using SqlConnection conn = GetConnection();
            string command = "SELECT * FROM " + tableName;
            using SqlCommand cmd = new SqlCommand(
                command, conn);

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);
            return table;
        }
    }
}