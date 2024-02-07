using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial_assistant
{
    public class MoneyAtTime
    {
        public double Amounts { get; set; } = 0; 
        public string Time { get; set; }



        public MoneyAtTime(string time)
        {
            Time = time;
        }
    }
}
