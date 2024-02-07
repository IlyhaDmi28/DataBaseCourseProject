using Financial_assistant.Classes;
using Microsoft.Win32;
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
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Financial_assistant
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window, INotifyPropertyChanged
    {
        ObservableCollection<User> users;
        User newUser;
        public Command AddUserCommand { get; set; }
        public Command SelectPictureCommand { get; set; }

        public BitmapImage Picture { get; set; }

        public AddUserWindow(User newUser, ObservableCollection<User> users)
        {
            InitializeComponent();
            this.newUser = newUser;
            AddUserCommand = new Command(o => AddUser());
            SelectPictureCommand = new Command(o => SelectPicture());

            this.users = users;
            DataContext = this;
        }

        private void AddUser()
        {

            newUser.Name = Name.FieldText.Text;
            newUser.Login = Login.FieldText.Text;
            newUser.Password = Password.FieldText.Text;
            newUser.Picture = Picture;

            if (UserRole.IsChecked == true) newUser.UserRole = Role.USER;
            else if(AdminRole.IsChecked == true) newUser.UserRole = Role.ADMIN;


            DBControler.UserDB.Add(newUser);
            users.Add(newUser);
            this.Close();
        }

        private void SelectPicture()
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true) Picture = new BitmapImage(new Uri(openFileDialog.FileName));

            OnPropertyChanged(nameof(Picture));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
