using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldCup
{
    public class Group
    {
        public int GroupIndex { get; set; }

        public GroupTeam[] Teams { get; private set; }
        public Match[] Matches { get; private set; }

        public bool[] PositionsSecure { get; private set; }
        bool groupComplete;

        public Group(int groupIndex)
        {
            GroupIndex = groupIndex;
            SetTeams();
            SetMatches();
        }

        private void SetTeams()
        {
            Teams = new GroupTeam[TournamentManager.GroupSize];
            PositionsSecure = new bool[TournamentManager.GroupSize];

            int teamIndexOffset = GroupIndex * TournamentManager.GroupSize;
            for (int teamIndex = 0; teamIndex < TournamentManager.GroupSize; teamIndex++)
            {
                Teams[teamIndex] = new GroupTeam(TournamentManager.Teams[teamIndex + teamIndexOffset]);
                PositionsSecure[teamIndex] = false;
            }
        }

        private void SetMatches()
        {
            Matches = new Match[Functions.Factorial(TournamentManager.GroupSize - 1)];

            int matchIndexOffset;
            for (int matchIndex = 0; matchIndex < Matches.Length; matchIndex++)
            {
                matchIndexOffset = ((TournamentManager.NumberOfGroups * TournamentManager.GroupSize / 2) * (matchIndex >> 1)) + (GroupIndex * TournamentManager.GroupSize / 2);
                Matches[matchIndex] = new Match((matchIndex & 1) + matchIndexOffset);
                SetMatchTeams(ref Matches[matchIndex], matchIndex);
                Matches[matchIndex].ScoreChanged += Group_ScoreChanged;
                Matches[matchIndex].UpdatingMatch += Group_ScoreUpdated;
            }
            for (int matchIndex = 0; matchIndex < Matches.Length; matchIndex++)
            {
                UpdateResultsOnMatchLoaded(Matches[matchIndex]);
            }
        }

        void UpdateResultsOnMatchLoaded(Match match)
        {
            if (match.matchResult != Match.Result.NotPlayed && match.matchResult != Match.Result.NotDrawn)
            {
                foreach (GroupTeam team in Teams)
                {
                    team.UpdateParameters(match);
                }
                RecalculateGroup();
            }
        }

        void Group_ScoreUpdated(object sender, int e)
        {
            int thisMatchIndex = ((e / 16) * 2) + (e & 1);
            foreach (GroupTeam team in Teams)
            {
                team.UndoMatchUpdates(Matches[thisMatchIndex]);
            }
        }

        void Group_ScoreChanged(object sender, int e)
        {
            int thisMatchIndex = ((e/16) * 2) + (e & 1); 
            foreach (GroupTeam team in Teams)
            {
                team.UpdateParameters(Matches[thisMatchIndex]);
            }
            RecalculateGroup();
        }

        public event EventHandler RepopulateScores;
        public event EventHandler<int> GroupOrderUpdated;

        private void RecalculateGroup()
        {
            groupComplete = IsGroupComplete();
            SortGroup();
            EvaluatePositions();
            SetInTournament();
            if (RepopulateScores != null)
                RepopulateScores(this, new EventArgs());
            if (GroupOrderUpdated != null)
                GroupOrderUpdated(this, GroupIndex);
        }

        private void SetInTournament()
        {
            Teams[0].team.InTournament = true;
            Teams[1].team.InTournament = true;
            Teams[2].team.InTournament = !(PositionsSecure[2]);
            Teams[3].team.InTournament = !(PositionsSecure[3]);
        }

        private bool IsGroupComplete()
        {
            bool complete = true;
            foreach (GroupTeam team in Teams)
            {
                if (team.Played != 3)
                    complete = false;
            }
            return complete;
        }

        private void EvaluatePositions()
        {
            bool lastPositionSecure = true;
            int pointsAvailable = 0;
            int mostPointsAvailable;
            bool canMoveUp;
            bool canMoveDown;
            for (int teamIndexBeingAssessed = 0; teamIndexBeingAssessed < TournamentManager.GroupSize; teamIndexBeingAssessed++)
            {
                canMoveUp = true;
                canMoveDown = true;
                mostPointsAvailable = 0;

                for (int teamBelow = teamIndexBeingAssessed + 1; teamBelow < TournamentManager.GroupSize; teamBelow++)
                {
                    pointsAvailable = Teams[teamBelow].Points + 3 * (((TournamentManager.GroupStageMatches * 2) / TournamentManager.NumberOfTeams) - Teams[teamBelow].Played);
                    if (pointsAvailable > mostPointsAvailable)
                        mostPointsAvailable = pointsAvailable;
                }

                if (lastPositionSecure)
                {
                    canMoveUp = false;
                }
                if (Teams[teamIndexBeingAssessed].Points - mostPointsAvailable > 0 || teamIndexBeingAssessed == TournamentManager.GroupSize - 1)
                {
                    canMoveDown = false;
                }

                lastPositionSecure = !canMoveDown;
                PositionsSecure[teamIndexBeingAssessed] = (!canMoveUp && !canMoveDown) || groupComplete;
            }
        }

        private void SortGroup()
        {
            IComparable[] array = Teams;
            Functions.Sort(ref array, 0, Teams.Length - 1);
            Teams = (GroupTeam[])array;
        }

        private void SetMatchTeams(ref Match match, int matchIndex)
        {
            switch (matchIndex)
            {
                case 0:
                    match.SetHomeTeam(Teams[0].team);
                    match.SetAwayTeam(Teams[1].team);
                    break;
                case 1:
                    match.SetHomeTeam(Teams[2].team);
                    match.SetAwayTeam(Teams[3].team);
                    break;
                case 2:
                    match.SetHomeTeam(Teams[2].team);
                    match.SetAwayTeam(Teams[0].team);
                    break;
                case 3:
                    match.SetHomeTeam(Teams[3].team);
                    match.SetAwayTeam(Teams[1].team);
                    break;
                case 4:
                    match.SetHomeTeam(Teams[0].team);
                    match.SetAwayTeam(Teams[3].team);
                    break;
                case 5:
                    match.SetHomeTeam(Teams[1].team);
                    match.SetAwayTeam(Teams[2].team);
                    break;
            }
        }
    }
}
