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
    /// Interaction logic for AllTeamList.xaml
    /// </summary>
    public partial class AllTeamList : Window
    {
        class TeamScore : IComparable
        {
            public Team team;
            public int goals;

            public int CompareTo(object obj)
            {
                TeamScore otherTeamScore = (TeamScore)obj;
                int returnValue = this.goals - otherTeamScore.goals;
                if (returnValue == 0)
                    returnValue = otherTeamScore.team.TeamIndex - this.team.TeamIndex;

                return returnValue;
            }
        }

        public AllTeamList()
        {
            InitializeComponent();
            TeamScore[] teams = new TeamScore[TournamentManager.NumberOfTeams];
            for (int teamIndex = 0; teamIndex < TournamentManager.NumberOfTeams; teamIndex++)
            {
                teams[teamIndex] = new TeamScore()
                {
                    team = TournamentManager.Teams[teamIndex],
                    goals = TournamentManager.Teams[teamIndex].GoalsScored()
                };
            }

            IComparable[] array = teams;
            Functions.Sort(ref array, 0, teams.Length - 1);
            teams = (TeamScore[])array;

            TeamLabelLeft label;
            Label score;
            int topPosition = 10;
            int leftScore = 10;
            int leftTeam = 35;
            foreach (TeamScore team in teams)
            {
                score = new Label() { Content = Convert.ToString(team.goals) };
                TeamList.Children.Add(score);
                Canvas.SetTop(score, topPosition);
                Canvas.SetLeft(score, leftScore);

                label = new TeamLabelLeft(team.team);
                TeamList.Children.Add(label);
                Canvas.SetTop(label, topPosition);
                Canvas.SetLeft(label, leftTeam);

                topPosition += 30;
            }
        }
    }
}
