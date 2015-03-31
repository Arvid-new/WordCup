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
    /// Interaction logic for Group.xaml
    /// </summary>
    public partial class GroupLabel : UserControl
    {
        Group group;
        public Group Group
        {
            get { return group; }
            set { group = value; }
        }

        TeamLabelLeft[] teams;
        Label[,] points;

        public GroupLabel(Group group)
        {
            InitializeComponent();
            this.Group = group;
            group.RepopulateScores += group_RepopulateScores;
            SetGroupNameContent();
            AddTeams();
            InitialiseScores();
            SetScoreBoxes();
        }

        void group_RepopulateScores(object sender, EventArgs e)
        {
            SetScoreBoxes();
        }

        private void SetScoreBoxes()
        {
            for (int row = 0; row < TournamentManager.GroupSize; row++)
            {
                if (group.PositionsSecure[row])
                {
                    for (int column = 0; column < 6; column++)
                    {
                        points[row, column].FontWeight = FontWeights.Bold;
                    }
                }
                else
                {
                    for (int column = 0; column < 6; column++)
                    {
                        points[row, column].FontWeight = FontWeights.Normal;
                    }
                }

                GroupTeam team = group.Teams[row];
                teams[row].SetTeam(team.team);
                points[row, 0].Content = Convert.ToString(team.Played);
                points[row, 1].Content = Convert.ToString(team.Won);
                points[row, 2].Content = Convert.ToString(team.Drawn);
                points[row, 3].Content = Convert.ToString(team.Lost);
                points[row, 4].Content = Convert.ToString(team.GD);
                points[row, 5].Content = Convert.ToString(team.Points);
            }
        }

        private void InitialiseScores()
        {
            points = new Label[TournamentManager.GroupSize, 6];
            for (int row = 0; row < TournamentManager.GroupSize; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    points[row, column] = new Label() { HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right};
                    Grid.SetRow(points[row, column], row + 1);
                    Grid.SetColumn(points[row, column], column + 1);
                    GroupTable.Children.Add(points[row, column]);
                }
            }
        }

        void SetGroupNameContent()
        {
            GroupName.Content = (char)((int)'A' + Group.GroupIndex);
        }

        void AddTeams()
        {
            teams = new TeamLabelLeft[4];

            for (int teamIndex = 0; teamIndex < 4; teamIndex++)
            {
                teams[teamIndex] = new TeamLabelLeft(Group.Teams[teamIndex].team);
                GroupTable.Children.Add(teams[teamIndex]);
                Grid.SetColumn(teams[teamIndex], 0);
                Grid.SetRow(teams[teamIndex], teamIndex + 1);
            }
        }
    }
}
