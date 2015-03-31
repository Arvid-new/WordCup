using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WorldCup
{
    public class Team
    {
        int teamIndex;
        public int TeamIndex
        {
            get { return teamIndex; }
            set { teamIndex = value; }
        }

        bool inTournament;
        public bool InTournament
        {
            get { return inTournament; }
            set { inTournament = value; }
        }
        public string TeamName { get; set; }
        public BitmapImage TeamFlag { get; set; }

        public Region TeamRegion { get; private set; }

        public List<Match> Matches { get; set; }
        public int GoalsScored()
        {
            int goalsScored = 0;
            foreach (Match match in Matches)
            {
                if (match.matchResult != Match.Result.NotDrawn && match.matchResult != Match.Result.NotPlayed)
                {
                    if (match.HomeTeam.teamIndex == teamIndex)
                    {
                        goalsScored += match.homeScore.score + match.homeScore.scoreAET;
                    }
                    if (match.AwayTeam.teamIndex == teamIndex)
                    {
                        goalsScored += match.awayScore.score + match.awayScore.scoreAET;
                    }
                }
            }

            return goalsScored;
        }

        public Team(int teamIndex, string teamName, int region)
        {
            TeamIndex = teamIndex;
            inTournament = true;
            TeamRegion = (Region)region;
            SetTeamProperties(teamName);
            Matches = new List<Match>();
        }

        public Team() { }

        void SetTeamProperties(string teamName)
        {
            TeamName = teamName;
            var path = Path.Combine(Environment.CurrentDirectory, "Images", teamName + ".png");
            if (File.Exists(path))
                TeamFlag = new BitmapImage(new Uri(path, UriKind.Absolute));
            else
                TeamFlag = new BitmapImage();
        }

    }
}
