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
    /// Interaction logic for GroupGamesGrid.xaml
    /// </summary>
    public partial class GroupGamesGrid : UserControl
    {
        GroupMatch[] MatchLabels;
        Group Group;

        public GroupGamesGrid(Group group)
        {
            this.Group = group;
            InitializeComponent();
            AddMatches();
        }

        private void AddMatches()
        {
            MatchLabels = new GroupMatch[Group.Matches.Length];

            for (int matchLabelIndex = 0; matchLabelIndex < Group.Matches.Length; matchLabelIndex++)
            {
                MatchLabels[matchLabelIndex] = new GroupMatch(Group.Matches[matchLabelIndex])
                    {
                        Margin = new Thickness(5)
                    };
                Games.Children.Add(MatchLabels[matchLabelIndex]);
                Grid.SetRow(MatchLabels[matchLabelIndex], (matchLabelIndex & 1));
                Grid.SetColumn(MatchLabels[matchLabelIndex], matchLabelIndex >> 1);
            }
        }

    }
}
