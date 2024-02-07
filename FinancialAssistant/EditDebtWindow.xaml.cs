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

namespace Financial_assistant
{
    /// <summary>
    /// Логика взаимодействия для EditDebtWindow.xaml
    /// </summary>
    public partial class EditDebtWindow : Window
    {
        Debt editDebt;
        public Command EditDebtCommand { get; set; }
        public EditDebtWindow(Debt newDebt)
        {
            InitializeComponent();
            this.editDebt = newDebt;

            NameDet.Field = editDebt.Name;
            NumberPayments.Field= editDebt.NumberPayments.ToString();
            AmountPayment.Field = editDebt.AmountPayment.ToString();
            Percent.Field = editDebt.Percent.ToString();
            ReceivingDate.SelectedDate = editDebt.ReceivingDate;
            MaturityDate.SelectedDate = editDebt.MaturityDate;

            NameDet.PlaceholderText.Visibility = Visibility.Hidden;
            NumberPayments.PlaceholderText.Visibility = Visibility.Hidden;
            AmountPayment.PlaceholderText.Visibility = Visibility.Hidden;
            Percent.PlaceholderText.Visibility = Visibility.Hidden;

            EditDebtCommand = new Command(o => EditDebt());
            DataContext = this;
        }

        private void EditDebt()
        {


            if (ReceivingDate.SelectedDate.Value <= MaturityDate.SelectedDate.Value)
            {
                if (Convert.ToDouble(Percent.FieldText.Text) > 100) Percent.FieldText.Text = "100";
                editDebt.Name = NameDet.FieldText.Text;
                editDebt.NumberPayments = Convert.ToInt32(NumberPayments.FieldText.Text);
                editDebt.AmountPayment = Convert.ToDouble(AmountPayment.FieldText.Text);
                editDebt.Percent = Convert.ToDouble(Percent.FieldText.Text);
                editDebt.ReceivingDate = ReceivingDate.SelectedDate.Value;
                editDebt.MaturityDate = MaturityDate.SelectedDate.Value;
                DBControler.DebtDB.Set(editDebt);
            }
            this.Close();
        }

        private void CheckInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли вводимый символ цифрой или запятой
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ",")
            {
                e.Handled = true; // Отменяем ввод символа
            }
        }

        private void CheckInputPayments(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли вводимый символ цифрой или запятой
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Отменяем ввод символа
            }
        }
    }
}
