using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
    /// Interaction logic for RaportContact.xaml
    /// </summary>
    public partial class RaportContact : Page
    {
        public RaportContact()
        {
            InitializeComponent();
            populateTable();
        }

        public void populateTable()
        {
            DataTable table = new DataTable();
            DataBase db = new DataBase();
            using var conn = db.GetConnection();

            conn.Open();

            var cmd = new SqlCommand(@"
                        SELECT 
                            b.Nume + ' ' + b.Prenume AS NumeComplet,
                            b.Email,
                            b.Telefon,
                            b.Adresa,
                            l.NumeLoc
                        FROM Beneficiari b
                        JOIN Localitati l ON b.CodLoc = l.CodLoc
                        ORDER BY b.Nume", conn);

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            

            adapter.Fill(table);

            dgContact.ItemsSource = table.DefaultView;

        }

        private void dgContact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
