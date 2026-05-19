using Bold.Licensing;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Practica
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App() {
            BoldLicenseProvider.RegisterLicense("Bdmurziqv3DwF6+w3JqS/XJfl3p73gMtc6AbPH9nAjE=");
        }
    }

}
