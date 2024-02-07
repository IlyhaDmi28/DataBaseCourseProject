using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Financial_assistant.Classes
{
    public class User : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string login;
        private BitmapImage picture;
        private string password;
        private Role userRole;

        public int ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Login
        {
            get { return login; }
            set
            {
                if (login != value)
                {
                    login = value;
                    OnPropertyChanged(nameof(Login));
                }
            }
        }

        public BitmapImage Picture
        {
            get { return picture; }
            set
            {
                if (picture != value)
                {
                    picture = value;
                    OnPropertyChanged(nameof(Picture));
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public Role UserRole
        {
            get { return userRole; }
            set
            {
                if (userRole != value)
                {
                    userRole = value;
                    OnPropertyChanged(nameof(UserRole));
                }
            }
        }

        public User()
        {

        }

        public User(int id, string name, string login, BitmapImage picture, string password, Role role)
        {
            ID = id;
            Name = name;
            Login = login;
            Picture = picture;
            Password = password;
            UserRole = role;
        }

        public User(string login, string name, string password, BitmapImage picture, Role role)
        {
            Name = name;
            Login = login;
            Picture = picture;
            Password = password;
            UserRole = role;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
