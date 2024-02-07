using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Financial_assistant
{
    public enum TypeAdditon
    {
        INCOME,
        EXPENSES
    }
    public class Categories : INotifyPropertyChanged
    {
        private string name;
        private TypeAdditon type;
        private BitmapImage picture;
        public int ID;
        public TypeAdditon Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));

            }
        }
        public BitmapImage Picture
        {
            get
            {
                return picture;
            }
            set
            {
                picture = value;
                OnPropertyChanged(nameof(Picture));

            }
        }



        public Categories(string name)
        {
            Name = name;
        }
        public Categories(string name, TypeAdditon type)
        {
            Name = name;
            Type = type;
        }

        public Categories(string name, TypeAdditon type, BitmapImage picture)
        {
            Name = name;
            Picture = picture;
            Type = type;
        }

        public Categories(int id, string name, TypeAdditon type, BitmapImage picture)
        {
            Name = name;
            Picture = picture;
            Type = type;
            ID = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
