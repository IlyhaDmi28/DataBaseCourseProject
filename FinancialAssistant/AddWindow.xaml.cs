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
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public List<Categories> Categories { get; set; }
        public Command AddCommand { get; set; }
        public AddWindow()
        {
            InitializeComponent();

            Income.Checked += Type_Change;
            Expenses.Checked += Type_Change;

            Income.IsChecked = true;
            Date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            AddCommand = new Command(o => AddExecute());

            DataContext = this;
        }



        private void AddExecute()
        {
            Categories selectedCategory = ((Categories)CategoriesBox.SelectedItem);
            if (Amount.FieldText.Text != "")
            {

                DBControler.AdditionDB.Add(new Addition(Convert.ToDouble(Amount.FieldText.Text), selectedCategory, Date.SelectedDate.Value));
            }
            this.Close();
        }

        public void Category_GotFocus(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.Text == "Выберите значение")
            {
                comboBox.Text = "";
            }
        }

        private void Type_Change(object sender, RoutedEventArgs e)
        {
            //if (Income.IsChecked == true) Categories = DBControler.CatigoryDB.Get(TypeAdditon.INCOME).ToList();
            //else Categories = DBControler.CatigoryDB.Get(TypeAdditon.EXPENSES).ToList();

            if (Income.IsChecked == true) Categories = Account.CategoriesIncomes;
            else Categories = Account.CategoriesExpenses;

            CategoriesBox.ItemsSource = Categories;
            if(Categories.Count > 0) CategoriesBox.SelectedItem = Categories[0];
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
