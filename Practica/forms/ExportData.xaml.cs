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
using System.Windows.Shapes;
using Practica.Classes;

namespace Practica
{
    /// <summary>
    /// Interaction logic for ExportData.xaml
    /// </summary>
    public partial class ExportData : Window
    {
        string tableName;
        System.Windows.Controls.DataGrid grid;

        public ExportData(string tableName)
        {
            InitializeComponent();
            this.tableName = tableName;
        }
        public ExportData(string tableName, System.Windows.Controls.DataGrid grid)
        {
            InitializeComponent();
            this.tableName = tableName;
            this.grid = grid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (tableName == "All")
            {
                EditDataService service= new EditDataService();
                service.ExportFullExcel();
            }
            else
            {
                EditDataService service = new EditDataService(tableName, grid);
                service.exportExcel();
            }
           
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (tableName == "All")
            {
                EditDataService service = new EditDataService();
                service.ExportFullWord();
            }
            else
            {
                EditDataService service = new EditDataService(tableName, grid);
                service.ExportWord();
            }

            this.Close();
        }
    }
}
