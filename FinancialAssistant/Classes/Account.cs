using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Financial_assistant
{
    public enum Role
    {
        USER,
        ADMIN
    }
    public static class Account
    {
        public static string Name { get; set; } = "Без имени";
        public static BitmapImage Photo { get; set; }

        public static int ID { get; set; } = 1;
        public static Role UserRole { get; set; } = Role.USER;

        public static double Budget { get; set;}

        public static List<Categories> CategoriesIncomes { get; set;}
        public static List<Categories> CategoriesExpenses { get; set;}
    }
}
