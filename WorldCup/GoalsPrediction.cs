using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCup
{
    public enum Region
    {
        Favourites,
        Africa,
        Asia,
        WestEurope,
        EastEurope,
        NorthAmerica,
        SouthAmerica
    }

    class GoalsPrediction
    {
        public Dictionary<Region, Team> DadSelections { get; set; }
        public Dictionary<Region, Team> AlexSelections { get; set; }
        public Dictionary<Region, Team> OptimumSelections { get; set; }

        public GoalsPrediction(string[] DadPredictions, string[] AlexPredictions)
        {
            DadSelections = GetTeamsFromPredictions(DadPredictions);
            AlexSelections = GetTeamsFromPredictions(AlexPredictions);
            SetOptimumSelections();
        }

        public int GetTotalGoals(Dictionary<Region, Team> prediction)
        {
            int totalGoals = 0;

            foreach (KeyValuePair<Region, Team> team in prediction)
            {
                totalGoals += team.Value.GoalsScored();
            }

            return totalGoals;
        }
        public int GetTournamentGoals()
        {
            int totalGoals = 0;

            foreach (Team team in TournamentManager.Teams)
            {
                totalGoals += team.GoalsScored();
            }

            try
            {
                totalGoals = (int)Math.Round((totalGoals * ((float)TournamentManager.TotalMatches / (float)TournamentManager.MatchesCompleted)));
            }
            catch (DivideByZeroException)
            {
                totalGoals = 0;
            }

            return totalGoals;
        }

        public void SetOptimumSelections()
        {
            Dictionary<Region, int> highestHolders = new Dictionary<Region, int>();
            int currentHighestScore;
            Dictionary<Region, Team> highestTeams = new Dictionary<Region, Team>();

            for (int region = 0; region < 7; region++)
            {
                highestHolders[(Region)region] = -1;
                highestTeams[(Region)region] = new Team();
            }

            for (int teamIndex = 0; teamIndex < TournamentManager.Teams.Length; teamIndex++)
            {
                currentHighestScore = TournamentManager.Teams[teamIndex].GoalsScored();

                if (currentHighestScore > highestHolders[TournamentManager.Teams[teamIndex].TeamRegion])
                {
                    highestHolders[TournamentManager.Teams[teamIndex].TeamRegion] = currentHighestScore;
                    highestTeams[TournamentManager.Teams[teamIndex].TeamRegion] = TournamentManager.Teams[teamIndex];
                }
            }

            OptimumSelections = highestTeams;
        }
        private Dictionary<Region, Team> GetTeamsFromPredictions(string[] Predictions)
        {
            Dictionary<Region, Team> teamPredictions = new Dictionary<Region,Team>();

            for (int team = 0; team < 7; team++)
            {
                for (int teamIndex = 0; (teamIndex < TournamentManager.Teams.Length); teamIndex++)
                {
                    if (TournamentManager.Teams[teamIndex].TeamRegion == (Region)team)
                    {
                        if (TournamentManager.Teams[teamIndex].TeamName.Equals(Predictions[team]))
                        {
                            teamPredictions[(Region)team] = TournamentManager.Teams[teamIndex];
                        }
                    }
                }
            }

            return teamPredictions;
        }
    }
}
