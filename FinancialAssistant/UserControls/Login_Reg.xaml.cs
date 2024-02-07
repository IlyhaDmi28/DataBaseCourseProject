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

namespace Financial_assistant
{
    /// <summary>
    /// Логика взаимодействия для Login_Reg.xaml
    /// </summary>
    public partial class Login_Reg : UserControl
    {
        public Login_Reg()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ChangeToRegisterCommandProperty = DependencyProperty.Register("ChangeToRegisterCommand", typeof(ICommand), typeof(Login_Reg), new PropertyMetadata(null));

        public ICommand ChangeToRegisterCommand
        {
            get { return (ICommand)GetValue(ChangeToRegisterCommandProperty); }
            set { SetValue(ChangeToRegisterCommandProperty, value); }
        }

        public static readonly DependencyProperty ChangeToLoginCommandProperty = DependencyProperty.Register("ChangeToLoginCommand", typeof(ICommand), typeof(Login_Reg), new PropertyMetadata(null));

        public ICommand ChangeToLoginCommand
        {
            get { return (ICommand)GetValue(ChangeToLoginCommandProperty); }
            set { SetValue(ChangeToLoginCommandProperty, value); }
        }
    }
}
