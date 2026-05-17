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



namespace MatchingGame
{   //只对当前命名空间有效
    using System.IO;
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed = 0;
        int matchesFound = 0;
        public MainWindow()
        {
         
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlocK.Text = (tenthsOfSecondsElapsed / 10.0).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = System.IO.Path.Combine(baseDirectory, "time_record.txt");
                string record = timeTextBlocK.Text + Environment.NewLine;
                string logContent = $"{DateTime.Now}-{record}" + Environment.NewLine;
                File.AppendAllText(filePath, logContent);
                MessageBox.Show("Game recode has been saved！");
                timeTextBlocK.Text = timeTextBlocK.Text + " - Click here to play again!";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🦜","🦜",
                "🦖","🦖",
                "🐲","🐲",
                "🐬","🐬",
                "🐋","🐋",
                "🦆","🦆",
                "🐈","🐈",
                "🦌","🦌",
            };
            Random random = new Random();
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {   if(timeTextBlocK.Name!= textBlock.Name)
                {   textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
                timer.Start();
                tenthsOfSecondsElapsed = 0;
                matchesFound = 0;
            }
        }
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock? textBlock = sender as TextBlock;
            if (findingMatch==false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (lastTextBlockClicked.Text==textBlock.Text)
            {   matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch=false;
            }
            else
            {   lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimerTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {  
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}