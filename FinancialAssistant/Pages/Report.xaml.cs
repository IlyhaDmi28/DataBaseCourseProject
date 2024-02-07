using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Financial_assistant.Pages
{
    /// <summary>
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Addition> additionsIncomes { get; set; }
        public ObservableCollection<Addition> additionsExpenses { get; set; }
        public ObservableCollection<Addition> additions { get; set; }
        public double Money { get; set; }
        public double Income { get; set; }

        public double Expense { get; set; }



        public Command ChangeTableToIncomeCommand { get; set; }
        public Command ChangeTableToExpensesCommand { get; set; }
        public Command ChangeTableToAllCommand { get; set; }

        public Report()
        {
            InitializeComponent();


            DateTime currentDate = DateTime.Now;
            StartDate.SelectedDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            EndDate.SelectedDate = currentDate;


            StartDate.SelectedDateChanged += DatePicker_Changed;
            EndDate.SelectedDateChanged += DatePicker_Changed;
            UpdateTable();
            CalculateBudget();

            ChangeTableToIncomeCommand = new Command(O => ChangeTableToIncome());
            ChangeTableToExpensesCommand = new Command(O => ChangeTableToExpenses());
            ChangeTableToAllCommand = new Command(O => ChangeTableToAll());

            ChangeTableToIncome();
            DataContext = this;
        }

        private void UpdateTable()
        {
            List<Addition> loadData = DBControler.AdditionDB.Get(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value, TypeAdditon.INCOME);

            if (loadData != null) additionsIncomes = new ObservableCollection<Addition>(loadData);
            else additionsIncomes = null;

            loadData = DBControler.AdditionDB.Get(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value, TypeAdditon.EXPENSES);
            if (loadData != null)  additionsExpenses = new ObservableCollection<Addition>(loadData);
            else additionsExpenses = null;

            loadData = DBControler.AdditionDB.Get(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value);
            if (loadData != null)  additions = new ObservableCollection<Addition>(loadData);
            else additions = null;

            OnPropertyChanged(nameof(additionsIncomes));
            OnPropertyChanged(nameof(additionsExpenses));
            OnPropertyChanged(nameof(additions));

            IncomeTable.Items.Refresh();
            ExpensesTable.Items.Refresh();
            AllTable.Items.Refresh();
        }

        private void CalculateBudget()
        {
            Money = DBControler.AdditionDB.GetBudget(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value);
            Income = DBControler.AdditionDB.GetIncomesSum(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value);
            Expense = DBControler.AdditionDB.GetExpensesSum(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value);

            OnPropertyChanged(nameof(Money));
            OnPropertyChanged(nameof(Income));
            OnPropertyChanged(nameof(Expense));
        }

        
        private void ChangeTableToIncome()
        {
            IncmeButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            ExpensesButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("LightGray"));
            AllButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("LightGray"));

            IncomesView.Visibility = Visibility.Visible;
            ExpensesView.Visibility = Visibility.Hidden;
            AllView.Visibility = Visibility.Hidden;
        }
        private void ChangeTableToExpenses()
        {
            IncmeButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("LightGray"));
            ExpensesButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            AllButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("LightGray"));

            IncomesView.Visibility = Visibility.Hidden;
            ExpensesView.Visibility = Visibility.Visible;
            AllView.Visibility = Visibility.Hidden;
        }

        private void ChangeTableToAll()
        {
            IncmeButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("LightGray"));
            ExpensesButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("LightGray"));
            AllButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

            IncomesView.Visibility = Visibility.Hidden;
            ExpensesView.Visibility = Visibility.Hidden;
            AllView.Visibility = Visibility.Visible;
        }

        public void DatePicker_Changed(object sender, SelectionChangedEventArgs e)
        {
            UpdateTable();
            CalculateBudget();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (IncomesView.Visibility == Visibility.Visible)
            {
                if (IncomeTable.SelectedItem is Addition selectedItem)
                {
                    DBControler.AdditionDB.Delete(selectedItem.ID);
                    additionsIncomes.Remove(selectedItem);

                    int ind = additions.IndexOf(additions.FirstOrDefault(add => add.ID == selectedItem.ID));
                    additions.RemoveAt(ind);

                    AllTable.Items.Refresh();
                    IncomeTable.Items.Refresh();
                    OnPropertyChanged(nameof(additionsIncomes));
                    OnPropertyChanged(nameof(additions));
                }
            }
            else if (ExpensesView.Visibility == Visibility.Visible)
            {
                if (ExpensesTable.SelectedItem is Addition selectedItem)
                {
                    DBControler.AdditionDB.Delete(selectedItem.ID);
                    additionsExpenses.Remove(selectedItem);

                    int ind = additions.IndexOf(additions.FirstOrDefault(add => add.ID == selectedItem.ID));
                    additions.RemoveAt(ind);
                    AllTable.Items.Refresh();
                    ExpensesTable.Items.Refresh();
                    OnPropertyChanged(nameof(additionsExpenses));
                    OnPropertyChanged(nameof(additions));
                }
            }
            if (AllView.Visibility == Visibility.Visible)
            {
                if (AllTable.SelectedItem is Addition selectedItem)
                {
                    DBControler.AdditionDB.Delete(selectedItem.ID);
                    switch(selectedItem.Category.Type)
                    {
                        case TypeAdditon.INCOME:
                            {
                                additionsIncomes.Remove(selectedItem);
                                IncomeTable.Items.Refresh();
                                break;
                            }
                        case TypeAdditon.EXPENSES:
                            {
                                additionsExpenses.Remove(selectedItem);
                                ExpensesTable.Items.Refresh();
                                break;
                            }
                    }

                    int ind = additions.IndexOf(additions.FirstOrDefault(add => add.ID == selectedItem.ID));
                    additions.RemoveAt(ind);
                    AllTable.Items.Refresh();
                    OnPropertyChanged(nameof(additionsExpenses));
                    OnPropertyChanged(nameof(additionsIncomes));
                    OnPropertyChanged(nameof(additions));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
