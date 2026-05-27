using Microsoft.Data.SqlClient;
using Practica.Classes;
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
            if(!win.IsActive)
            {
                populateTable();
                setStatistics();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(beneficiariTable.SelectedItem != null)
            {
                EditDataService delService = new EditDataService("Beneficiari", beneficiariTable);
                delService.deleteData();
                System.Windows.MessageBox.Show("Datele au fost sterse cu success", "Sters cu success");
                populateTable();
                setStatistics();
            }
            else
            {
                System.Windows.MessageBox.Show("Nu puteti sa stergeti nimic atat timp cat nu ati selectat datele", "Nu ati selectat nici un rand");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(beneficiariTable.SelectedItem != null)
            {
                EditDataService service = new EditDataService("Beneficiari", beneficiariTable);
                service.updateData();
                System.Windows.MessageBox.Show("Datele au fost modificate cu success", "Modificat cu success");
                populateTable();
                setStatistics();
            }
            else
            {
                System.Windows.MessageBox.Show("Nu puteti sa modificati nimic atat timp cat nu ati setat sau modificat datele", "Nu ati selectat nici un rand");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ExportData exportData = new ExportData("Beneficiari", beneficiariTable);
            exportData.ShowDialog();
        }

        private void DateRural_Click(object sender, RoutedEventArgs e)
        {
            DateSelectateAleBeneficiarilor win = new DateSelectateAleBeneficiarilor("Rural");
            win.ShowDialog();
        }

        private void DateUrban_Click(object sender, RoutedEventArgs e)
        {
            DateSelectateAleBeneficiarilor win = new DateSelectateAleBeneficiarilor("Urban");
            win.ShowDialog();
        }
    }
}
