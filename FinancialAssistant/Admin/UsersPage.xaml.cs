using Financial_assistant.Classes;
using Financial_assistant.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Financial_assistant.Admin
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<User> users { get; set; }
        public Command OpenAddUserCommand { get; set; }


        public UsersPage()
        {
            InitializeComponent();

            OpenAddUserCommand = new Command(o => OpenAddUser());
            users = new ObservableCollection<User>(DBControler.UserDB.Get());
            DataContext = this;
        }

        private void OpenAddUser()
        {
            User newUser = new User();
            AddUserWindow addUserWindow = new AddUserWindow(newUser, users);
            addUserWindow.ShowDialog();
            UsersTable.Items.Refresh();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (UsersTable.SelectedItem is User selectedUser)
            {
                users.Remove(selectedUser);
                DBControler.UserDB.Delete(selectedUser.ID);
                UsersTable.Items.Refresh();
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (UsersTable.SelectedItem is User selectedUser)
            {
                EditUserWindow editUserWindow = new EditUserWindow(selectedUser);
                editUserWindow.ShowDialog();

                int index = users.IndexOf(selectedUser);
                if (index != -1)
                {
                    selectedUser = DBControler.UserDB.GetById(selectedUser.ID);
                    users[index] = selectedUser;
                }


                OnPropertyChanged(nameof(users));

                UsersTable.Items.Refresh();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
