using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using BoldReports.UI.Xaml;

namespace Practica
{
    public partial class Statistici : Page
    {
        private bool[] _loaded = new bool[3];

        public Statistici()
        {
            InitializeComponent();
            LoadReport(ReportViewer1,
                Path.Combine(Environment.CurrentDirectory, "Resources", "Beneficiari.rdl"));
            _loaded[0] = true;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not TabControl tab) return;

            switch (tab.SelectedIndex)
            {
                case 0:
                    if (!_loaded[0])
                    {
                        LoadReport(ReportViewer1,
                            Path.Combine(Environment.CurrentDirectory, "Resources", "Beneficiari.rdl"));
                        _loaded[0] = true;
                    }
                    break;

                case 1:
                    if (!_loaded[1])
                    {
                        LoadReport(ReportViewer2,
                            Path.Combine(Environment.CurrentDirectory, "Resources", "Dupa orase.rdl"));
                        _loaded[1] = true;
                    }
                    break;

                case 2:
                    if (!_loaded[2])
                    {
                        LoadReport(ReportViewer3,
                            Path.Combine(Environment.CurrentDirectory, "Resources", "TipDeLoc.rdl"));
                        _loaded[2] = true;
                    }
                    break;
            }
        }

        private void LoadReport(ReportViewer viewer, string reportPath)
        {
                viewer.ProcessingMode = ProcessingMode.Remote;
                viewer.ReportPath = reportPath;
                viewer.RefreshReport();
        }
    }
}