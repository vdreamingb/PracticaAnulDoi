using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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

namespace Practica.forms
{
    /// <summary>
    /// Interaction logic for AddLocalitati.xaml
    /// </summary>
    public partial class AddLocalitati : Page
    {
        Window win;
        public AddLocalitati(Window win)
        {
            InitializeComponent();
            this.win = win;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nume = txtNume.Text.Trim();
                string tip = (cmbTip.SelectedItem as ComboBoxItem)?.Content.ToString();
                string judet = txtJudet.Text.Trim();

                if (string.IsNullOrEmpty(nume) || string.IsNullOrEmpty(tip) || string.IsNullOrEmpty(judet))
                {
                    System.Windows.MessageBox.Show("Toate câmpurile sunt obligatorii.", "Eroare Validare");
                    return;
                }

                DataBase db = new DataBase();
                using var con = db.GetConnection();
                con.Open();

                var cmd = new SqlCommand("INSERT INTO Localitati (NumeLoc, Tip, Judet) VALUES (@nume, @tip, @judet)", con);
                cmd.Parameters.AddWithValue("@nume", nume);
                cmd.Parameters.AddWithValue("@tip", tip);
                cmd.Parameters.AddWithValue("@judet", judet);
                cmd.ExecuteNonQuery();

                System.Windows.MessageBox.Show("Localitatea a fost adăugată cu succes!", "Succes");
                win.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("A apărut o eroare: " + ex.Message, "Eroare Bază de Date");
            }
        }
    }
}