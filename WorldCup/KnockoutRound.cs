using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldCup
{
    class KnockoutRound
    {
        int firstRoundMatches;
        public Match[] Matches { get; private set; }

        public KnockoutRound(int matchesInFirstRound)
        {
            this.firstRoundMatches = matchesInFirstRound;
            SetFirstRoundMatches();
        }

        private void SetFirstRoundMatches()
        {
            Matches = new Match[(firstRoundMatches * 2) - 2];

            int matchOffset = TournamentManager.GroupStageMatches;
            for (int matchIndex = 0; matchIndex < Matches.Length; matchIndex++)
            {
                Matches[matchIndex] = new Match(matchIndex + matchOffset);
            }
        }
    }
}
