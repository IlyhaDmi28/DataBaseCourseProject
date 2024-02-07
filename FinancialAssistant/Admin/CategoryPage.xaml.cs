using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : Page, INotifyPropertyChanged
    {
        ObservableCollection<Categories> IncomesList;
        ObservableCollection<Categories> ExpensesList;
        public CategoryPage()
        {
            InitializeComponent();


            Photo = Account.Photo;
            AccountName = Account.Name;
            


            selectedCategory = new Categories("", TypeAdditon.INCOME, null);
            CatigoriesList.SelectionChanged += CategorySelectChanged;
          


            ChangeListToIncomeCommand = new Command(o => ChangeTableToIncome());
            ChangeListToExpensesCommand = new Command(o => ChangeTableToExpenses());
            ChangeOperationToAddCommand = new Command(o => Change());
            AddCommand = new Command(o => AddCategory());
            EditCommand = new Command(o => EditCategory());
            ChangePictureCommand = new Command(o => ChangePicture());
            DeleteCommand = new Command(o => Delete());

            Change();
            ChangeTableToIncome();


            IncomesList = DBControler.CatigoryDB.Get(TypeAdditon.INCOME);
            ExpensesList = DBControler.CatigoryDB.Get(TypeAdditon.EXPENSES);
            CatigoriesList.ItemsSource = IncomesList;

            DataContext = this;
        }

        
        public ObservableCollection<Categories> CategoryList { get; set; }

        bool isIncomes;
        bool isExpenses;



        private Categories selectedCategory;
        public string CategoryName
        {
            get
            {
                return selectedCategory.Name;
            }
            set
            {
                selectedCategory.Name = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }
        public BitmapImage Picture
        {
            get
            {
                return selectedCategory.Picture;
            }
            set
            {
                selectedCategory.Picture = value;
                OnPropertyChanged(nameof(Picture));
            }
        }

        public Command ChangeListToIncomeCommand { get; set; }
        public Command ChangeListToExpensesCommand { get; set; }
        public Command AddCategoryCommand { get; set; }
        public Command ChangeOperationToAddCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public Command ChangeOperationToEditCommand { get; set; }
        public Command AddCommand { get; set; }
        public Command EditCommand { get; set; }
        public Command ChangePictureCommand { get; set; }
        public Command OpenAccountWindowCommand { get; set; }
        public Command ChangeThemeToBlueCommand { get; set; }
        public Command ChangeThemeToGreenCommand { get; set; }
        public Command ChangeThemeToDarkCommand { get; set; }
        public Command ChangeThemeToOrangeCommand { get; set; }

        ResourceDictionary dict = new ResourceDictionary();
        public string AccountName { get; set; }
        public BitmapImage Photo { get; set; }

       

        private void ChangeTableToIncome()
        {
            IncmeButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            ExpensesButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("LightGray"));

            CatigoriesList.ItemsSource = IncomesList;
            isIncomes = true;
            isExpenses = false;

            selectedCategory = new Categories("", TypeAdditon.INCOME, null);
            OnPropertyChanged(nameof(CategoryName));
            OnPropertyChanged(nameof(Picture));

            Change();
        }
        private void ChangeTableToExpenses()
        {
            IncmeButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("LightGray"));
            ExpensesButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

            CatigoriesList.ItemsSource = ExpensesList;

            isIncomes = false;
            isExpenses = true;

            selectedCategory = new Categories("", TypeAdditon.INCOME, null);
            OnPropertyChanged(nameof(CategoryName));
            OnPropertyChanged(nameof(Picture));

            Change();
        }

        private void CategorySelectChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCategory = (Categories)CatigoriesList.SelectedItem;
            if (selectedCategory != null)
            {
                CategoryField.FieldText.Text = CategoryName;
                OnPropertyChanged(nameof(Picture));
                CategoryField.PlaceholderText.Visibility = Visibility.Hidden;


                OperationButton.Command = EditCommand;
                OperationButton.Content = "Изменить";
            }


        }

        void AddCategory()
        {
            if (isIncomes == true)
            {
                Categories newCategory = new Categories(CategoryField.FieldText.Text, TypeAdditon.INCOME, Picture);
                IncomesList.Add(newCategory);
                CatigoriesList.ItemsSource = IncomesList;
                DBControler.CatigoryDB.Add(newCategory);
            }
            else
            {
                Categories newCategory = new Categories(CategoryField.FieldText.Text, TypeAdditon.EXPENSES, Picture);
                ExpensesList.Add(newCategory);
                CatigoriesList.ItemsSource = ExpensesList;
                DBControler.CatigoryDB.Add(newCategory);
            }


        }

        private void EditCategory()
        {
            selectedCategory.Name = CategoryField.FieldText.Text;
            selectedCategory.Picture = Picture;


            DBControler.CatigoryDB.Set(selectedCategory);
        }

        void Change()
        {
            selectedCategory = new Categories("", TypeAdditon.INCOME, null);
            OperationButton.Command = AddCommand;
            OperationButton.Content = "Добавить";
            CategoryField.FieldText.Text = "";
            CategoryField.PlaceholderText.Visibility = Visibility.Visible;

            Picture = null;
        }

        void Delete()
        {
            DBControler.CatigoryDB.Delete(selectedCategory.ID);
            if (isIncomes)
            {
                IncomesList.Remove(selectedCategory);
                CatigoriesList.ItemsSource = IncomesList;
            }
            if (isExpenses)
            {
                ExpensesList.Remove(selectedCategory);
                CatigoriesList.ItemsSource = ExpensesList;
            }

            Change();
        }

        void ChangePicture()
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true) Picture = new BitmapImage(new Uri(openFileDialog.FileName));
            OnPropertyChanged(nameof(Picture));
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }


}
