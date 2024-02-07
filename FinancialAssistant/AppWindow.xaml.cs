using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для AppWindow.xaml
    /// </summary>
    public partial class AppWindow : Window, INotifyPropertyChanged
    {
        public Command ChangeToDiagramCommand { get; set; }
        public Command ChangeToBudgetCommand { get; set; }
        public Command ChangeToGoalsCommand { get; set; }
        public Command ChangeToReportCommand { get; set; }
        public Command ChangeToDebtCommand { get; set; }
        public Command OpenAccountWindowCommand { get; set; }
        ResourceDictionary dict = new ResourceDictionary();

        public string AccountName { get; set; }
        public BitmapImage Photo { get; set; }

        public double Money { get; set; }

        public AppWindow()
        {
            InitializeComponent();

  
            dict.Source = new Uri("/Themes/BlueTheme.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dict);


            PageContent.Source = new Uri("Budget.xaml", UriKind.Relative);
            ChangeToDiagramCommand = new Command(o => ChangeToDiagram());
            ChangeToBudgetCommand = new Command(o => ChangeToBudget());
            ChangeToGoalsCommand = new Command(o => ChangeToGoals());
            ChangeToReportCommand = new Command(o => ChangeToReport());
            OpenAccountWindowCommand = new Command(o => OpenAccountWindow());
            ChangeToDebtCommand = new Command(o => ChangeToDebt());

            Photo = Account.Photo;
            AccountName = Account.Name;

            ChangeToBudget();

            DataContext = this;
        }

        private void ChangeToDiagram()
        {
            PageContent.Source = new Uri("Pages/Diagram.xaml", UriKind.Relative);
        }

        private void ChangeToBudget()
        {
            PageContent.Source = new Uri("Pages/Budget.xaml", UriKind.Relative);
        }

        private void ChangeToGoals()
        {
            PageContent.Source = new Uri("Pages/Goal/Goal.xaml", UriKind.Relative);
        }

        private void ChangeToReport()
        {
            PageContent.Source = new Uri("Pages/Report.xaml", UriKind.Relative);
        }

        private void ChangeToDebt()
        {
            PageContent.Source = new Uri("Pages/Debt.xaml", UriKind.Relative);
        }

        private void OpenAccountWindow()
        {
            AccountWindow account = new AccountWindow(this);

            account.ShowDialog();

            AccountName = Account.Name;
            Photo = Account.Photo;
            OnPropertyChanged(nameof(AccountName));
            OnPropertyChanged(nameof(Photo));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
