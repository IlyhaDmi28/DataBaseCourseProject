using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
using Separator = LiveCharts.Wpf.Separator;

namespace Financial_assistant.Pages
{
    /// <summary>
    /// Логика взаимодействия для Budget.xaml
    /// </summary>
    public partial class Budget : Page, INotifyPropertyChanged
    {
        private double budget;
        private double income;
        private double expense;
        public double Money
        {
            get
            {
                return budget;
            }
            set
            {
                budget = value;
            }
        }

        public double Income
        {
            get
            {
                return income;
            }
            set
            {
                income = value;
            }
        }

        public double Expense
        {
            get
            {
                return expense;
            }
            set
            {
                expense = value;
            }
        }

        public Command OpenAddWindowCommand { get; set; }

        List<Addition> additionsIncomes;
        List<Addition> additionsExpenses;
        List<Addition> additions;

        private List<MoneyAtTime> IncomesList = new List<MoneyAtTime>();
        private List<MoneyAtTime> ExpensesList = new List<MoneyAtTime>();
        private List<MoneyAtTime> BudgetList = new List<MoneyAtTime>();
        
        public Budget()
        {
            InitializeComponent();

            DateTime currentDate = DateTime.Now;
            StartDate.SelectedDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            EndDate.SelectedDate = currentDate;

            StartDate.SelectedDateChanged += DatePicker_Changed;
            EndDate.SelectedDateChanged += DatePicker_Changed;

            OpenAddWindowCommand = new Command(o => OpenAddWindow());
            DataContext = this;
            ShowIncomes.Checked += RadioButton_Checked;
            ShowExpenses.Checked += RadioButton_Checked;
            ShowBudget.Checked += RadioButton_Checked;


            additionsIncomes = DBControler.AdditionDB.Get(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value, TypeAdditon.INCOME);
            additionsExpenses = DBControler.AdditionDB.Get(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value, TypeAdditon.EXPENSES);
            CalculateBudget();


            ShowIncomes.IsChecked = true;
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

        private void CountBudget()
        {
            BudgetList.Clear();

            additions = DBControler.AdditionDB.Get(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value);
            var time_moments = additions.Select(add => add.Date).Distinct().OrderBy(add => add.Date);

            foreach (var time_moment in time_moments)
            {
                BudgetList.Add(new MoneyAtTime(time_moment.ToString("yyyy-MM-dd")));
            }

            foreach (Addition add in additions)
            {
                MoneyAtTime money = BudgetList.FirstOrDefault(moneyAtTime => moneyAtTime.Time == add.Date.ToString("yyyy-MM-dd"));

                if (add.Category.Type == TypeAdditon.INCOME) money.Amounts += add.Amount;
                else money.Amounts -= add.Amount;
            }

            for (int i = 0; i < BudgetList.Count; i++)
            {
                if (i > 0) BudgetList[i].Amounts += BudgetList[i - 1].Amounts;
            }

        }

        private void CountIncome()
        {
            try
            {
                IncomesList.Clear();


                var time_moments = additionsIncomes.Select(add => add.Date).Distinct().OrderBy(add => add.Date);

                foreach (var time_moment in time_moments)
                {
                    IncomesList.Add(new MoneyAtTime(time_moment.ToString("yyyy-MM-dd")));
                }

                foreach (Addition add in additionsIncomes)
                {
                    MoneyAtTime money = IncomesList.FirstOrDefault(moneyAtTime => moneyAtTime.Time == add.Date.ToString("yyyy-MM-dd"));

                    money.Amounts += add.Amount;
                }
            }
            catch
            {

            }
        }

        private void CountExpenses()
        {
            try
            {
                ExpensesList.Clear();

                var time_moments = additionsExpenses.Select(add => add.Date).Distinct().OrderBy(add => add.Date);

                foreach (var time_moment in time_moments)
                {
                    ExpensesList.Add(new MoneyAtTime(time_moment.ToString("yyyy-MM-dd")));
                }

                foreach (Addition add in additionsExpenses)
                {
                    MoneyAtTime money = ExpensesList.FirstOrDefault(moneyAtTime => moneyAtTime.Time == add.Date.ToString("yyyy-MM-dd"));

                    money.Amounts += add.Amount;
                }
            }
            catch
            {

            }
        }
        private void UpdateGraphik()
        {
            try
            {
                if (ShowIncomes.IsChecked == true)
                {
                    CountIncome();

                    Graphik.Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<double>(IncomesList.Select(money => money.Amounts)),
                        Stroke = System.Windows.Media.Brushes.Green
                    }
                };

                    Graphik.AxisY[0].MinValue = 0;

                    var time_moments = additionsIncomes.Select(add => add.Date).Distinct().OrderBy(add => add.Date);

                    foreach (var time_moment in time_moments)
                    {
                        BudgetList.Add(new MoneyAtTime(time_moment.ToString("yyyy-MM-dd")));
                    }



                    Graphik.AxisX.Clear();
                    Graphik.AxisX.Add(new Axis
                    {
                        Labels = new ChartValues<string>(IncomesList.Select(money => money.Time)),
                        FontSize = 24,
                        LabelsRotation = -45,

                        Separator = new Separator
                        {
                            StrokeThickness = 0
                        }
                    });
                }

                if (ShowExpenses.IsChecked == true)
                {
                    CountExpenses();

                    Graphik.Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<double>(ExpensesList.Select(money => money.Amounts)),
                        Stroke = System.Windows.Media.Brushes.Red
                    }
                };

