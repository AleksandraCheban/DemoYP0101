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
using System.Data.SqlClient;
using System.Data.Entity;

namespace DemoYP0101
{
    /// <summary>
    /// Логика взаимодействия для ListHousingStockWindow.xaml
    /// </summary>
    public partial class ListHousingStockWindow : Window
    {
        private Entities _db = new Entities();
        public ListHousingStockWindow()
        {
            InitializeComponent();
            LoadList();
            CheckUserRole();
        }
        public ListHousingStockWindow(Users user)
        {
            InitializeComponent();
            LoadList();
            CheckUserRole();
        }
        private void CheckUserRole()
        {
            var currentUser = CurrentSession.CurrentUser;

            if (currentUser == null || currentUser.RoleId == 2)
            {

                AddButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
                Remake.Visibility = Visibility.Collapsed;
            }
            else
            {

                AddButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
                Remake.Visibility = Visibility.Visible;
            }
        }

        private void LoadList()
        {

            var reportData = _db.ListHousingStock
        .Select(pr => new
        {
            pr.Id,
            pr.Adress,
            pr.Beginning,
            pr.Floors,
            pr.Flat,
            pr.Year,
            pr.Square,
            
        })
        .ToList();

            ListGrid.ItemsSource = reportData;
        }


        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            
            new MenuWindow().Show();
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListGrid.SelectedItem != null)
            {
                dynamic selectedItem = ListGrid.SelectedItem;
                int housingStockId = selectedItem.Id;

                var result = MessageBox.Show("Вы уверены, что хотите удалить эту запись жилищного фонда?",
                                            "Подтверждение удаления",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Проверяем, есть ли связанные записи в других таблицах
                        bool hasRelatedArrears = _db.Arrears.Any(a => a.AdressId == housingStockId);
                        bool hasRelatedPayments = _db.PaymentReport.Any(p => p.AdressId == housingStockId);

                        if (hasRelatedArrears || hasRelatedPayments)
                        {
                            MessageBox.Show("Невозможно удалить запись, так как с ней связаны задолженности или платежи",
                                          "Ошибка",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                            return;
                        }

                        var housingStockToDelete = _db.ListHousingStock.Find(housingStockId);
                        if (housingStockToDelete != null)
                        {
                            _db.ListHousingStock.Remove(housingStockToDelete);
                            _db.SaveChanges();
                            LoadList(); // Обновляем таблицу
                            MessageBox.Show("Запись успешно удалена",
                                          "Успех",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                                      "Ошибка",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var addWindow = new AddEditHousingStockWindow();
                if (addWindow.ShowDialog() == true)
                {
                    LoadList(); // Обновляем таблицу после добавления
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна добавления: {ex.Message}");
            }
        }

        private void Remake_Click(object sender, RoutedEventArgs e)
        {
            if (ListGrid.SelectedItem != null)
            {
                try
                {
                    dynamic selectedItem = ListGrid.SelectedItem;
                    int housingStockId = selectedItem.Id;

                    var housingStockToEdit = _db.ListHousingStock.Find(housingStockId);
                    if (housingStockToEdit != null)
                    {
                        var editWindow = new AddEditHousingStockWindow(housingStockToEdit);
                        if (editWindow.ShowDialog() == true)
                        {
                            LoadList(); // Обновляем таблицу после редактирования
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при редактировании: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
            }
        }
    }
}