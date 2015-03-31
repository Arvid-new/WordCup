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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorldCup
{
    /// <summary>
    /// Interaction logic for TeamGoalsLabel.xaml
    /// </summary>
    public partial class TeamGoalsLabel : UserControl
    {
        Team team;
        TeamLabelLeft teamLabel;

        public TeamGoalsLabel(Team team)
        {
            InitializeComponent();
            SetTeam(team);
        }

        public void SetTeam(Team team)
        {
            if (this.team != null)
            {
                teamLabel.SetTeam(team);
            }
            else
            {
                teamLabel = new TeamLabelLeft(team);
                ContentGrid.Children.Add(teamLabel);
                teamLabel.Margin = new Thickness(5);
                Grid.SetColumn(teamLabel, 0);
            }

            this.team = team;
            GoalsScored.Content = Convert.ToString(team.GoalsScored());

            if (team.InTournament)
                GoalsScored.FontWeight = FontWeights.Bold;
            else
                GoalsScored.FontWeight = FontWeights.Normal;
        }
    }
}
