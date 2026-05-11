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
    /// Interaction logic for Localitati.xaml
    /// </summary>
    public partial class Localitati : Page
    {
        public Localitati()
        {
            InitializeComponent();
            populateTable();
        }

        private void populateTable()
        {
            DataBase db = new DataBase();
            DataTable localitati = db.SelectData("Localitati");
            localitatiTable.ItemsSource = localitati.DefaultView;
        }
    }
}
