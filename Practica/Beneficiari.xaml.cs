using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Practica
{
    /// <summary>
    /// Interaction logic for Beneficiari.xaml
    /// </summary>
    public partial class Beneficiari : Page
    {
        public Beneficiari()
        {
            InitializeComponent();
            firstRect.UseLayoutRounding = true;
            populateTable();
            setStatistics();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void populateTable()
        {
            DataBase db = new DataBase();
            DataTable table = db.SelectData("Beneficiari");
            beneficiariTable.ItemsSource = table.DefaultView;
        }
        private void setStatistics()
        {
            DataBase db = new DataBase();
            using var conn = db.GetConnection();
            conn.Open();
            var cmd = new SqlCommand("sp_StatisticiDashboard", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                totalBeneficiari.Content = reader["Total"].ToString();
                totalUrban.Content = reader["NrUrban"].ToString();
                totalRural.Content = reader["NrRural"].ToString();
                urbanPercent.Content = reader["ProcUrban"].ToString() + "%"+" din total";
                ruralPercent.Content = reader["ProcRural"].ToString() + "%" + " din total";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add win = new Add("Beneficiari");
            win.ShowDialog();
            if(win == null)
            {
                populateTable();
            }
        }
    }
}
