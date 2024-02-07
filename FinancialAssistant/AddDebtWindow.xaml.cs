using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
using System.Xml.Linq;

namespace Financial_assistant
{
    /// <summary>
    /// Логика взаимодействия для AddDebtWindow.xaml
    /// </summary>
    public partial class AddDebtWindow : Window
    {
        Debt newDebt;
        public Command AddDebtCommand { get; set; }
        public AddDebtWindow(Debt newDebt)
        {
            InitializeComponent();
            this.newDebt = newDebt;
            AddDebtCommand = new Command(o => AddDebtl());
            DataContext = this;
        }

        private void AddDebtl()
        {
           

            if (ReceivingDate.SelectedDate.Value <= MaturityDate.SelectedDate.Value)
            {
                if (Convert.ToDouble(Percent.FieldText.Text) > 100) Percent.FieldText.Text = "100";
                newDebt.Name = NameDet.FieldText.Text;
                newDebt.Paid = 0;
                newDebt.NumberPayments = Convert.ToInt32(NumberPayments.FieldText.Text);
                newDebt.AmountPayment = Convert.ToDouble(AmountPayment.FieldText.Text);
                newDebt.Percent = Convert.ToDouble(Percent.FieldText.Text);
                newDebt.ReceivingDate = ReceivingDate.SelectedDate.Value;
                newDebt.MaturityDate = MaturityDate.SelectedDate.Value;
                DBControler.DebtDB.Add(newDebt);
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
