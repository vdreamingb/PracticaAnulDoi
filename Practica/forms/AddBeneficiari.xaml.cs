using Microsoft.Data.SqlClient;
using Practica.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions; // Added for Email validation
using System.Windows;
using System.Windows.Controls;

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
            if (!int.TryParse(nrBenTextBox.Text, out int nrBen) || nrBen <= 0)
            {
                MessageBox.Show("Numărul beneficiarului trebuie să fie un număr valid mai mare decât 0.", "Eroare Validare");
                return;
            }

            if (!int.TryParse(codLocTextBox.Text, out int codLoc) || codLoc <= 0)
            {
                MessageBox.Show("Codul localității trebuie să fie un număr valid mai mare decât 0.", "Eroare Validare");
                return;
            }

            string surname = surnameTextBox.Text.Trim();
            string name = nameTextBox.Text.Trim();
            string addres = addresBox.Text.Trim();
            string phoneNumber = phoneNumberTextBox.Text.Trim();
            string email = emailTextBox.Text.Trim();

            if (string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Numele și Prenumele sunt câmpuri obligatorii.", "Eroare Validare");
                return;
            }

            if (string.IsNullOrEmpty(addres))
            {
                MessageBox.Show("Adresa este obligatorie.", "Eroare Validare");
                return;
            }

            if (string.IsNullOrEmpty(phoneNumber) || !Regex.IsMatch(phoneNumber, @"^[0-9+\s-]+$"))
            {
                MessageBox.Show("Introduceți un număr de telefon valid (doar cifre, spații sau +).", "Eroare Validare");
                return;
            }

            if (!string.IsNullOrEmpty(email))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, emailPattern))
                {
                    MessageBox.Show("Formatul adresei de email este invalid.", "Eroare Validare");
                    return;
                }
            }

            benefeiciar = new Beneficiar(nrBen, surname, name, phoneNumber, email, addres, codLoc);

            if (benefeiciar.IsEmpty())
            {
                MessageBox.Show("Unele informații nu sunt complete în obiectul Beneficiar.", "Introduceți datele");
                return;
            }

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
                    cmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodLoc", codLoc);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Beneficiar adăugat cu succes!", "Succes");
                    form.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Eroare Bază de Date");
                }
            }
        }
    }
}