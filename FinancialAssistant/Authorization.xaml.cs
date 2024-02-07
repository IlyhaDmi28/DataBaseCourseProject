using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Financial_assistant
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Command LoginCommand { get; set; }
        public Command RegisterCommand { get; set; }
        public Command ChangeToRegisterCommand { get; set; }
        public Command ChangeToLoginCommand { get; set; }

        public Authorization()
        {
            //DBControler.DBGenerate();
            InitializeComponent();
            
            DBControler.ChangeToAdmin();
            LoginCommand = new Command(o => ExecuteLoggin());
            RegisterCommand = new Command(o => ExecuteRegister());
            login_reg.ChangeToLoginCommand = new Command(o => ChangeToLogin());
            login_reg.ChangeToRegisterCommand = new Command(o => ChangeToRegister());



            ChangeToLogin();

            DataContext = this;
        }

        public void ExecuteRegister()
        {
            if (PasswordReg.FieldText.Password != RepeatPasswordReg.FieldText.Password) ErrorMessageReg.Text = "Пароли не совпадают!";
            else
            {
                try
                {
                    DBControler.UserDB.Register(EmailReg.FieldText.Text, NameReg.FieldText.Text, PasswordReg.FieldText.Password);

                    Account.ID = DBControler.UserDB.GetByLogin(EmailReg.FieldText.Text).ID;
                    Account.Name = NameReg.FieldText.Text;
                    AppWindow appWindow = new AppWindow();
                    Application.Current.MainWindow = appWindow;
                    this.Close();
                    appWindow.Show();
                }
                catch (Exception ex)
                {
                    ErrorMessageReg.Text = "Пользователь с введёным логином существует!";
                }
            }
        }

        public void ExecuteLoggin()
        {
            
            if (DBControler.UserDB.Loggin(EmailLogin.FieldText.Text, PasswordLogin.FieldText.Password))
            {   
                if(Account.UserRole == Role.ADMIN)
                {
                    
                    AdminWindow adminWindow = new AdminWindow();
                    // Делаем его главным
                    Application.Current.MainWindow = adminWindow;
                    // Закрываем старое окно
                    this.Close();
                    // Показываем новое окно
                    adminWindow.Show();
                    Account.CategoriesIncomes = DBControler.CatigoryDB.Get(TypeAdditon.INCOME).ToList();
                    Account.CategoriesExpenses = DBControler.CatigoryDB.Get(TypeAdditon.EXPENSES).ToList();
                }
                else
                {
                    DBControler.ChangeToUser();
                    AppWindow appWindow = new AppWindow();
                    // Делаем его главным
                    Application.Current.MainWindow = appWindow;
                    // Закрываем старое окно
                    this.Close();
                    // Показываем новое окно
                    appWindow.Show();
                    Account.CategoriesIncomes = DBControler.CatigoryDB.Get(TypeAdditon.INCOME).ToList();
                    Account.CategoriesExpenses = DBControler.CatigoryDB.Get(TypeAdditon.EXPENSES).ToList();
                }
            }
            else ErrorMessageLog.Text = "Неверный логин или пароль";
        }

        private void ChangeToRegister()
        {
            login_reg.RegisterButton.Background = new SolidColorBrush(Color.FromRgb(0x00, 0x75, 0xFF));
            login_reg.RegisterButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            login_reg.LoginButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            login_reg.LoginButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));

            LoginForm.Visibility = Visibility.Hidden;
            RegForm.Visibility = Visibility.Visible;
        }

        private void ChangeToLogin()
        {
            login_reg.LoginButton.Background = new SolidColorBrush(Color.FromRgb(0x00, 0x75, 0xFF));
            login_reg.LoginButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            login_reg.RegisterButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            login_reg.RegisterButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));

            LoginForm.Visibility = Visibility.Visible;
            RegForm.Visibility = Visibility.Hidden;
        }
    }
}
