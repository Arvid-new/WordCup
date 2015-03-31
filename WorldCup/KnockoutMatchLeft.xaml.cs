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
    /// Interaction logic for KnockoutMatch.xaml
    /// </summary>
    public partial class KnockoutMatchLeft : UserControl
    {
        protected Match match;

        protected TeamLabelLeft homeTeam, awayTeam;

        public KnockoutMatchLeft(Match match)
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

        protected void match_TeamSet(object sender, EventArgs e)
        {
            if (match.HomeTeam != null)
                homeTeam.SetTeam(match.HomeTeam);
            if (match.AwayTeam != null)
                awayTeam.SetTeam(match.AwayTeam);
        }

        protected void SetMatchData(Match match)
        {
            if (match.matchIndex >= TournamentManager.GroupStageMatches)
            {
                if (match.matchIndex < TournamentManager.GroupStageMatches + TournamentManager.NumberOfGroups)
                {
                    MatchStage.Content = "Last Sixteen " + Convert.ToString((match.matchIndex % TournamentManager.NumberOfGroups) + 1);
                }
                else if (match.matchIndex < TournamentManager.GroupStageMatches + TournamentManager.NumberOfGroups * 3 / 2f)
                {
                    MatchStage.Content = "Quarter Final " + Convert.ToString((match.matchIndex % (TournamentManager.NumberOfGroups / 2)) + 1);
                }
                else
                {
                    MatchStage.Content = "Semi Final " + Convert.ToString((match.matchIndex % (TournamentManager.NumberOfGroups / 4)) + 1);
                }
            }

            string matchData = match.matchData.Replace('\n', '\t');
            MatchData.Content = matchData;
        }

        protected virtual void SetTeams(Match match)
        {
            homeTeam = new TeamLabelLeft(match.HomeTeam);
            Data.Children.Add(homeTeam);
            Grid.SetRow(homeTeam, 1);
            Grid.SetColumn(homeTeam, 0);

            awayTeam = new TeamLabelLeft(match.AwayTeam);
            Data.Children.Add(awayTeam);
            Grid.SetRow(awayTeam, 2);
            Grid.SetColumn(awayTeam, 0);
        }

        protected void HomeScore_TextChanged(object sender, TextChangedEventArgs e)
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

        protected void AwayScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetMatchScore();
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
