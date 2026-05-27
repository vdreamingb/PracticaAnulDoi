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
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace Practica
{
    public partial class LogInWindows : Window
    {
        private readonly string serverName = @"Home-PC\SQLEXPRESS";
        private readonly string databaseName = @"Beneficiari";
        private int failedAttempts = 0;
        private const int MaxAttempts = 3;
        public bool isUser = false;

        public LogInWindows()
        {
            InitializeComponent();
        }

        private bool TryAuthenticateWithSqlServer(string username, string password)
        {
            string connectionString = $"Server={serverName};Database={databaseName};User Id={username};Password={password};TrustServerCertificate=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }

        private void logIn()
        {
            string introducedUsername = usernameTextBox.Text;
            string introducedPassword = passwordTextBox.Password;

            if (string.IsNullOrWhiteSpace(introducedUsername) || string.IsNullOrWhiteSpace(introducedPassword))
            {
                MessageBox.Show("Va rugam introduceti numele de utilizator si parola.", "Campuri goale", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TryAuthenticateWithSqlServer(introducedUsername, introducedPassword))
            {
                isUser = true;
                this.Close();
            }
            else
            {
                failedAttempts++;

                if (failedAttempts >= MaxAttempts)
                {
                    MessageBox.Show($"Ati depasit numarul maxim de incercari ({MaxAttempts}). Aplicatia se va inchide.", "Acces blocat", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
                else
                {
                    MessageBox.Show($"Parola sau nume de utilizator incorect. Incercari ramase: {MaxAttempts - failedAttempts}", "Eroare autentificare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logIn();
        }
    }
}