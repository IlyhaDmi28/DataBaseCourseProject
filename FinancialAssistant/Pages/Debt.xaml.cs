using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Financial_assistant.Pages
{
    /// <summary>
    /// Логика взаимодействия для Debt.xaml
    /// </summary>
    public partial class Debts : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Debt> debts { get; set; }

        public double Debt { get; set; }
        public double TotalDebt { get; set; }
        public double Paid { get; set; }
        public Command OpenAddDebtCommand { get; set; }
        public Debts()
        {
            InitializeComponent();

            OpenAddDebtCommand = new Command(o => OpenAddDebt());

            try
            {
                debts = new ObservableCollection<Debt>(DBControler.DebtDB.Get());
            }
            catch 
            {
            }
            CalculateDebtInfo();

            DataContext = this;
        }

        private void CalculateDebtInfo()
        {
            TotalDebt = DBControler.DebtDB.GetTotalDebt();
            Paid = DBControler.DebtDB.GetPaid();

            OnPropertyChanged(nameof(Debt));
            OnPropertyChanged(nameof(TotalDebt));
            OnPropertyChanged(nameof(Paid));
        }

        private void OpenAddDebt()
        {
            Debt newDebt = new Debt();
            AddDebtWindow addDebtWindow = new AddDebtWindow(newDebt);
            addDebtWindow.ShowDialog();
            try
            {
                debts = new ObservableCollection<Debt>(DBControler.DebtDB.Get());
                DebtTable.ItemsSource = debts;
            }
            catch
            {
            }

            CalculateDebtInfo();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (DebtTable.SelectedItem is Debt selectedDebt)
            {
                debts.Remove(selectedDebt);
                DBControler.DebtDB.Delete(selectedDebt.ID);
                DebtTable.Items.Refresh();
                CalculateDebtInfo();
            }
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {

            if (DebtTable.SelectedItem is Debt selectedDebt)
            {
                selectedDebt.Paid += 1;
                DBControler.DebtDB.Set(selectedDebt);
                int index = debts.IndexOf(selectedDebt);
                if (index != -1)
                {
                    selectedDebt = DBControler.DebtDB.GetById(selectedDebt.ID);
                    debts[index] = selectedDebt;
                }
                OnPropertyChanged(nameof(debts));
                DebtTable.Items.Refresh();

                CalculateDebtInfo();
            }
        }
        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (DebtTable.SelectedItem is Debt selectedDebt)
            {
                EditDebtWindow editDebtWindow = new EditDebtWindow(selectedDebt);
                editDebtWindow.ShowDialog();
       
                int index = debts.IndexOf(selectedDebt);
                if (index != -1)
                {
                    selectedDebt = DBControler.DebtDB.GetById(selectedDebt.ID);
                    debts[index] = selectedDebt;
                }


                OnPropertyChanged(nameof(debts));

                DebtTable.Items.Refresh();
                CalculateDebtInfo();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
