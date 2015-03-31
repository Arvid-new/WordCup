using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCup
{
    static class TournamentManager
    {
        public const int NumberOfTeams = 32;
        public const int GroupSize = 4;
        public const int GroupWinners = 2;

        public static int GroupStageMatches;
        public static int TotalMatches;
        public static int NumberOfGroups;
        public static int MatchesCompleted;

        public static Team[] Teams;
        public static Group[] Groups;
        /// <summary>
        /// Format: Date,Time,Location,Channel,Result (int),HomeScore,AwayScore
        /// </summary>
        public static string[] MatchData;

        public static GoalsPrediction Prediction;
        public static KnockoutRound KnockoutStages;
        public static Match PlayOff;
        public static Match Final;

        public static event EventHandler MatchScoresUpdated;

        public static void SetMatchNumbers()
        {
            NumberOfGroups = NumberOfTeams / GroupSize;

            GroupStageMatches = Functions.Factorial(GroupSize - 1);
            GroupStageMatches *= NumberOfGroups;

            int teamsInKnockout = NumberOfGroups * GroupWinners;
            TotalMatches = GroupStageMatches + teamsInKnockout;
        }
        public static void LoadMatchData()
        {
            MatchData = new string[TotalMatches];
            using (StreamReader sr = new StreamReader("Matches.txt"))
            {
                for (int matchIndex = 0; matchIndex < TotalMatches; matchIndex++)
                {
                    MatchData[matchIndex] = sr.ReadLine();
                }
            }
        }
        public static void SetTeams()
        {
            Teams = new Team[NumberOfTeams];
            StreamReader sr = new StreamReader("Teams.txt");

            string[] lineParts;
            for (int teamIndex = 0; teamIndex < NumberOfTeams; teamIndex++)
            {
                lineParts = sr.ReadLine().Split(',');
                Teams[teamIndex] = new Team(teamIndex, lineParts[0], Convert.ToInt32(lineParts[1]));
            }
            sr.Dispose();
        }
        private static void SetGroups()
        {
            Groups = new Group[NumberOfGroups];
            for (int groupIndex = 0; groupIndex < NumberOfGroups; groupIndex++)
            {
                Groups[groupIndex] = new Group(groupIndex);
                Groups[groupIndex].GroupOrderUpdated += SetKnockoutRounds;
            }
            for (int groupIndex = 0; groupIndex < NumberOfGroups; groupIndex++)
            {
                SetKnockoutRounds(null, groupIndex);
            }
        }
        private static void SetKnockoutRounds(object sender, int e)
        {
            int matchIndex;
            for (int groupPosition = 0; groupPosition < GroupWinners; groupPosition++)
            {
                if ((groupPosition & 1) == (e & 1))
                {
                    matchIndex = (e / 2);
                }
                else
                {
                    matchIndex = (e / 2) + (NumberOfGroups / 2);
                }

                if (groupPosition == 0)
                {
                    KnockoutStages.Matches[matchIndex].SetHomeTeam(Groups[e].Teams[groupPosition].team);
                }
                else
                {
                    KnockoutStages.Matches[matchIndex].SetAwayTeam(Groups[e].Teams[groupPosition].team);
                }
            }
        }

        public static void ResetMatches()
        {
            string[] fileData = new string[TotalMatches];
            int lineIndex = 0;
            StreamReader sr = new StreamReader("Matches.txt");
            do
            {
                fileData[lineIndex++] = sr.ReadLine();
            } while (!sr.EndOfStream);
            sr.Dispose();

            for (lineIndex = 0; lineIndex < fileData.Length; lineIndex++)
            {
                if (fileData[lineIndex].Split(',').Length == 7)
                {
                    fileData[lineIndex] = fileData[lineIndex].Substring(0, fileData[lineIndex].Length - 13);
                    fileData[lineIndex] += "0";
                }
                if (fileData[lineIndex].Split(',').Length == 5)
                {
                    fileData[lineIndex] = fileData[lineIndex].Substring(0, fileData[lineIndex].Length - 1);
                    fileData[lineIndex] += "0";
                }
            }

            StreamWriter sw = new StreamWriter("Matches.txt");
            for (lineIndex = 0; lineIndex < fileData.Length; lineIndex++)
            {
                sw.WriteLine(fileData[lineIndex]);
            }
            sw.Dispose();
        }

        public static void MatchUpdated()
        {
            if (Prediction != null)
                Prediction.SetOptimumSelections();
            if (MatchScoresUpdated != null)
                MatchScoresUpdated(null, new EventArgs());
        }

        [System.STAThreadAttribute()]
        public static void Main()
        {
            WorldCup.App app = new App();
            MatchesCompleted = 0;
            SetMatchNumbers();
            //ResetMatches();
            LoadMatchData();
            SetupFinals();
            app.Exit += app_Exit;
            SetTeams();
            SetGroups();
            Prediction = new GoalsPrediction(Properties.Resources.DadPredictions.Split(','), Properties.Resources.AlexPredictions.Split(','));
            var mainWindow = new MainWindow();
            app.Run(mainWindow);
        }

        private static void SetupFinals()
        {
            PlayOff = new Match(TotalMatches - 2);
            Final = new Match(TotalMatches - 1);
            KnockoutStages = new KnockoutRound(NumberOfGroups);
        }

        static void app_Exit(object sender, System.Windows.ExitEventArgs e)
        {
            using (StreamWriter sr = new StreamWriter("Matches.txt"))
            {
                for (int matchIndex = 0; matchIndex < TotalMatches; matchIndex++)
                {
                    sr.WriteLine(MatchData[matchIndex]);
                }
            }
        }

        internal static void ShowTeamWindow(Team team)
        {
            var teamWindow = new TeamWindow(team);
            teamWindow.Show();
        }
        internal static void SetFinalsData(Match match)
        {
            if (match.matchIndex == TotalMatches - 4)
            {
                switch (match.matchResult)
                {
                    case Match.Result.HomeWin:
                        Final.SetHomeTeam(match.HomeTeam);
                        PlayOff.SetHomeTeam(match.AwayTeam);
                        break;
                    case Match.Result.AwayWin:
                        Final.SetHomeTeam(match.AwayTeam);
                        PlayOff.SetHomeTeam(match.HomeTeam);
                        break;
                    case Match.Result.NotPlayed:
                        Final.ResetHomeTeam();
                        PlayOff.ResetHomeTeam();
                        break;
                }
            }
            else if (match.matchIndex == TotalMatches - 3)
            {
                switch (match.matchResult)
                {
                    case Match.Result.HomeWin:
                        Final.SetAwayTeam(match.HomeTeam);
                        PlayOff.SetAwayTeam(match.AwayTeam);
                        break;
                    case Match.Result.AwayWin:
                        Final.SetAwayTeam(match.AwayTeam);
                        PlayOff.SetAwayTeam(match.HomeTeam);
                        break;
                    case Match.Result.NotPlayed:
                        Final.ResetAwayTeam();
                        PlayOff.ResetAwayTeam();
                        break;
                }
            }
        }
    }
}
