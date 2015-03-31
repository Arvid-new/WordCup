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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GroupLabel[] groups;
        GroupGamesGrid[] groupGames;
        TeamGoalsLabel[,] goalLabels;

        public MainWindow()
        {
            InitializeComponent();
            AddGroups();
            AddGroupGames();
            AddKnockoutStages();
            AddFinals();
            AddGoalPredictions();

            TournamentManager.MatchScoresUpdated += TournamentManager_MatchScoresUpdated;
        }

        void TournamentManager_MatchScoresUpdated(object sender, EventArgs e)
        {
            UpdateScorePredictions();
        }

        private void UpdateScorePredictions()
        {
            foreach (Region region in (Region[])Enum.GetValues(typeof(Region)))
            {
                goalLabels[(int)region, 0].SetTeam(TournamentManager.Prediction.OptimumSelections[region]);
                goalLabels[(int)region, 1].SetTeam(TournamentManager.Prediction.DadSelections[region]);
                goalLabels[(int)region, 2].SetTeam(TournamentManager.Prediction.AlexSelections[region]);
            }

            CurrentGoals.Content = Convert.ToString(TournamentManager.Prediction.GetTotalGoals(TournamentManager.Prediction.OptimumSelections));
            DadGoals.Content = Convert.ToString(TournamentManager.Prediction.GetTotalGoals(TournamentManager.Prediction.DadSelections));
            AlexGoals.Content = Convert.ToString(TournamentManager.Prediction.GetTotalGoals(TournamentManager.Prediction.AlexSelections));
            PredictedGoals.Content = Convert.ToString(TournamentManager.Prediction.GetTournamentGoals());
            DadPredictedGoals.Content = Properties.Resources.DadGoals;
            AlexPredictedGoals.Content = Properties.Resources.AlexGoals;
        }

        private void AddGoalPredictions()
        {
            goalLabels = new TeamGoalsLabel[7, 3];

            foreach (Region region in (Region[])Enum.GetValues(typeof(Region)))
            {
                goalLabels[(int)region, 0] = new TeamGoalsLabel(TournamentManager.Prediction.OptimumSelections[region]);
                GoalsGrid.Children.Add(goalLabels[(int)region, 0]);
                Grid.SetRow(goalLabels[(int)region, 0], 1 + (int)region);
                Grid.SetColumn(goalLabels[(int)region, 0], 1);

                goalLabels[(int)region, 1] = new TeamGoalsLabel(TournamentManager.Prediction.DadSelections[region]);
                GoalsGrid.Children.Add(goalLabels[(int)region, 1]);
                Grid.SetRow(goalLabels[(int)region, 1], 1 + (int)region);
                Grid.SetColumn(goalLabels[(int)region, 1], 2);

                goalLabels[(int)region, 2] = new TeamGoalsLabel(TournamentManager.Prediction.AlexSelections[region]);
                GoalsGrid.Children.Add(goalLabels[(int)region, 2]);
                Grid.SetRow(goalLabels[(int)region, 2], 1 + (int)region);
                Grid.SetColumn(goalLabels[(int)region, 2], 3);
            }

            CurrentGoals.Content = Convert.ToString(TournamentManager.Prediction.GetTotalGoals(TournamentManager.Prediction.OptimumSelections));
            DadGoals.Content = Convert.ToString(TournamentManager.Prediction.GetTotalGoals(TournamentManager.Prediction.DadSelections));
            AlexGoals.Content = Convert.ToString(TournamentManager.Prediction.GetTotalGoals(TournamentManager.Prediction.AlexSelections));
            PredictedGoals.Content = Convert.ToString(TournamentManager.Prediction.GetTournamentGoals());
            DadPredictedGoals.Content = Properties.Resources.DadGoals;
            AlexPredictedGoals.Content = Properties.Resources.AlexGoals;
        }

        private void AddFinals()
        {
            FinalsMatch label = new FinalsMatch(TournamentManager.Final);
            FinalsGrid.Children.Add(label);
            Grid.SetRow(label, 0);
            Grid.SetColumn(label, 0);
            label.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            label = new FinalsMatch(TournamentManager.PlayOff);
            FinalsGrid.Children.Add(label);
            Grid.SetRow(label, 1);
            Grid.SetColumn(label, 0);
            label.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

        }

        private void AddKnockoutStages()
        {
            KnockoutMatchLeft matchLabelLeft;
            KnockoutMatchRight matchLabelRight;
            int matchIndex;
            for (matchIndex = 0; matchIndex < TournamentManager.NumberOfGroups / 2; matchIndex++)
            {
                matchLabelLeft = new KnockoutMatchLeft(TournamentManager.KnockoutStages.Matches[matchIndex]);
                matchLabelLeft.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                LastSixteenLeft.Children.Add(matchLabelLeft);
                Grid.SetRow(matchLabelLeft, matchIndex % (TournamentManager.NumberOfGroups/2));
            }
            for (matchIndex = TournamentManager.NumberOfGroups; matchIndex <  5 * TournamentManager.NumberOfGroups / 4; matchIndex++)
            {
                matchLabelLeft = new KnockoutMatchLeft(TournamentManager.KnockoutStages.Matches[matchIndex]);
                matchLabelLeft.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                LastEightLeft.Children.Add(matchLabelLeft);
                Grid.SetRow(matchLabelLeft, matchIndex % (TournamentManager.NumberOfGroups / 4));
            }
            for (matchIndex = 3 * TournamentManager.NumberOfGroups / 2; matchIndex <  13 * TournamentManager.NumberOfGroups / 8; matchIndex++)
            {
                matchLabelLeft = new KnockoutMatchLeft(TournamentManager.KnockoutStages.Matches[matchIndex]);
                matchLabelLeft.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                LastFourLeft.Children.Add(matchLabelLeft);
                Grid.SetRow(matchLabelLeft, matchIndex % (TournamentManager.NumberOfGroups / 8));
            }

            for (matchIndex = TournamentManager.NumberOfGroups / 2; matchIndex < TournamentManager.NumberOfGroups; matchIndex++)
            {
                matchLabelRight = new KnockoutMatchRight(TournamentManager.KnockoutStages.Matches[matchIndex]);
                matchLabelRight.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                LastSixteenRight.Children.Add(matchLabelRight);
                Grid.SetRow(matchLabelRight, matchIndex % (TournamentManager.NumberOfGroups/2));
            }
            for (matchIndex = 5 * TournamentManager.NumberOfGroups / 4; matchIndex < 3 * TournamentManager.NumberOfGroups / 2; matchIndex++)
            {
                matchLabelRight = new KnockoutMatchRight(TournamentManager.KnockoutStages.Matches[matchIndex]);
                matchLabelRight.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                LastEightRight.Children.Add(matchLabelRight);
                Grid.SetRow(matchLabelRight, matchIndex % (TournamentManager.NumberOfGroups / 4));
            }
            for (matchIndex = 13 * TournamentManager.NumberOfGroups / 8; matchIndex < 7 * TournamentManager.NumberOfGroups / 4; matchIndex++)
            {
                matchLabelRight = new KnockoutMatchRight(TournamentManager.KnockoutStages.Matches[matchIndex]);
                matchLabelRight.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                LastFourRight.Children.Add(matchLabelRight);
                Grid.SetRow(matchLabelRight, matchIndex % (TournamentManager.NumberOfGroups / 8));
            }
        }

        void AddGroups()
        {
            groups = new GroupLabel[TournamentManager.NumberOfGroups];

            for (int groupIndex = 0; groupIndex < TournamentManager.NumberOfGroups; groupIndex++)
            {
                groups[groupIndex] = new GroupLabel(TournamentManager.Groups[groupIndex]);

                GroupDisplay.Children.Add(groups[groupIndex]);
                Grid.SetRow(groups[groupIndex], groupIndex >> 1);
                Grid.SetColumn(groups[groupIndex], (groupIndex & 1));
            }
        }

        void AddGroupGames()
        {
            groupGames = new GroupGamesGrid[TournamentManager.NumberOfGroups];

            Label label;
            for (int groupIndex = 0; groupIndex < TournamentManager.NumberOfGroups; groupIndex++)
            {
                label = new Label() { Content = (char)((int)'A' + groupIndex), VerticalAlignment = VerticalAlignment.Center};
                GroupGamesGrids.Children.Add(label);
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, groupIndex);

                groupGames[groupIndex] = new GroupGamesGrid(TournamentManager.Groups[groupIndex]);
                GroupGamesGrids.Children.Add(groupGames[groupIndex]);
                Grid.SetColumn(groupGames[groupIndex], 1);
                Grid.SetRow(groupGames[groupIndex], groupIndex);
            }
        }
    }
}
