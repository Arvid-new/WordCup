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
    /// Interaction logic for GroupMatch.xaml
    /// </summary>
    public partial class GroupMatch : UserControl
    {
        Match match;
        bool initialising;

        public GroupMatch(Match match)
        {
            initialising = true;
            InitializeComponent();
            this.match = match;

            TeamLabelLeft homeLabel = new TeamLabelLeft(match.HomeTeam);
            TeamLabelLeft awayLabel = new TeamLabelLeft(match.AwayTeam);

            MainGrid.Children.Add(homeLabel);
            Grid.SetRow(homeLabel, 0);
            Grid.SetColumn(homeLabel, 0);
            MainGrid.Children.Add(awayLabel);
            Grid.SetRow(awayLabel, 1);
            Grid.SetColumn(awayLabel, 0);
            MatchData.Content = match.matchData;

            if (match.matchResult != Match.Result.NotDrawn && match.matchResult != Match.Result.NotPlayed)
            {
                HomeScore.Text = Convert.ToString(match.homeScore.score);
                AwayScore.Text = Convert.ToString(match.awayScore.score);
            }
            initialising = false;
        }

        private void HomeScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!initialising)
                SetMatchScore();
        }

        private void SetMatchScore()
        {
            int homeScore = 0;
            int awayScore = 0;
            bool succeeded = int.TryParse(HomeScore.Text, out homeScore) && int.TryParse(AwayScore.Text, out awayScore);

            if (succeeded)
                match.SetScore(new Match.TeamScore() { score = homeScore }, new Match.TeamScore() { score = awayScore }, false);
        }

        private void AwayScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!initialising)
                SetMatchScore();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            match.ResetMatch();
            HomeScore.Text = "";
            AwayScore.Text = "";
        }
    }
}
