using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCup
{
    public class GroupTeam : IComparable
    {
        public Team team;

        public GroupTeam(Team team)
        {
            this.team = team;
        }

        public int Points { get; set;}
        public int For { get; set; }
        public int Against { get; set; }
        public int GD { get { return For - Against; } }
        public int Played{ get; set; }
        public int Won{ get; set; }
        public int Lost{ get; set; }
        public int Drawn{ get; set; }

        public void UpdateParameters(Match match)
        {
            if (match.HomeTeam.TeamIndex == team.TeamIndex)
            {
                if (match.matchResult == Match.Result.HomeWin)
                {
                    Points += 3;
                    Won++;
                }
                if (match.matchResult == Match.Result.Draw)
                {
                    Points += 1;
                    Drawn++;
                }
                if (match.matchResult == Match.Result.AwayWin)
                {
                    Lost++;
                }
                if (match.matchResult != Match.Result.NotDrawn && match.matchResult != Match.Result.NotPlayed)
                {
                    For += match.homeScore.score + match.homeScore.scoreAET;
                    Against += match.awayScore.score + match.awayScore.scoreAET;
                    Played++;
                }
            }
            else if (match.AwayTeam.TeamIndex == team.TeamIndex)
            {
                if (match.matchResult == Match.Result.AwayWin)
                {
                    Points += 3;
                    Won++;
                }
                if (match.matchResult == Match.Result.Draw)
                {
                    Points += 1;
                    Drawn++;
                }
                if (match.matchResult == Match.Result.HomeWin)
                {
                    Lost++;
                }
                if (match.matchResult != Match.Result.NotDrawn && match.matchResult != Match.Result.NotPlayed)
                {
                    For += match.awayScore.score + match.awayScore.scoreAET;
                    Against += match.homeScore.score + match.homeScore.scoreAET;
                    Played++;
                }
            }
        }

        public void UndoMatchUpdates(Match match)
        {
            if (match.HomeTeam.TeamIndex == team.TeamIndex)
            {
                if (match.matchResult == Match.Result.HomeWin)
                {
                    Points -= 3;
                    Won--;
                }
                if (match.matchResult == Match.Result.Draw)
                {
                    Points -= 1;
                    Drawn--;
                }
                if (match.matchResult == Match.Result.AwayWin)
                {
                    Lost--;
                }
                For -= match.homeScore.score + match.homeScore.scoreAET;
                Against -= match.awayScore.score + match.awayScore.scoreAET;
                Played--;
            }
            else if (match.AwayTeam.TeamIndex == team.TeamIndex)
            {
                if (match.matchResult == Match.Result.AwayWin)
                {
                    Points -= 3;
                    Won--;
                }
                if (match.matchResult == Match.Result.Draw)
                {
                    Points -= 1;
                    Drawn--;
                }
                if (match.matchResult == Match.Result.HomeWin)
                {
                    Lost--;
                }
                For -= match.awayScore.score + match.awayScore.scoreAET;
                Against -= match.homeScore.score + match.homeScore.scoreAET;
                Played--;
            }

        }

        public int CompareTo(object other)
        {
            int returnValue;
            GroupTeam compareTeam = (GroupTeam)other;

            returnValue = this.Points - compareTeam.Points;
            if (returnValue == 0)
            {
                returnValue = this.GD - compareTeam.GD;
                if (returnValue == 0)
                {
                    returnValue = this.For - compareTeam.For;
                    if (returnValue == 0)
                    {
                        //Compare on head to head here.
                        if (returnValue == 0)
                        {
                            returnValue = compareTeam.team.TeamIndex - this.team.TeamIndex;
                            if (returnValue == 0)
                            {
                                returnValue = 1;
                            }
                        }
                    }
                }
            }

            return returnValue;
        }
    }
}
