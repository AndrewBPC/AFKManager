using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AutoUpdaterDotNET;

namespace AFKManager
{
    /// <summary>
    /// Interaction logic for updateWindow.xaml
    /// </summary>
    public partial class updateWindow : Window
    {
        UpdateInfoEventArgs _args;
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern long DeleteUrlCacheEntry(string lpszUrlName);
        public updateWindow(UpdateInfoEventArgs args)
        {
            DeleteUrlCacheEntry("https://bottleneckpc.ru/");
            InitializeComponent();
            _args = args;
            newVersion.Text += _args.CurrentVersion;
            oldVersion.Text += _args.InstalledVersion.ToString();
        }
        
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (AutoUpdater.DownloadUpdate(_args))
            {
                this.Close();
                Application.Current.MainWindow.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Debug.Print("asdasd");
        }
    }
}
