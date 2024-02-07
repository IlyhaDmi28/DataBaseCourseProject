using System;
using System.Collections.Generic;
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

namespace Financial_assistant.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Command ChangeToRegisterCommand { get; set; }
        public Command ChangeToLoginCommand { get; set; }

        public Authorization()
        {
            InitializeComponent();
            login_reg.ChangeToLoginCommand = new Command(o => ChangeToLogin());
            login_reg.ChangeToRegisterCommand = new Command(o => ChangeToRegister());
            AuthorizationFrame.Source = new Uri("Login.xaml", UriKind.Relative);
        }

        private void ChangeToRegister()
        {
            login_reg.RegisterButton.Background = new SolidColorBrush(Color.FromRgb(0x00, 0x75, 0xFF));
            login_reg.RegisterButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            login_reg.LoginButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            login_reg.LoginButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));

        }

        private void ChangeToLogin()
        {
            login_reg.LoginButton.Background = new SolidColorBrush(Color.FromRgb(0x00, 0x75, 0xFF));
            login_reg.LoginButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            login_reg.RegisterButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            login_reg.RegisterButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));

            AuthorizationFrame.Source = new Uri("Login.xaml", UriKind.Relative);
        }
    }
}
