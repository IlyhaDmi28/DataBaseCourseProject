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
    /// Логика взаимодействия для InvestmentWindow.xaml
    /// </summary>
    public partial class InvestmentWindow : Window
    {
        private Goals goal;
        public Command InvestmentCommand { get; set; }
        public InvestmentWindow(Goals goal)
        {
            InitializeComponent();
            this.goal = goal;
            DataContext = this;
            InvestmentCommand = new Command(o => Investment());
        }

        private void Investment()
        {
            if (Accumulated.FieldText.Text != "")
            {
                goal.Accumulated += Convert.ToDouble(Accumulated.FieldText.Text);
                DBControler.GoalDB.Set(goal);
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
    }
}
