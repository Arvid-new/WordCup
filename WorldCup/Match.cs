using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCup
{
    public class Match
    {
        public enum Stage { Group, Knockout, Final };
        public enum Result { NotDrawn, NotPlayed, HomeWin, AwayWin, Draw };
        public enum MatchLength { Normal, ExtraTime, Penalties };

        public struct TeamScore
        {
            public int score;
            public int scoreAET;
            public int penalties;

            /// <summary>
            /// Returns the match result based on a comparison of the home team and away team scores.
            /// </summary>
            /// <param name="other">The away team's score</param>
            /// <returns></returns>
            internal Result CompareTo(TeamScore other)
            {
                if (this.score > other.score) // Home win
                {
                    return Result.HomeWin;
                }
                else if (this.score < other.score) // Away win
                {
                    return Result.AwayWin;
                }
                else
                {
                    if (this.scoreAET > other.scoreAET) //Home win
                    {
                        return Result.HomeWin;
                    }
                    else if (this.scoreAET < other.scoreAET)
                    {
                        return Result.AwayWin;
                    }
                    else
                    {
                        if (this.penalties > other.penalties)
                        {
                            return Result.HomeWin;
                        }
                        else if (this.penalties < other.penalties)
                        {
                            return Result.AwayWin;
                        }
                        else
                        {
                            return Result.Draw;
                        }
                    }
                }
            }

            internal static TeamScore FromString(string score)
            {
                TeamScore newScore = new TeamScore();
                string[] scoreParts = score.Split('-');
                newScore.score = Convert.ToInt32(scoreParts[0]);
                newScore.scoreAET = Convert.ToInt32(scoreParts[1]);
                newScore.penalties = Convert.ToInt32(scoreParts[2]);

                return newScore;
            }

            public override string ToString()
            {
                string newString = "";
                newString += Convert.ToString(score);
                newString += '-' + Convert.ToString(scoreAET);
                newString += '-' + Convert.ToString(penalties);

                return newString;
            }
        }

        public Team HomeTeam { get; private set; }
        public Team AwayTeam { get; private set; }

        public TeamScore homeScore, awayScore;

        public Stage matchStage;
        public Result matchResult;

        public int nextMatchIndex;
        public int matchIndex;

        public event EventHandler<int> ScoreChanged;
        public event EventHandler<int> UpdatingMatch;
        public string matchData;

        public Match(int index)
        {
            matchIndex = index;
            ProcessMatchData(TournamentManager.MatchData[index]);
            ScoreChanged += Match_ScoreChanged;
            UpdatingMatch += Match_UpdatingMatch;
            matchStage = GetMatchStage(index);
            if (matchStage == Stage.Knockout)
            {
                SetNextMatch(index);
            }
            TeamSet += Match_TeamSet;
        }

        void Match_TeamSet(object sender, EventArgs e)
        {
            UpdateNextMatch(false);
            TournamentManager.MatchUpdated();
        }

        void Match_UpdatingMatch(object sender, int e)
        {
            TournamentManager.MatchData[e] = TournamentManager.MatchData[e].Substring(0, TournamentManager.MatchData[e].Length - 12);
        }

        void Match_ScoreChanged(object sender, int e)
        {
            TournamentManager.MatchData[e] = TournamentManager.MatchData[e].Substring(0, TournamentManager.MatchData[e].Length - 1);
            string scoreToAdd = "";
            scoreToAdd += Convert.ToString((int)matchResult);
            if (matchResult != Result.NotDrawn && matchResult != Result.NotPlayed)
            {
                scoreToAdd += ',' + homeScore.ToString();
                scoreToAdd += ',' + awayScore.ToString();
            }
            TournamentManager.MatchData[e] += scoreToAdd;
            TournamentManager.MatchUpdated();
        }

        private void ProcessMatchData(string matchData)
        {
            string[] dataItems = matchData.Split(',');

            this.matchData = dataItems[0];
            this.matchData += '\t' + dataItems[1];
            this.matchData += '\n' + dataItems[2];
            this.matchData += '\n' + dataItems[3];

            matchResult = (Result)Convert.ToInt32(dataItems[4]);

            if (matchResult != Result.NotDrawn && matchResult != Result.NotPlayed)
            {
                TournamentManager.MatchesCompleted++;
                SetScore(TeamScore.FromString(dataItems[5]), TeamScore.FromString(dataItems[6]), false);
            }
        }

        private void SetNextMatch(int index)
        {
            int matchesLeft = TournamentManager.TotalMatches - index;
            if (matchesLeft > 4) // If not in the semi final stages.
            {
                int differenceToNextMatch = (matchesLeft + 1) / 2;
                nextMatchIndex = TournamentManager.TotalMatches - TournamentManager.GroupStageMatches - differenceToNextMatch;
            }
            else
            {
                ScoreChanged += UpdateFinals;
                TeamSet += SetFinalTeams;
            }
        }

        private void SetFinalTeams(object sender, EventArgs e)
        {
            SetFinalsData();
        }

        private void UpdateFinals(object sender, int e)
        {
            SetFinalsData();
        }

        private void SetFinalsData()
        {
            if (HomeTeam != null && AwayTeam != null)
                TournamentManager.SetFinalsData(this);
        }

        private Stage GetMatchStage(int index)
        {
            if (index < TournamentManager.GroupStageMatches)
            {
                return Stage.Group;
            }
            else if (index >= TournamentManager.TotalMatches - 2)
            {
                return Stage.Final;
            }
            else
            {
                return Stage.Knockout;
            }
        }

        public event EventHandler TeamSet;

        public void SetHomeTeam(Team team)
        {
            if (team != null)
            {
                if (HomeTeam != null)
                    HomeTeam.Matches.Remove(this);
                HomeTeam = team;
                team.Matches.Add(this);
                if (TeamSet != null)
                    TeamSet(this, new EventArgs());
                if (AwayTeam != null && matchResult == Result.NotDrawn)
                {
                    matchResult = Result.NotPlayed;
                }
            }
        }
        public void SetAwayTeam(Team team)
        {
            if (team != null)
            {
                if (AwayTeam != null)
                    AwayTeam.Matches.Remove(this);
                AwayTeam = team;
                team.Matches.Add(this);
                if (TeamSet != null)
                    TeamSet(this, new EventArgs());
                if (HomeTeam != null && matchResult == Result.NotDrawn)
                {
                    matchResult = Result.NotPlayed;
                }
            }
        }

        public void SetScore(TeamScore home, TeamScore away, bool resettingMatch)
        {
            if (matchResult != Result.NotDrawn && matchResult != Result.NotPlayed)
            {
                if (UpdatingMatch != null)
                    UpdatingMatch(this, this.matchIndex);
                if (resettingMatch)
                {
                    TournamentManager.MatchesCompleted--;
                }
            }
            else if (!resettingMatch)
            {
                TournamentManager.MatchesCompleted++;
            }

            homeScore = home;
            awayScore = away;

            if (!resettingMatch)
            {
                matchResult = homeScore.CompareTo(awayScore);
            }
            else
            {
                matchResult = Result.NotPlayed;
            }

            if (HomeTeam != null && AwayTeam != null)
                UpdateNextMatch(resettingMatch);

            if (ScoreChanged != null)
                ScoreChanged(this, this.matchIndex);
        }

        private void UpdateNextMatch(bool resettingMatch)
        {
            if (matchStage == Stage.Knockout && !resettingMatch)
            {
                if (HomeTeam != null)
                    HomeTeam.InTournament = (matchResult != Result.AwayWin);
                if (AwayTeam != null)
                    AwayTeam.InTournament = (matchResult != Result.HomeWin);

                if (this.matchIndex < TournamentManager.TotalMatches - 4)
                {
                    if (matchResult == Result.HomeWin)
                    {
                        if ((matchIndex & 1) == 1)
                            TournamentManager.KnockoutStages.Matches[nextMatchIndex].SetAwayTeam(this.HomeTeam);
                        else
                            TournamentManager.KnockoutStages.Matches[nextMatchIndex].SetHomeTeam(this.HomeTeam);
                    }
                    if (matchResult == Result.AwayWin)
                    {
                        if ((matchIndex & 1) == 1)
                            TournamentManager.KnockoutStages.Matches[nextMatchIndex].SetAwayTeam(this.AwayTeam);
                        else
                            TournamentManager.KnockoutStages.Matches[nextMatchIndex].SetHomeTeam(this.AwayTeam);
                    }
                }
            }
            else if (matchStage == Stage.Knockout && resettingMatch)
            {
                if (HomeTeam != null)
                    HomeTeam.InTournament = true;
                if (AwayTeam != null)
                    AwayTeam.InTournament = true;

                if (this.matchIndex < TournamentManager.TotalMatches - 4)
                {
                    TournamentManager.KnockoutStages.Matches[nextMatchIndex].ResetMatch();
                    if ((matchIndex & 1) == 1)
                        TournamentManager.KnockoutStages.Matches[nextMatchIndex].ResetAwayTeam();
                    else
                        TournamentManager.KnockoutStages.Matches[nextMatchIndex].ResetHomeTeam();

                }
            }
            else if (matchStage == Stage.Final && !resettingMatch)
            {
                if (HomeTeam != null)
                    HomeTeam.InTournament = false;
                if (AwayTeam != null)
                    AwayTeam.InTournament = false;
            }
            else if (matchStage == Stage.Final && resettingMatch)
            {
                if (HomeTeam != null)
                    HomeTeam.InTournament = true;
                if (AwayTeam != null)
                    AwayTeam.InTournament = true;
            }
        }

        internal void ResetHomeTeam()
        {
            HomeTeam.Matches.Remove(this);
            HomeTeam = new Team();
            TeamSet(this, new EventArgs());
        }

        internal void ResetAwayTeam()
        {
            AwayTeam.Matches.Remove(this);
            AwayTeam = new Team();
            TeamSet(this, new EventArgs());
        }

        private MatchLength GetMatchLength(TeamScore home, TeamScore away)
        {
            if (matchStage == Stage.Group)
            {
                return MatchLength.Normal;
            }
            else
            {
                if (home.score == away.score)
                {
                    if (home.scoreAET == away.scoreAET)
                    {
                        return MatchLength.Penalties;
                    }
                    else
                    {
                        return MatchLength.ExtraTime;
                    }
                }
                else
                {
                    return MatchLength.Normal;
                }
            }
        }

        internal void ResetMatch()
        {
            SetScore(new TeamScore(), new TeamScore(), true);
            matchResult = Result.NotPlayed;
        }
    }
}
