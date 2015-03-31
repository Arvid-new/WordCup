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
    /// Interaction logic for FinalsMatch.xaml
    /// </summary>
    public partial class FinalsMatch : UserControl
    {
        Match match;
        TeamLabelLeft homeTeam;
        TeamLabelRight awayTeam;

        public FinalsMatch(Match match)
        {
            this.match = match;
            InitializeComponent();
            SetTeams(match);
            SetMatchData(match);
            SetScore(match);
            HomeScore.TextChanged += HomeScore_TextChanged;
            HomeScoreAET.TextChanged += HomeScore_TextChanged;
            HomeScorePenalties.TextChanged += HomeScore_TextChanged;
            AwayScore.TextChanged += AwayScore_TextChanged;
            AwayScoreAET.TextChanged += AwayScore_TextChanged;
            AwayScorePenalties.TextChanged += AwayScore_TextChanged;
            match.TeamSet += match_TeamSet;
        }

        void match_TeamSet(object sender, EventArgs e)
        {
            if (match.HomeTeam != null)
                homeTeam.SetTeam(match.HomeTeam);
            if (match.AwayTeam != null)
                awayTeam.SetTeam(match.AwayTeam);
        }

        protected void SetScore(Match match)
        {
            if (match.homeScore.ToString() != match.awayScore.ToString())
            {
                HomeScore.Text = Convert.ToString(match.homeScore.score);
                HomeScoreAET.Text = Convert.ToString(match.homeScore.scoreAET);
                HomeScorePenalties.Text = Convert.ToString(match.homeScore.penalties);
                AwayScore.Text = Convert.ToString(match.awayScore.score);
                AwayScoreAET.Text = Convert.ToString(match.awayScore.scoreAET);
                AwayScorePenalties.Text = Convert.ToString(match.awayScore.penalties);
            }
        }

        protected void SetMatchData(Match match)
        {
            if (match.matchIndex >= TournamentManager.GroupStageMatches)
            {
                if (match.matchIndex == TournamentManager.TotalMatches - 1)
                {
                    MatchName.Content = "FINAL";
                }
                else if (match.matchIndex == TournamentManager.TotalMatches - 2)
                {
                    MatchName.Content = "THIRD PLACE PLAY OFF";
                }
            }

            string[] matchDataParts = match.matchData.Split('\n');
            MatchTimeData.Content = matchDataParts[0].Replace('\t', '\n');
            MatchLocationData.Content = matchDataParts[1] + '\n' + matchDataParts[2];
        }


        protected void SetTeams(Match match)
        {
            homeTeam = new TeamLabelLeft(match.HomeTeam);
            MatchGrid.Children.Add(homeTeam);
            Grid.SetRow(homeTeam, 0);
            Grid.SetColumn(homeTeam, 0);

            awayTeam = new TeamLabelRight(match.AwayTeam);
            MatchGrid.Children.Add(awayTeam);
            Grid.SetRow(awayTeam, 0);
            Grid.SetColumn(awayTeam, 3);
        }

        private void HomeScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetMatchScore();
        }

        private void AwayScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetMatchScore();
        }

        protected void SetMatchScore()
        {
            string homeScore = (HomeScore.Text == "" ? "0" : HomeScore.Text) +
                '-' + (HomeScoreAET.Text == "" ? "0" : HomeScoreAET.Text) +
                '-' + (HomeScorePenalties.Text == "" ? "0" : HomeScorePenalties.Text);
            string awayScore = (AwayScore.Text == "" ? "0" : AwayScore.Text) +
                '-' + (AwayScoreAET.Text == "" ? "0" : AwayScoreAET.Text) +
                '-' + (AwayScorePenalties.Text == "" ? "0" : AwayScorePenalties.Text);
            match.SetScore(Match.TeamScore.FromString(homeScore), Match.TeamScore.FromString(awayScore), false);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            match.ResetMatch();
            HomeScore.TextChanged -= HomeScore_TextChanged;
            HomeScoreAET.TextChanged -= HomeScore_TextChanged;
            HomeScorePenalties.TextChanged -= HomeScore_TextChanged;
            AwayScore.TextChanged -= AwayScore_TextChanged;
            AwayScoreAET.TextChanged -= AwayScore_TextChanged;
            AwayScorePenalties.TextChanged -= AwayScore_TextChanged;
            HomeScore.Text = "";
            AwayScore.Text = "";
            HomeScoreAET.Text = "";
            AwayScoreAET.Text = "";
            HomeScorePenalties.Text = "";
            AwayScorePenalties.Text = "";
            HomeScore.TextChanged += HomeScore_TextChanged;
            HomeScoreAET.TextChanged += HomeScore_TextChanged;
            HomeScorePenalties.TextChanged += HomeScore_TextChanged;
            AwayScore.TextChanged += AwayScore_TextChanged;
            AwayScoreAET.TextChanged += AwayScore_TextChanged;
            AwayScorePenalties.TextChanged += AwayScore_TextChanged;

        }
    }
}
