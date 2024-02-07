using Financial_assistant.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Financial_assistant
{
    public class GoalViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Goals> GoalsList { get; set; }

        public ListBox goalList;

        private Goals selectedGoal;

        public Goals SelectedGoal
        {
            get { return selectedGoal; }
            set
            {
                selectedGoal = value;
                OnPropertyChanged("SelectedGoal");
            }
        }

        public Command OpenAddGoalCommand { get; set; }
        public Command OpenInvestmentCommand { get; set; }
        public Command SelectPictureCommand { get; set; }

        public GoalViewModel(ListBox goalList)
        {
            this.goalList = goalList;
            this.goalList.SelectionChanged += SelectGoal;


            OpenAddGoalCommand = new Command(o => OpenAddGoal());
            OpenInvestmentCommand = new Command(o => OpenInvestmen());
            SelectPictureCommand = new Command(o => ChangePicture());

            GoalsList = DBControler.GoalDB.Get();


            if (GoalsList.Count > 0)
            {
                selectedGoal = GoalsList[0];
                Accumulated = selectedGoal.Accumulated;
                Price = selectedGoal.Price;
                NameGoal = selectedGoal.Name;
                Picture = selectedGoal.Picture;
                OnPropertyChanged(nameof(Accumulated));
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(NameGoal));
                OnPropertyChanged(nameof(Picture));
            }
            else
            {
                SelectedGoal = new Goals("Нет цели", 0, 0, null);
            }
        }

        public double Accumulated
        {
            get { return SelectedGoal.Accumulated; }
            set
            {
                SelectedGoal.Accumulated = value;
                OnPropertyChanged("Accumulated");
            }
        }
        public double Price
        {
            get { return SelectedGoal.Price; }
            set
            {
                SelectedGoal.Price = value;
                OnPropertyChanged("Price");
            }
        }


        public string NameGoal
        {
            get { return SelectedGoal.Name; }
            set
            {
                SelectedGoal.Name = value;
                OnPropertyChanged("NameGoal");
            }
        }
        public BitmapImage Picture
        {
            get { return SelectedGoal.Picture; }
            set
            {
                SelectedGoal.Picture = value;
                OnPropertyChanged("Picture");
            }
        }

        public void DeleteFromGoals(Goals goal)
        {
            GoalsList.Remove(goal);
            goalList.Items.Refresh();
            OnPropertyChanged("GoalsList");
        }


        private void OpenAddGoal()
        {
            AddGoalWindow addGoalWindow = new AddGoalWindow();
            addGoalWindow.ShowDialog();
            GoalsList = DBControler.GoalDB.Get();
            OnPropertyChanged(nameof(GoalsList));
        }

        private void OpenInvestmen()
        {
            InvestmentWindow investmentWindow = new InvestmentWindow(selectedGoal);
            investmentWindow.ShowDialog();
            Accumulated = selectedGoal.Accumulated;
            OnPropertyChanged(nameof(Accumulated));
            OnPropertyChanged(nameof(GoalsList));
        }

        private void SelectGoal(object sender, SelectionChangedEventArgs e)
        {
            selectedGoal = (Goals)goalList.SelectedItem;
            if (selectedGoal != null)
            {
                Accumulated = selectedGoal.Accumulated;
                Price = selectedGoal.Price;
                NameGoal = selectedGoal.Name;
                Picture = selectedGoal.Picture;
                OnPropertyChanged(nameof(Accumulated));
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(NameGoal));
                OnPropertyChanged(nameof(Picture));
            }
        }

        public void SetNewSelectGoal(Goals goal)
        {
            Accumulated = goal.Accumulated;
            Price = goal.Price;
            NameGoal = goal.Name;
            Picture = goal.Picture;
            OnPropertyChanged(nameof(Accumulated));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(NameGoal));
            OnPropertyChanged(nameof(Picture));
        }
        private void ChangePicture()
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true) selectedGoal.Picture = new BitmapImage(new Uri(openFileDialog.FileName));
            Picture = selectedGoal.Picture;
            OnPropertyChanged(nameof(Picture));
            DBControler.GoalDB.Set(SelectedGoal);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
