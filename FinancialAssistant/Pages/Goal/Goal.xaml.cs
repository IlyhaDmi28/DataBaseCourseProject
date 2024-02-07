using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace Financial_assistant.Pages
{
    /// <summary>
    /// Логика взаимодействия для Goal.xaml
    /// </summary>
    public partial class Goal : Page
    {
        GoalViewModel goalViewModel;
        public Goal()
        {
            InitializeComponent();

            goalViewModel = new GoalViewModel(GoalList);
            DataContext = goalViewModel;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (goalViewModel.goalList.SelectedItem is Goals selectGoal)
            {
                if (selectGoal == goalViewModel.SelectedGoal)
                {

                    if (selectGoal == goalViewModel.GoalsList[0])
                    {
                        if (goalViewModel.GoalsList.Count == 1)
                        {
                            goalViewModel.SelectedGoal = new Goals("Нет цели", 0, 0, null);
                            goalViewModel.SetNewSelectGoal(goalViewModel.SelectedGoal);
                        }
                        else goalViewModel.SelectedGoal = goalViewModel.GoalsList[1];
                        goalViewModel.SetNewSelectGoal(goalViewModel.SelectedGoal);
                    }
                    else
                    {
                        goalViewModel.SelectedGoal = goalViewModel.GoalsList[0];
                        goalViewModel.SetNewSelectGoal(goalViewModel.SelectedGoal);
                    }
                }

                goalViewModel.GoalsList.Remove(selectGoal);
                goalViewModel.goalList.Items.Refresh();
                
                DBControler.GoalDB.Delete(selectGoal.ID);
            }
        }
    }
}
