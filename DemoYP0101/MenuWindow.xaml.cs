using Demo2modl.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DemoYP0101
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }
        public MenuWindow(Users user)
        {
            InitializeComponent();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            new ReportWindow().Show();
            Close();
        }

        private void ListHousingStockButton_Click(object sender, RoutedEventArgs e)
        {
            new ListHousingStockWindow().Show();
            Close();
        }

        private void ArrearsButton_Click(object sender, RoutedEventArgs e)
        {
            new ArrearsWindow().Show();
            Close();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentSession.CurrentUser = null;
            new MainWindow().Show();
            Close();
        }

        private void ApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            new ApplicationWindow().Show();
            Close();
        }
    }
}
