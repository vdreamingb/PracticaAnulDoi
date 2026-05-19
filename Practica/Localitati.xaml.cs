using ClosedXML.Excel;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add win = new Add("Localitati");
            win.ShowDialog();
            if (!win.IsActive)
            {
                populateTable();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (localitatiTable.SelectedItem != null)
            {
                EditDataService delService = new EditDataService("Localitati", localitatiTable);
                delService.deleteData();
                System.Windows.MessageBox.Show("Datele au fost sterse cu success", "Sters cu success");
                populateTable();
            }
            else
            {
                System.Windows.MessageBox.Show("Nu puteti sa stergeti nimic atat timp cat nu ati selectat datele", "Nu ati selectat nici un rand");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(localitatiTable.SelectedItem != null)
            {
                EditDataService service = new EditDataService("Localitati", localitatiTable);
                service.updateData();
                System.Windows.MessageBox.Show("Datele au fost modificate cu success", "Modificat cu success");
                populateTable();
            }
            else
            {
                System.Windows.MessageBox.Show("Nu puteti sa modificati nimic atat timp cat nu ati setat sau modificat datele", "Nu ati selectat nici un rand");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ExportData exportData = new ExportData("Localitati",localitatiTable);
            exportData.ShowDialog();
        }
    }
}
