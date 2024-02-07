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

namespace Financial_assistant
{
    /// <summary>
    /// Логика взаимодействия для AddGoalWindow.xaml
    /// </summary>
    public partial class AddGoalWindow : Window, INotifyPropertyChanged
    {
        public Command AddGoalCommand { get; set; }
        public Command SelectPictureCommand { get; set; }


        public BitmapImage Picture { get; set; }
        public AddGoalWindow()
        {
            InitializeComponent();

            AddGoalCommand = new Command(o => AddGoal());
            SelectPictureCommand = new Command(o => SelectPicture());

            Picture = null;

            DataContext = this;
        }

        private void AddGoal()
        {
            if (Price.FieldText.Text != "")
            {
                DBControler.GoalDB.Add(new Goals(GoalName.FieldText.Text, 0, Convert.ToDouble(Price.FieldText.Text), Picture));
            }
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

        private void CheckInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли вводимый символ цифрой или запятой
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ",")
            {
                e.Handled = true; // Отменяем ввод символа
            }
        }
    }
}
