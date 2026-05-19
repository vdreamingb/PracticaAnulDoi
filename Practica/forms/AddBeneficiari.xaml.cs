using Microsoft.Data.SqlClient;
using Practica.Classes;
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

namespace Practica
{
    /// <summary>
    /// Interaction logic for AddBeneficiari.xaml
    /// </summary>
    public partial class AddBeneficiari : Page
    {
        public Beneficiar benefeiciar = new Beneficiar();
        Window form;
        public AddBeneficiari(Window form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int nrBen = Convert.ToInt32(nrBenTextBox.Text == ""?0: nrBenTextBox.Text);
            string surname = surnameTextBox.Text;
            string name = nameTextBox.Text;
            string addres = addresBox.Text;
            string phoneNumber = phoneNumberTextBox.Text;
            string email = emailTextBox.Text;
            int codLoc = Convert.ToInt32(codLocTextBox.Text == ""?0: codLocTextBox.Text);
            benefeiciar = new Beneficiar(nrBen, surname, name, phoneNumber, email, addres, codLoc);
            if (benefeiciar.IsEmpty())
            {
                System.Windows.MessageBox.Show("Unele informatii nu sunt complete", "Introduceti datele");
            }
            else
            {
                DataBase db = new DataBase();
                using (var conn = db.GetConnection())
                {
                    try
                    {
                        conn.Open();

                        using var cmd = new SqlCommand("sp_Adauga", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NrBen", nrBen);
                        cmd.Parameters.AddWithValue("@Nume", surname);
                        cmd.Parameters.AddWithValue("@Prenume", name);
                        cmd.Parameters.AddWithValue("@Adresa", addres);
                        cmd.Parameters.AddWithValue("@Telefon", phoneNumber);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@CodLoc", codLoc);

                        cmd.ExecuteNonQuery();

                        System.Windows.MessageBox.Show("Beneficiar adăugat cu succes!");
                        form.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Database Error: " + ex.Message);
                    }
                }
            }
        }
    }
}
