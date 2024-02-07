using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Application = System.Windows.Application;

namespace Financial_assistant
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    /// 

    public partial class AdminWindow : Window, INotifyPropertyChanged
    {
        public Command ChangeToCategoryCommand { get; set; }
        public Command ChangeToUsersCommand { get; set; }
        public Command OpenAccountWindowCommand { get; set; }

        ResourceDictionary dict = new ResourceDictionary();

        public string AccountName { get; set; }
        public BitmapImage Photo { get; set; }
        public AdminWindow()
        {
            InitializeComponent();

            dict.Source = new Uri("/Themes/BlueTheme.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dict);


            ChangeToCategoryCommand = new Command(o => ChangeToCategory());
            ChangeToUsersCommand = new Command(o => ChangeToUsers());
            OpenAccountWindowCommand = new Command(o => OpenAccountWindow());


            Photo = Account.Photo;
            AccountName = Account.Name;
            OnPropertyChanged(nameof(AccountName));
            OnPropertyChanged(nameof(Photo));

            ChangeToCategory();

            DataContext = this;
        }

        private void ChangeToCategory()
        {
            PageContent.Source = new Uri("CategoryPage.xaml", UriKind.Relative);
        }

        private void ChangeToUsers()
        {
            PageContent.Source = new Uri("UsersPage.xaml", UriKind.Relative);
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
