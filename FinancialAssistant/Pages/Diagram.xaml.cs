using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Financial_assistant.Pages
{
    /// <summary>
    /// Логика взаимодействия для Diagram.xaml
    /// </summary>
    public partial class Diagram : Page
    { 
        public SeriesCollection DiagrammIncometData { get; set; }
        public SeriesCollection DiagrammExpensesData { get; set; }

        private List<Addition> additions { get; set; }

        public ObservableCollection<MoneyAtCategory> moneyAtCategorysIncome { get; set; }
        public ObservableCollection<MoneyAtCategory> moneyAtCategorysExpenses { get; set; }
        public Diagram()
        {
            InitializeComponent();

            DateTime currentDate = DateTime.Now;
            StartDateIncomes.SelectedDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            StartDateExpenses.SelectedDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            EndDateIncomes.SelectedDate = currentDate;
            EndDateExpenses.SelectedDate = currentDate;

            StartDateIncomes.SelectedDateChanged += DatePickerIncomes_Changed;
            EndDateIncomes.SelectedDateChanged += DatePickerIncomes_Changed;
            StartDateExpenses.SelectedDateChanged += DatePickerExpenses_Changed;
            EndDateExpenses.SelectedDateChanged += DatePickerExpenses_Changed;

            SetIncomeData();
            SetExpensesData();



            DataContext = this;

        }

        private void SetIncomeData()
        {
            try
            {
                additions = DBControler.AdditionDB.Get(StartDateIncomes.SelectedDate.Value, EndDateIncomes.SelectedDate.Value, TypeAdditon.INCOME);
                if (additions == null)
                {
                    DiagrammIncometData = null;
                    moneyAtCategorysIncome = null;
                    DiagrammIncome.Series = DiagrammIncometData;
                    IncomeList.ItemsSource = moneyAtCategorysIncome;
                    return;
                }
                var incomes = additions.Select(add => add.Category);

                if (incomes.Count() == 0)
                {
                    DiagrammIncometData = null;
                    moneyAtCategorysIncome = null;
                    DiagrammIncome.Series = DiagrammIncometData;
                    IncomeList.ItemsSource = moneyAtCategorysIncome;
                    return;
                }

                var categories = incomes.Select(income => income.Name);
                categories = categories.Distinct();

                moneyAtCategorysIncome = new ObservableCollection<MoneyAtCategory>();
                foreach (var categorie in categories)
                {
                    moneyAtCategorysIncome.Add(new MoneyAtCategory(categorie));
                }

                foreach (Addition add in additions)
                {
                    MoneyAtCategory money = moneyAtCategorysIncome.FirstOrDefault(moneyAtCategory => moneyAtCategory.Category == add.Category.Name);
                    if (money != null)
                    {
                        //money.Amounts += add.Amount;
                        double percent = 0;
                        money.Amounts = DBControler.CatigoryDB.GetIncomesByCategory(StartDateIncomes.SelectedDate.Value, EndDateIncomes.SelectedDate.Value, add.Category.ID, out percent);
                        money.Percent = percent;
                        money.Picture = add.Category.Picture;
                    }
                }

                DiagrammIncometData = new SeriesCollection();

                foreach (MoneyAtCategory moneyAtCategory in moneyAtCategorysIncome)
                {
                    DiagrammIncometData.Add(new PieSeries { Title = moneyAtCategory.Category, DataLabels = true, Values = new ChartValues<ObservableValue> { new ObservableValue(moneyAtCategory.Amounts) } });
                }

                DiagrammIncome.Series = DiagrammIncometData;
                IncomeList.ItemsSource = moneyAtCategorysIncome;
            }
            catch
            {

            }
        }

        private void SetExpensesData()
        {
            try
            {
                additions = DBControler.AdditionDB.Get(StartDateExpenses.SelectedDate.Value, EndDateExpenses.SelectedDate.Value, TypeAdditon.EXPENSES);
                if (additions == null)
                {
                    DiagrammExpensesData = null;
                    moneyAtCategorysExpenses = null;
                    DiagrammExpenses.Series = DiagrammExpensesData;
                    ExpensesList.ItemsSource = moneyAtCategorysExpenses;
                    return;
                }
                var expenses = additions.Select(add => add.Category);

                if (expenses.Count() == 0)
                {
                    DiagrammExpensesData = null;
                    moneyAtCategorysExpenses = null;
                    DiagrammExpenses.Series = DiagrammExpensesData;
                    ExpensesList.ItemsSource = moneyAtCategorysExpenses;
                    return;
                }

                var categories = expenses.Select(expens => expens.Name);
                categories = categories.Distinct().OrderBy(categorie => categorie);

                moneyAtCategorysExpenses = new ObservableCollection<MoneyAtCategory>();
                foreach (string categorie in categories)
                {
                    moneyAtCategorysExpenses.Add(new MoneyAtCategory(categorie));
                }

                foreach (Addition add in additions)
                {
                    MoneyAtCategory money = moneyAtCategorysExpenses.FirstOrDefault(moneyAtCategory => moneyAtCategory.Category == add.Category.Name);
                    if (money != null)
                    {
                        //money.Amounts += add.Amount;
                        double percent = 0;
                        money.Amounts = DBControler.CatigoryDB.GetExpensesByCategory(StartDateExpenses.SelectedDate.Value, EndDateExpenses.SelectedDate.Value, add.Category.ID, out percent);
                        money.Percent = percent;
                        money.Picture = add.Category.Picture;
                    }
                }

                DiagrammExpensesData = new SeriesCollection();

                foreach (MoneyAtCategory moneyAtCategory in moneyAtCategorysExpenses)
                {
                    DiagrammExpensesData.Add(new PieSeries { Title = moneyAtCategory.Category, DataLabels = true, Values = new ChartValues<ObservableValue> { new ObservableValue(moneyAtCategory.Amounts) } });
                }

                DiagrammExpenses.Series = DiagrammExpensesData;
                ExpensesList.ItemsSource = moneyAtCategorysExpenses;
            }
            catch
            {

            }
        }

        private void DatePickerIncomes_Changed(object sender, SelectionChangedEventArgs e) => SetIncomeData();

        private void DatePickerExpenses_Changed(object sender, SelectionChangedEventArgs e) => SetExpensesData();


        private void Picture_category_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
