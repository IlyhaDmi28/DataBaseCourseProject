using Financial_assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using UserControl = System.Windows.Controls.UserControl;

namespace Financial_assistant
{
    /// <summary>
    /// Логика взаимодействия для PlaceholdrField.xaml
    /// </summary>
    public partial class PlaceholdField : UserControl
    {
        public PlaceholdField()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        "Placeholder", typeof(string), typeof(PlaceholdField), new PropertyMetadata(string.Empty));

        public string Placeholder
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
            if(FieldText.Text == "" )PlaceholderText.Visibility = Visibility.Visible;
        }




        public static readonly DependencyProperty FieldProperty = DependencyProperty.Register(
        "Field", typeof(string), typeof(PlaceholdField), new PropertyMetadata(string.Empty));

        public string Field
        {
            get { return (string)GetValue(FieldProperty); }
            set { SetValue(FieldProperty, value); }
        }
    }


}
