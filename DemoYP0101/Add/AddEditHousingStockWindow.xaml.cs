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
    /// Логика взаимодействия для AddEditHousingStockWindow.xaml
    /// </summary>
    public partial class AddEditHousingStockWindow : Window
    {
        private Entities _db = new Entities();
        private ListHousingStock _housingStockToEdit = null;
        public AddEditHousingStockWindow()
        {
            InitializeComponent();
            Title = "Добавление жилищного фонда";
        }
        public AddEditHousingStockWindow(ListHousingStock housingStockToEdit)
        {
            InitializeComponent();
            _housingStockToEdit = housingStockToEdit;
            LoadHousingStockData();
            Title = "Редактирование жилищного фонда";
        }

        private void LoadHousingStockData()
        {
            if (_housingStockToEdit != null)
            {
                AddressTextBox.Text = _housingStockToEdit.Adress;

                // Для даты используем DatePicker
                // Beginning - это DateTime (не nullable), поэтому просто присваиваем
                BeginningDatePicker.SelectedDate = _housingStockToEdit.Beginning;

                FloorsTextBox.Text = _housingStockToEdit.Floors;
                FlatTextBox.Text = _housingStockToEdit.Flat;
                YearTextBox.Text = _housingStockToEdit.Year;
                SquareTextBox.Text = _housingStockToEdit.Square.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация данных
                if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
                {
                    MessageBox.Show("Введите адрес");
                    return;
                }

                if (BeginningDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату начала управления");
                    return;
                }

                if (string.IsNullOrWhiteSpace(FloorsTextBox.Text))
                {
                    MessageBox.Show("Введите количество этажей");
                    return;
                }

                if (string.IsNullOrWhiteSpace(FlatTextBox.Text))
                {
                    MessageBox.Show("Введите количество квартир");
                    return;
                }

                if (string.IsNullOrWhiteSpace(YearTextBox.Text))
                {
                    MessageBox.Show("Введите год постройки");
                    return;
                }

                if (string.IsNullOrWhiteSpace(SquareTextBox.Text))
                {
                    MessageBox.Show("Введите площадь");
                    return;
                }

                decimal square;
                if (!decimal.TryParse(SquareTextBox.Text, out square))
                {
                    MessageBox.Show("Введите корректное числовое значение для площади");
                    return;
                }

                if (_housingStockToEdit == null)
                {
                    // Добавление новой записи
                    var newHousingStock = new ListHousingStock
                    {
                        Adress = AddressTextBox.Text,
                        Beginning = BeginningDatePicker.SelectedDate.Value, // или .Value
                        Floors = FloorsTextBox.Text,
                        Flat = FlatTextBox.Text,
                        Year = YearTextBox.Text,
                        Square = square
                    };

                    _db.ListHousingStock.Add(newHousingStock);
                }
                else
                {
                    // Редактирование существующей записи
                    var housingStock = _db.ListHousingStock.Find(_housingStockToEdit.Id);
                    if (housingStock != null)
                    {
                        housingStock.Adress = AddressTextBox.Text;
                        housingStock.Beginning = BeginningDatePicker.SelectedDate.Value; // или .Value
                        housingStock.Floors = FloorsTextBox.Text;
                        housingStock.Flat = FlatTextBox.Text;
                        housingStock.Year = YearTextBox.Text;
                        housingStock.Square = square;
                    }
                }

                _db.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}