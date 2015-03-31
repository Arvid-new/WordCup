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
using System.Windows.Shapes;

namespace WorldCup
{
    /// <summary>
    /// Interaction logic for TeamWindow.xaml
    /// </summary>
    public partial class TeamWindow : Window
    {
        public TeamWindow(Team team)
        {
            InitializeComponent();
            TeamLabelLeft teamLabel = new TeamLabelLeft(team);
            TeamGrid.Children.Add(teamLabel);
            Grid.SetRow(teamLabel, 0);
            Score.Content = team.GoalsScored();

            GroupMatch matchLabel;
            int row = 1;
            foreach (Match match in team.Matches)
            {
                matchLabel = new GroupMatch(match);
                Games.Children.Add(matchLabel);
                Grid.SetRow(matchLabel, row++);
                Grid.SetColumnSpan(matchLabel, 2);
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var AllTeamList = new AllTeamList();
            AllTeamList.Show();
        }
    }
}
