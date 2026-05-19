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

namespace Practica
{
    /// <summary>
    /// Interaction logic for LogInWindows.xaml
    /// </summary>
    public partial class LogInWindows : Window
    {
        private string username = "admin";
        private string password = "admin";

        public LogInWindows()
        {
            InitializeComponent();
            passwordTextBox.DataContext = "*";
        }


        private void logIn()
        {
            string introducedUsername = usernameTextBox.Text;
            string introducedPassword = passwordTextBox.Password;

            if(introducedPassword == this.password && introducedUsername == this.username)
            {
                this.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Parola sau nume de utilizator incorect");
            }
        }
      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.logIn();
        }
    }
}
