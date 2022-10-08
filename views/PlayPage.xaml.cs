using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pilots_brothers_safe.views
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class PlayPage : Page
    {
        private bool[,] _playField;
        private int valveWidth = 50;
        private int valveHeight = 50;
        private int vectorSize;
        private Stack<int> movesHistory;

        public PlayPage(int vectorSize)
        {
            InitializeComponent();
            movesHistory = new Stack<int>();
            this.vectorSize = vectorSize;
            StartNewGame();
        }

        private void StartNewGame()
        {
            _playField = CreatePlayField();
            CreatePlayGrid();
            ScrambleGameField();
        }

        private bool[,] CreatePlayField()
        {
            return new bool[vectorSize, vectorSize];
        }

        private void CreatePlayGrid()
        {
            PlayGrid.Columns = vectorSize;
            PlayGrid.Rows = vectorSize;
            PlayGrid.Width = valveWidth * (vectorSize + 1);
            PlayGrid.Height = valveHeight * (vectorSize + 1);
            PlayGrid.Margin = new Thickness(left: DoorPlayGround.Margin.Left + (528 - PlayGrid.Width) / 2, 
                                            top: DoorPlayGround.Margin.Top + (528 - PlayGrid.Height) / 2, 
                                            right: 0,
                                            bottom: 0);
            for (int x = 0; x < vectorSize*vectorSize; x++)
            {
                Image img = CreateValveImage();
                Button button = CreateButtonWithImage(img);
                PlayGrid.Children.Add(button);
            }
        }

        private Button CreateButtonWithImage(Image img)
        {
            var button = new Button();
            button.Content = img;
            button.Width = img.Width + 5;
            button.Height = img.Height + 5;
            button.Background = null;
            button.BorderBrush = null;
            // button.BorderThickness = new Thickness(1);

            button.Click += BtnValve_Click;
            return button;
        }

        private Image CreateValveImage()
        {
            BitmapImage biValve = new BitmapImage();
            biValve.BeginInit();
            biValve.UriSource = new Uri("/src/safe/Valve.png", UriKind.Relative);
            biValve.EndInit();
            
            Image valveImage = new Image();
            valveImage.Stretch = Stretch.Uniform;
            valveImage.Source = biValve;
            valveImage.Width = valveWidth;
            valveImage.Height = valveHeight;

            return valveImage;
        }

        private void ScrambleGameField()
        {
            var randomValve = new Random();
            for (int i = 0; i < 6; i++)
            {
                int randomValveIndex = randomValve.Next(0, _playField.Length);
                int row, column; (row, column) = ConvertIndexToCoordinates(randomValveIndex);
                RotateRowAndColumn(row, column, randomValveIndex);
            }
        }

        private (int, int) ConvertIndexToCoordinates(int number) => (number / vectorSize, number % vectorSize);
        private int ConvertCoordinatesToIndex(int row, int column) => row * vectorSize + column;

        private void UpdateMovesHistory(int clickedButtonIndex)
        {
            if (movesHistory.Count == 0 || movesHistory.Peek() != clickedButtonIndex) movesHistory.Push(clickedButtonIndex);
            else movesHistory.Pop();
        }
        
        private void RotateRowAndColumn(int row, int column, int clickedButtonIndex)
        {
            UpdateMovesHistory(clickedButtonIndex);

            _playField[row, column] ^= true;
            PlayGrid.Children[clickedButtonIndex].RenderTransform = GetValveRotation(clickedButtonIndex);

            for (int i = 0; i < vectorSize; i++)
            {
                int sameRowValve = ConvertCoordinatesToIndex(row, i);
                int sameColumnValve = ConvertCoordinatesToIndex(i, column);
                
                // Rotate same row
                _playField[row, i] ^= true;
                PlayGrid.Children[sameRowValve].RenderTransform = GetValveRotation(sameRowValve);
                // Rotate same column
                _playField[i, column] ^= true;
                PlayGrid.Children[sameColumnValve].RenderTransform = GetValveRotation(sameColumnValve);
            }
        }

        private RotateTransform GetValveRotation(int index)
        {
            Button valve = PlayGrid.Children[index] as Button;
            RotateTransform currentRotation = valve.RenderTransform as RotateTransform ?? new RotateTransform();
            currentRotation.Angle += 90;
            currentRotation.CenterX = valve.Width / 2;
            currentRotation.CenterY = valve.Height / 2;
            return currentRotation;
        }

        private void BtnValve_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = (Button)sender;
            int valveIndex = PlayGrid.Children.IndexOf(clickedButton);
            int row, column;
            (row, column) = ConvertIndexToCoordinates(valveIndex);
            RotateRowAndColumn(row, column, valveIndex);
            (PlayGrid.Children[valveIndex] as Button).BorderBrush = null;

            CheckForWinCondition();
        }

        private void CheckForWinCondition()
        {
            bool condition = _playField[0, 0];
            foreach (var item in _playField) if (condition != item) return;
            DoorPlayGround.Opacity = 0;
            OpenDoor.Opacity = 1;
            PlayGrid.Children.Clear();
            
            // this.NavigationService.Navigate(new WinPage());
        }

        private void BtnMainMenu_Click(object sender, RoutedEventArgs e) => this.NavigationService.Navigate(new MainMenuPage());

        private void BtnRestartGame_Click(object sender, RoutedEventArgs e) => this.NavigationService.Navigate(new PlayPage(vectorSize));

        private void BtnHint_Click(object sender, RoutedEventArgs e)
        {
            int valveIndex = movesHistory.Peek();
            HighlightCorrectValve(valveIndex);
        }

        private void HighlightCorrectValve(int valveIndex)
        {
            (PlayGrid.Children[valveIndex] as Button).BorderBrush = Brushes.Green;

            // TODO v2 Переделать под анимацию мигания (https://stackoverflow.com/questions/4954254/blinking-animation-wpf)
            DoubleAnimation valveHighlightAnimation = new DoubleAnimation();
            valveHighlightAnimation.Duration = TimeSpan.FromMilliseconds(200);
            valveHighlightAnimation.From = 1;
            valveHighlightAnimation.To = 0;
            valveHighlightAnimation.RepeatBehavior = new RepeatBehavior(2); // 3 раза мигает
            valveHighlightAnimation.AutoReverse = true;

            Storyboard.SetTarget(valveHighlightAnimation, (PlayGrid.Children[valveIndex] as Button));
            Storyboard.SetTargetProperty(
                valveHighlightAnimation, new PropertyPath(Border.OpacityProperty));

            Storyboard valvesStoryboard = new Storyboard();
            valvesStoryboard.Children.Add(valveHighlightAnimation);
            valvesStoryboard.Begin(this);
        }
    }
}
