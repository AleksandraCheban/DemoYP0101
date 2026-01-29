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
    /// Логика взаимодействия для ArrearsWindow.xaml
    /// </summary>
    public partial class ArrearsWindow : Window
    {
        private Entities _db = new Entities();
        public ArrearsWindow()
        {
            InitializeComponent();
            LoadArrears();
        }
        public ArrearsWindow(Users user)
        {
            InitializeComponent();
            LoadArrears();
            

        }
        


        private void LoadArrears()
        {
            var currentUser = CurrentSession.CurrentUser;
            var reportData = _db.Arrears
           
            .Select(pr => new
            {
            pr.Id,
            pr.UserId,
            pr.AdressId,
            pr.Flat,
            pr.Phone,
            pr.Water,
            pr.ElectricPower,
            UserName = _db.Users.Where(u => u.Id == pr.UserId).Select(u => u.Name).FirstOrDefault(),
            Adress = _db.ListHousingStock.Where(lhs => lhs.Id == pr.AdressId).Select(lhs => lhs.Adress).FirstOrDefault()
        })
        .ToList();

            ReportGrid.ItemsSource = reportData;
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            
            new MenuWindow().Show();
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReportGrid.SelectedItem != null)
            {
                dynamic selectedItem = ReportGrid.SelectedItem;
                int arrearsId = selectedItem.Id;

                var result = MessageBox.Show("Вы уверены, что хотите удалить эту задолженность?",
                                            "Подтверждение удаления",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var arrearToDelete = _db.Arrears.Find(arrearsId);
                        if (arrearToDelete != null)
                        {
                            _db.Arrears.Remove(arrearToDelete);
                            _db.SaveChanges();
                            LoadArrears(); 
                            MessageBox.Show("Задолженность успешно удалена");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditArrearsWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadArrears(); 
            }
        }

        private void Remake_Click(object sender, RoutedEventArgs e)
        {
            if (ReportGrid.SelectedItem != null)
            {
                dynamic selectedItem = ReportGrid.SelectedItem;
                int arrearsId = selectedItem.Id;

                var arrearToEdit = _db.Arrears.Find(arrearsId);
                if (arrearToEdit != null)
                {
                    var editWindow = new AddEditArrearsWindow(arrearToEdit);
                    if (editWindow.ShowDialog() == true)
                    {
                        LoadArrears(); 
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования");
            }
        }
    }
}