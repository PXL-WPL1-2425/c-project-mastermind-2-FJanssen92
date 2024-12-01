using Microsoft.VisualBasic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MastermindCSProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private class Attempt
        {
            public Brush ChosenColor1 { get; set; }
            public Brush ChosenColor2 { get; set; }
            public Brush ChosenColor3 { get; set; }
            public Brush ChosenColor4 { get; set; }
            public Brush Color1BorderBrush { get; set; }
            public Thickness Color1BorderThickness { get; set; }
            public Brush Color2BorderBrush { get; set; }
            public Thickness Color2BorderThickness { get; set; }
            public Brush Color3BorderBrush { get; set; }
            public Thickness Color3BorderThickness { get; set; }
            public Brush Color4BorderBrush { get; set; }
            public Thickness Color4BorderThickness { get; set; }
        }

        private List<Attempt> attemptsList;
        private string color1, color2, color3, color4;
        private int attempts = 1;
        private DispatcherTimer timer;
        private int startTime;
        private bool isGameOver = false;
        private int score = 100;
        private Color[] colors = { Colors.White, Colors.Red, Colors.Blue, Colors.Green, Colors.Yellow, Colors.Orange };
        private int[] colorIndex = { 0, 0, 0, 0 };


        public MainWindow()
        {
            InitializeComponent();
            attemptsList = new List<Attempt>();
            timer = new DispatcherTimer();
            RandomColors(out color1, out color2, out color3, out color4);
            secretCodeTextBox.Text = $"Kleur 1: {color1}, Kleur 2: {color2}, Kleur 3:{color3}, Kleur 4:{color4}";
            Title = $"Mastermind - Poging: {attempts}";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartCountdown();
        }

        /// <summary>
        /// Timer wordt geinitialiseerd en gestart.
        /// de start tijd wordt ingesteld op 10, zodat de timer vanaf 10 kan aftellen in de Timer_Tick methode.
        /// </summary>
        private void StartCountdown()
        {
            startTime = 10;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick -= Timer_Tick;
            timer.Tick += Timer_Tick;
            timer.Start();
        }


        /// <summary>
        /// De timer telt af van 10 naar 0, zolang de starttijd groter is dan 0.
        /// Zoniet, wordt de StopTimer methode aangeroepen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (startTime >= 0)
            {
                timerLabel.Content = "Timer: " + startTime.ToString();
                startTime--;
            }
            else
            {
                StopTimer();
            }
        }


        /// <summary>
        /// De timer wordt gestopt en het aantal pogingen wordt verhoogd en in de titel weergegeven.
        /// Daarna wordt de StartCountdown methode weer opgeroepen zodat er een nieuwe beurt begint.
        /// </summary>
        private void StopTimer()
        {
            timer.Stop();
            if (attempts <= 10 && !isGameOver)
            {
                attempts++;
                Title = $"Mastermind - Poging: {attempts}";
                timerLabel.Content = "Tijd is op! Beurt verloren!";
                StartCountdown();
            }
            
            else
            {
                isGameOver = true;
                MessageBox.Show("Game Over! De correct code was: " + color1 + " " + color2 + " " + color3 + " " + color4);
                MessageBoxResult result =  MessageBox.Show("Wil je nog een keer spelen?", "Einde Spel!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    this.Close();
                }
                else
                {
                    ResetGame();
                }
            }
        }

        private void ResetGame()
        {
            attempts = 1;
            RandomColors(out color1, out color2, out color3, out color4);
            secretCodeTextBox.Text = $"Kleur 1: {color1}, Kleur 2: {color2}, Kleur 3:{color3}, Kleur 4:{color4}";
            Title = $"Mastermind - Poging: {attempts}";
            timerLabel.Content = "Timer: 10";

            color1Ellipse.Fill = Brushes.White;
            color2Ellipse.Fill = Brushes.White;
            color3Ellipse.Fill = Brushes.White;
            color4Ellipse.Fill = Brushes.White;

            color1Border.BorderBrush = Brushes.Black;
            color1Border.BorderThickness = new Thickness(1);
            color2Border.BorderBrush = Brushes.Black;
            color2Border.BorderThickness = new Thickness(1);
            color3Border.BorderBrush = Brushes.Black;
            color3Border.BorderThickness = new Thickness(1);
            color4Border.BorderBrush = Brushes.Black;
            color4Border.BorderThickness = new Thickness(1);

            StartCountdown();
        }

        /// <summary>
        /// Methode die laat zien wat de geheime code is als de speler CTRL + F12 indrukt.
        /// Als de toetscombinatie daarna weer wordt ingedrukt, verdwijnt de textbox weer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleDebug(object sender, KeyEventArgs e)
        {
            //Als CTRL + F12 wordt ingedrukt EN de texbox is verborgen, laat deze dan zien.
            if ((e.Key == Key.F12 && e.KeyboardDevice.Modifiers == ModifierKeys.Control) & secretCodeTextBox.Visibility == Visibility.Hidden)
            {
                secretCodeTextBox.Visibility = Visibility.Visible;
            }

            //Hetzelfde gebeurd hier, maar omgekeerd. Als de textbox zichtbaar is, wordt deze verborgen.
            else if ((e.Key == Key.F12 && e.KeyboardDevice.Modifiers == ModifierKeys.Control) & secretCodeTextBox.Visibility == Visibility.Visible)
            {
                secretCodeTextBox.Visibility= Visibility.Hidden;
            }
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            int index = 0;

            if (ellipse == color1Ellipse) index = 0;
            else if (ellipse == color2Ellipse) index = 1;
            else if (ellipse == color3Ellipse) index = 2;
            else if (ellipse == color4Ellipse) index = 3;

            colorIndex[index] = (colorIndex[index] + 1) % colors.Length;
            ellipse.Fill = new SolidColorBrush(colors[colorIndex[index]]);
        }

        public void RandomColors(out string color1, out string color2, out string color3, out string color4)
        {
            Random newColor = new Random();
            List<string> colorList = new List<string>();
            string randomColor;

            for (int i = 1; i <= 4; i++)
            {
                randomColor = (newColor.Next(1, 7)).ToString();
                switch (randomColor)
                {
                    case "1":
                        randomColor = "White";
                        break;
                    case "2":
                        randomColor = "Red";
                        break;
                    case "3":
                        randomColor = "Blue";
                        break;
                    case "4":
                        randomColor = "Green";
                        break;
                    case "5":
                        randomColor = "Yellow";
                        break;
                    case "6":
                        randomColor = "Orange";
                        break;
                }

                colorList.Add(randomColor);
            }

            color1 = colorList[0];
            color2 = colorList[1];
            color3 = colorList[2];
            color4 = colorList[3];

        }

      private string GetColorName(Color color)
        {
            if (color == Colors.White) return "White";
            if (color == Colors.Red) return "Red";
            if (color == Colors.Blue) return "Blue";
            if (color == Colors.Green) return "Green";
            if (color == Colors.Yellow) return "Yellow";
            if (color == Colors.Orange) return "Orange";
            return "";
        }

        private void checkCodeButton_Click(object sender, RoutedEventArgs e)
        {

            Brush chosenColor1, chosenColor2, chosenColor3, chosenColor4;
            chosenColor1 = color1Ellipse.Fill;
            chosenColor2 = color2Ellipse.Fill;
            chosenColor3 = color3Ellipse.Fill;
            chosenColor4 = color4Ellipse.Fill;

            List<Brush> generatedColors = new List<Brush> 
            { new SolidColorBrush((Color)ColorConverter.ConvertFromString(color1)), 
              new SolidColorBrush((Color)ColorConverter.ConvertFromString(color2)),
              new SolidColorBrush((Color)ColorConverter.ConvertFromString(color3)),
              new SolidColorBrush((Color)ColorConverter.ConvertFromString(color4))
            };
            List<Brush> chosenColors = new List<Brush> {chosenColor1, chosenColor2, chosenColor3, chosenColor4};

            var attempt = new Attempt
            {
                ChosenColor1 = chosenColor1,
                ChosenColor2 = chosenColor2,
                ChosenColor3 = chosenColor3,
                ChosenColor4 = chosenColor4,
            };

            for (int i = 0; i < 4; i++) 
            {
                Border targetBorder = null;

                switch (i)
                {
                    case 0: targetBorder = color1Border;
                        break;
                    case 1:
                        targetBorder = color2Border;
                        break;
                    case 2:
                        targetBorder = color3Border;
                        break;
                    case 3:
                        targetBorder = color4Border;
                        break;
                }

                if (targetBorder != null)
                {
                    if (((SolidColorBrush)chosenColors[i]).Color == ((SolidColorBrush)generatedColors[i]).Color)
                    {
                        targetBorder.BorderBrush = Brushes.DarkRed;
                        targetBorder.BorderThickness = new Thickness(5);
                    }
                    else if (generatedColors.Any(gc => ((SolidColorBrush)gc).Color == ((SolidColorBrush)chosenColors[i]).Color))
                    {
                        targetBorder.BorderBrush = Brushes.Wheat;
                        targetBorder.BorderThickness = new Thickness(5);
                        score -= 1;
                    }
                    else
                    {
                        targetBorder.BorderBrush = Brushes.Transparent;
                        targetBorder.BorderThickness = new Thickness(5);
                        score -= 2;
                    }

                    // Border informatie opslaan in de attempt
                    switch (i)
                    {
                        case 0:
                            attempt.Color1BorderBrush = targetBorder.BorderBrush;
                            attempt.Color1BorderThickness = targetBorder.BorderThickness;
                            break;
                        case 1:
                            attempt.Color2BorderBrush = targetBorder.BorderBrush;
                            attempt.Color2BorderThickness = targetBorder.BorderThickness;
                            break;
                        case 2:
                            attempt.Color3BorderBrush = targetBorder.BorderBrush;
                            attempt.Color3BorderThickness = targetBorder.BorderThickness;
                            break;
                        case 3:
                            attempt.Color4BorderBrush = targetBorder.BorderBrush;
                            attempt.Color4BorderThickness = targetBorder.BorderThickness;
                            break;
                    }
                }
            }

            attemptsList.Add(attempt);
            attemptsListBox.ItemsSource = null;
            attemptsListBox.ItemsSource = attemptsList;

            if (attempts <= 10)
            {
                attempts++;
                this.Title = $"Mastermind - Poging: {attempts}";
                StartCountdown();
            }
            else
            {
                StopTimer();
            }

            scoreLabel.Content = "Score: " + score;

        }
    }
}