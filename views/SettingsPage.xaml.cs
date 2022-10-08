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
using System.Xml;

namespace pilots_brothers_safe.views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private int settingsGridSize;
        private XmlDocument settingsDocument;
        public SettingsPage(int currentGridSize)
        {
            InitializeComponent();
            InitializeSliderValue(currentGridSize);
        }

        private void InitializeSliderValue(int currentGridSize)
        {
            SizeSlider.Value = currentGridSize;
        }

        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            settingsGridSize = (int)SizeSlider.Value;
            MainWindow.GameSettings.GridSize = settingsGridSize;

            settingsDocument = new XmlDocument();
            settingsDocument.Load("../../../cfg/Settings.xml");
            XmlNode? node = settingsDocument.SelectSingleNode("/Settings");
            node?.RemoveAll();

            XmlNode xmlGridSize = settingsDocument.CreateElement("GridSize");
            xmlGridSize.InnerText = settingsGridSize.ToString();
            node?.AppendChild(xmlGridSize);

            settingsDocument.Save("../../../cfg/Settings.xml");
            this.NavigationService.GoBack();
        }

        private void BtnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
