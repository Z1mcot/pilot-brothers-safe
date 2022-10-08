using System;
using System.Collections.Generic;
using System.IO;
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

namespace pilots_brothers_safe.views
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void BtnGame_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PlayPage(MainWindow.GameSettings.GridSize));
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SettingsPage(MainWindow.GameSettings.GridSize));
        }

        private void BtnTutorial_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TutorialPage());
        }
    }
}
