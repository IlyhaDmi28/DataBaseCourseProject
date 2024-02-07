using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Financial_assistant
{
    public class Goals : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public int ID { get; private set; }
        public double Price { get; set; }

        public double Accumulated
        {
            get { return accumulated; }
            set
            {
                accumulated = value;
                OnPropertyChanged(nameof(Accumulated));
            }
        }

        public BitmapImage Picture
        {
            get { return picture; }
            set
            {
                picture = value;
                OnPropertyChanged(nameof(Picture));
            }
        }


        private BitmapImage picture;

        private double accumulated;
       

        

        public Goals(int id, string name, double accumulated, double price, BitmapImage picture)
        {
            ID = id;
            Name = name;
            Picture = picture;
            Price = price;
            Accumulated = accumulated;
        }
        public Goals(string name, double accumulated, double price, BitmapImage picture)
        {
            Name = name;
            Picture = picture;
            Price = price;
            Accumulated = accumulated;
        }

        public Goals(string name, double price, BitmapImage picture)
        {
            Name = name;
            Picture = picture;
            Price = price;
        }

   

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
