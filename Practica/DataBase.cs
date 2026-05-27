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
        public DataTable GetBeneficiariByLocalitate(int codLoc)
        {
            using SqlConnection conn = GetConnection();
            string query = "SELECT CodBen, NrBen, Nume, Prenume, Adresa, Telefon, Email FROM Beneficiari WHERE CodLoc = @CodLoc";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CodLoc", codLoc);
            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public DataTable GetBeneficiariByTip(string tip)
        {
            using SqlConnection conn = GetConnection();
            string query = @"
        SELECT b.CodBen, b.NrBen, b.Nume, b.Prenume, b.Adresa, b.Telefon, b.Email
        FROM Beneficiari b
        INNER JOIN Localitati l ON b.CodLoc = l.CodLoc
        WHERE l.Tip = @Tip";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Tip", tip);
            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
    }
}