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
    /// Interaction logic for Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        string table;
        public Add(string table)
        {
            InitializeComponent();
            this.table = table;
            ShowForm();
        }

        public void ShowForm()
        {
            if(table == "Beneficiari")
            {
                addFrame.Navigate(new AddBeneficiari(this));
            }
            else{
                addFrame.Navigate(new Localitati());
            }
        }

    }
}
