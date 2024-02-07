using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Window = System.Windows.Window;
using Financial_assistant.Pages;
using Financial_assistant.Classes;

namespace Financial_assistant
{
    /// <summary>
    /// Логика взаимодействия для AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window, INotifyPropertyChanged
    {
        public Command SelectPictureCommand { get; set; }
        public Command LeaveCommand { get; set; }

        public BitmapImage Photo { get; set; }
        public string NameAccount { get; set; }

        Window appWindow;
        public AccountWindow(Window appWindow)
        {
            this.appWindow = appWindow;
            InitializeComponent();

            SelectPictureCommand = new Command(o => SelectPicture());
            LeaveCommand = new Command(o => Leave());

            Photo = Account.Photo;
            NameAccount = Account.Name;
            DataContext = this;
        }

        private void SelectPicture()
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)  Photo = new BitmapImage(new Uri(openFileDialog.FileName));
            
            OnPropertyChanged(nameof(Photo));
        }
        private void Leave()
        {
            Authorization authWindow = new Authorization();
            // Делаем его главным
            Application.Current.MainWindow = authWindow;
            // Закрываем старое окно
            appWindow.Close();
            this.Close();
            // Показываем новое окно
            authWindow.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Account.Name = UserName.Text;
            Account.Photo = Photo;

            DBControler.UserDB.Edit(Account.ID);

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