                    Graphik.AxisY[0].MinValue = 0;

                    var time_moments = additionsExpenses.Select(add => add.Date).Distinct().OrderBy(add => add.Date);

                    foreach (var time_moment in time_moments)
                    {
                        BudgetList.Add(new MoneyAtTime(time_moment.ToString("yyyy-MM-dd")));
                    }



                    Graphik.AxisX.Clear();
                    Graphik.AxisX.Add(new Axis
                    {
                        Labels = new ChartValues<string>(ExpensesList.Select(money => money.Time)),
                        FontSize = 24,
                        LabelsRotation = -45,

                        Separator = new Separator
                        {
                            StrokeThickness = 0
                        }
                    });
                }

                if (ShowBudget.IsChecked == true)
                {
                    CountBudget();

                    Graphik.Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<double>(BudgetList.Select(money => money.Amounts)),
                    }
                };

                    if (BudgetList.Count != 0)
                    {
                        if (BudgetList.Min(money => money.Amounts) >= 0) Graphik.AxisY[0].MinValue = 0;
                        else Graphik.AxisY[0].MinValue = BudgetList.Min(money => money.Amounts);
                    }

                    var time_moments = additions.Select(add => add.Date).Distinct().OrderBy(add => add.Date);

                    foreach (var time_moment in time_moments)
                    {
                        BudgetList.Add(new MoneyAtTime(time_moment.ToString("yyyy-MM-dd")));
                    }



                    Graphik.AxisX.Clear();
                    Graphik.AxisX.Add(new Axis
                    {
                        Labels = new ChartValues<string>(BudgetList.Select(money => money.Time)),
                        FontSize = 24,
                        LabelsRotation = -45,

                        Separator = new Separator
                        {
                            StrokeThickness = 0
                        }
                    });
                }
            }
            catch
            {

            }
           
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e) => UpdateGraphik();
        
        

        private void OpenAddWindow()
        {
            AddWindow addWindow = new AddWindow();
            addWindow.ShowDialog();

            additionsIncomes = DBControler.AdditionDB.Get(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value, TypeAdditon.INCOME);
            additionsExpenses = DBControler.AdditionDB.Get(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value, TypeAdditon.EXPENSES);
            CalculateBudget();


            UpdateGraphik();
        }

        private void DatePicker_Changed(object sender, SelectionChangedEventArgs e)
        {
            additionsIncomes = DBControler.AdditionDB.Get(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value, TypeAdditon.INCOME);
            additionsExpenses = DBControler.AdditionDB.Get(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value, TypeAdditon.EXPENSES);

            CalculateBudget();


            UpdateGraphik();
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
