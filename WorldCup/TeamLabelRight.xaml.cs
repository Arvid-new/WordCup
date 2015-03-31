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
    /// Interaction logic for TeamLabelRight.xaml
    /// </summary>
    public partial class TeamLabelRight : UserControl
    {
        Team team;

        public TeamLabelRight(Team team)
        {
            InitializeComponent();
            SetTeam(team);
        }

        void TeamLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TournamentManager.ShowTeamWindow(team);
        }

        void TeamLabel_MouseLeave(object sender, MouseEventArgs e)
        {
            ControlBackground.Background = new SolidColorBrush(Colors.Black) { Opacity = 0 };
        }

        void TeamLabel_MouseEnter(object sender, MouseEventArgs e)
        {
            ControlBackground.Background = new SolidColorBrush(Colors.Black) { Opacity = 0.2 };
        }

        public void SetTeam(Team team)
        {
            if (this.team != null)
            {
                MouseEnter -= TeamLabel_MouseEnter;
                MouseLeave -= TeamLabel_MouseLeave;
                MouseDown -= TeamLabel_MouseDown;
            }

            if (team != null)
            {
                this.team = team;
                Country.Content = team.TeamName;
                Flag.Source = team.TeamFlag;
                MouseEnter += TeamLabel_MouseEnter;
                MouseLeave += TeamLabel_MouseLeave;
                MouseDown += TeamLabel_MouseDown;
            }
        }
    }
}
