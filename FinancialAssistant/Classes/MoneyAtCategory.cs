using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Financial_assistant
{
    public class MoneyAtCategory
    {
        public double Amounts { get; set; } = 0;
        public double Percent { get; set; } = 0;
        public string Category { get; set; }
        public BitmapImage Picture { get; set; }



        public MoneyAtCategory(string category)
        {
            Category = category;
        }
    }
}
