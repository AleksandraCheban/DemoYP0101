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
    /// Логика взаимодействия для AddEditPaymentReportWindow.xaml
    /// </summary>
    public partial class AddEditPaymentReportWindow : Window
    {
        private Entities _db = new Entities();
        private PaymentReport _paymentReportToEdit = null;
        public AddEditPaymentReportWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
            Title = "Добавление платежного отчета";
        }

        public AddEditPaymentReportWindow(PaymentReport paymentReportToEdit)
        {
            InitializeComponent();
            _paymentReportToEdit = paymentReportToEdit;
            LoadComboBoxData();
            LoadPaymentReportData();
            Title = "Редактирование платежного отчета";
        }

        private void LoadComboBoxData()
        {
            try
            {
                // Загружаем пользователей
                UserComboBox.ItemsSource = _db.Users.ToList();

                // Загружаем адреса
                AddressComboBox.ItemsSource = _db.ListHousingStock.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void LoadPaymentReportData()
        {
            if (_paymentReportToEdit != null)
            {
                UserComboBox.SelectedValue = _paymentReportToEdit.UserId;
                AddressComboBox.SelectedValue = _paymentReportToEdit.AdressId;
                FlatTextBox.Text = _paymentReportToEdit.Flat;
                PeriodTextBox.Text = _paymentReportToEdit.Period;
                AccruedTextBox.Text = _paymentReportToEdit.Accrued.ToString();
                PaidTextBox.Text = _paymentReportToEdit.Paid.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация данных
                if (UserComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Выберите владельца");
                    return;
                }

                if (AddressComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Выберите адрес");
                    return;
                }

                if (string.IsNullOrWhiteSpace(FlatTextBox.Text))
                {
                    MessageBox.Show("Введите номер квартиры");
                    return;
                }

                if (string.IsNullOrWhiteSpace(PeriodTextBox.Text))
                {
                    MessageBox.Show("Введите период (например, 01.2024)");
                    return;
                }

                decimal accrued, paid;
                if (!decimal.TryParse(AccruedTextBox.Text, out accrued))
                {
                    MessageBox.Show("Введите корректное числовое значение для начисленной суммы");
                    return;
                }

                if (!decimal.TryParse(PaidTextBox.Text, out paid))
                {
                    MessageBox.Show("Введите корректное числовое значение для оплаченной суммы");
                    return;
                }

                if (_paymentReportToEdit == null)
                {
                    // Добавление новой записи
                    var newPaymentReport = new PaymentReport
                    {
                        UserId = (int)UserComboBox.SelectedValue,
                        AdressId = (int)AddressComboBox.SelectedValue,
                        Flat = FlatTextBox.Text,
                        Period = PeriodTextBox.Text,
                        Accrued = accrued,
                        Paid = paid
                    };

                    _db.PaymentReport.Add(newPaymentReport);
                }
                else
                {
                    // Редактирование существующей записи
                    var paymentReport = _db.PaymentReport.Find(_paymentReportToEdit.Id);
                    if (paymentReport != null)
                    {
                        paymentReport.UserId = (int)UserComboBox.SelectedValue;
                        paymentReport.AdressId = (int)AddressComboBox.SelectedValue;
                        paymentReport.Flat = FlatTextBox.Text;
                        paymentReport.Period = PeriodTextBox.Text;
                        paymentReport.Accrued = accrued;
                        paymentReport.Paid = paid;
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