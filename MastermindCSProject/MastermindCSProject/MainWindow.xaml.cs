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

        private string color1, color2, color3, color4;
        private int attempts = 1;
        private DispatcherTimer timer;
        private int startTime;
        private bool isGameOver = false;


        public MainWindow()
        {
            InitializeComponent();
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
            color1ComboBox.SelectedIndex = -1;
            color2ComboBox.SelectedIndex = -1;
            color3ComboBox.SelectedIndex = -1;
            color4ComboBox.SelectedIndex = -1;
            color1Label.Background = Brushes.Transparent;
            color2Label.Background = Brushes.Transparent;
            color3Label.Background = Brushes.Transparent;
            color4Label.Background = Brushes.Transparent;
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

        private void colorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (sender is ComboBox comboBox)
            {
                var selectedColor = comboBox.SelectedIndex switch
                {
                    0 => Brushes.White,
                    1 => Brushes.Red,
                    2 => Brushes.Blue,
                    3 => Brushes.Green,
                    4 => Brushes.Orange,
                    5 => Brushes.Yellow,
                    _ => Brushes.Transparent
                };

                if (comboBox == color1ComboBox) color1Label.Background = selectedColor;
                else if (comboBox == color2ComboBox) color2Label.Background = selectedColor;
                else if (comboBox == color3ComboBox) color3Label.Background = selectedColor;
                else if (comboBox == color4ComboBox) color4Label.Background = selectedColor;
            }
        }
            
        

        private void checkCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string chosenColor1, chosenColor2, chosenColor3, chosenColor4;
            chosenColor1 = color1ComboBox.Text;
            chosenColor2 = color2ComboBox.Text;
            chosenColor3 = color3ComboBox.Text;
            chosenColor4 = color4ComboBox.Text;

            List<string> generatedColors = new List<string> {color1, color2, color3, color4};
            List<string> chosenColors = new List<string> {chosenColor1, chosenColor2, chosenColor3, chosenColor4};

            for (int i = 0; i < 4; i++) 
            {
                Label targetLabel= null;

                switch (i)
                {
                    case 0: targetLabel = color1Label;
                        break;
                    case 1: targetLabel = color2Label;
                        break;
                    case 2: targetLabel = color3Label;
                        break;
                    case 3: targetLabel = color4Label;
                        break;
                }

                if (targetLabel != null)
                {
                    if (chosenColors[i] == generatedColors[i])
                    {
                        targetLabel.BorderBrush = Brushes.DarkRed;
                        targetLabel.BorderThickness = new Thickness(5);
                    }
                    else if (generatedColors.Contains(chosenColors[i]))
                    {
                        targetLabel.BorderBrush = Brushes.Wheat;
                        targetLabel.BorderThickness = new Thickness(5);
                    }
                    else
                    {
                        targetLabel.BorderBrush = Brushes.Transparent;
                        targetLabel.BorderThickness = new Thickness(5);
                    }
                }
            }

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
           
        }
    }
}