using System.Data;
using System.Windows;

namespace Practica
{
    public partial class DateSelectateAleBeneficiarilor : Window
    {
        private string _tip;

        public DateSelectateAleBeneficiarilor(string tip)
        {
            InitializeComponent();
            Title = $"Beneficiari - {tip}";
            _tip = tip;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DataBase db = new DataBase();
            DataTable table = db.GetBeneficiariByTip(_tip);

            if (table.Rows.Count == 0)
            {
                MessageBox.Show($"Nu există beneficiari în zona {_tip}.",
                    "Niciun rezultat", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                return;
            }

            beneficiari.ItemsSource = table.DefaultView;
        }
    }
}