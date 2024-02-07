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
    /// Логика взаимодействия для PsswordField.xaml
    /// </summary>
    public partial class PasswordField : UserControl
    {
        public PasswordField()
        {
            InitializeComponent();
            PlaceholderText.Visibility = Visibility.Visible;
        }

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        "PlaceholderPassword", typeof(string), typeof(PlaceholdField), new PropertyMetadata(string.Empty));

        public string PlaceholderPassword
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        private void HidePlaceholder(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = Visibility.Hidden;

        }

        private void ShowPlaceholder(object sender, RoutedEventArgs e)
        {
            if (FieldText.Password == "") PlaceholderText.Visibility = Visibility.Visible;
        }




        //public static readonly DependencyProperty FieldProperty = DependencyProperty.Register(
        //"FieldPassword", typeof(string), typeof(PlaceholdField), new PropertyMetadata(string.Empty));

        //public string FieldPassword
        //{
        //    get { return (string)GetValue(FieldProperty); }
        //    set { SetValue(FieldProperty, value); }
        //}

    }
}
