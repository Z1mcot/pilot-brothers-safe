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
using System.Xml.Serialization;
using pilots_brothers_safe.cfg;

namespace pilots_brothers_safe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Settings GameSettings { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            InitSettings();
            MainFrame.Navigate(new views.MainMenuPage());
        }

        private void InitSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (Stream reader = new FileStream("../../../cfg/settings.xml", FileMode.Open))
            {
                GameSettings = (Settings)serializer.Deserialize(reader);
            }
        }
    }
}
