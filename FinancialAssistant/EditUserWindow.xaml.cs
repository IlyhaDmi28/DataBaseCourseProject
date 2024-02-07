using Financial_assistant.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace Financial_assistant
{
    /// <summary>
    /// Логика взаимодействия для EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window, INotifyPropertyChanged
    {
        User editUser;
        public Command EditUserCommand { get; set; }
        public Command SelectPictureCommand { get; set; }

        public BitmapImage Picture { get; set; }

        public EditUserWindow(User editUser)
        {
            InitializeComponent();
            this.editUser = editUser;
            EditUserCommand = new Command(o => EditUser());
            SelectPictureCommand = new Command(o => SelectPicture());


            Name.Field = editUser.Name;
            Login.Field = editUser.Login;

            switch (editUser.UserRole)
            {
                case Role.USER: UserRole.IsChecked = true; break;
                case Role.ADMIN: AdminRole.IsChecked = true; break;
            }

            Name.PlaceholderText.Visibility = Visibility.Hidden;
            Login.PlaceholderText.Visibility = Visibility.Hidden;
            Password.PlaceholderText.Visibility = Visibility.Hidden;

            Password.FieldText.IsEnabled = false;
            Password.FieldText.Background = System.Windows.Media.Brushes.LightGray;

            Picture = editUser.Picture;
            OnPropertyChanged(nameof(Picture));

            DataContext = this;
        }

        private void EditUser()
        {

            editUser.Name = Name.FieldText.Text;
            editUser.Login = Login.FieldText.Text;
            if(IsPasswordInput.IsChecked == true) editUser.Password = Password.FieldText.Text;
            editUser.Picture = Picture;

            if (UserRole.IsChecked == true) editUser.UserRole = Role.USER;
            else if (AdminRole.IsChecked == true) editUser.UserRole = Role.ADMIN;


            DBControler.UserDB.Set(editUser, IsPasswordInput.IsChecked.Value);
            this.Close();
        }

        private void SelectPicture()
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true) Picture = new BitmapImage(new Uri(openFileDialog.FileName));

            OnPropertyChanged(nameof(Picture));
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Password.FieldText.IsEnabled = true;
            Password.FieldText.Background = null;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Password.FieldText.IsEnabled = false;
            Password.FieldText.Background = System.Windows.Media.Brushes.LightGray;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
