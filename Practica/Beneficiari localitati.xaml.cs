using System.Data;
using System.Windows;

namespace Practica
{
    public partial class Beneficiari_localitati : Window
    {
        private int _codLoc;

        public Beneficiari_localitati(int codLoc)
        {
            InitializeComponent();
            _codLoc = codLoc;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DataBase db = new DataBase();
            DataTable table = db.GetBeneficiariByLocalitate(_codLoc);

            if (table.Rows.Count == 0)
            {
                MessageBox.Show($"Nu există beneficiari pentru localitatea cu codul {_codLoc}.",
                    "Niciun rezultat", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                return;
            }

            beneficiari.ItemsSource = table.DefaultView;
        }
    }
}