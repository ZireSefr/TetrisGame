using System.Media;
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
using TetrisGame.BLL;

namespace TetrisGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameManager gameManager;
        private DispatcherTimer gameTimer;
        private SoundPlayer backgroundMusicPlayer;
        private Thread musicThread;

        private bool isGamePause = false;
        private bool isMusicPlaying = false;



        public MainWindow()
        {
            InitializeComponent();

            //Initialize the SoundPlayer
            backgroundMusicPlayer = new SoundPlayer("Sounds/Robots Outro.wav");
            musicThread= new Thread(new ThreadStart(PlayMusic));
            musicThread.IsBackground = true;

            gameManager = new GameManager();

            gameManager.GameOver += OnGameOver;
            gameManager.scoreManager.ScoreUpdate += OnUpdateScore;
            gameManager.scoreManager.LevelUpdate += OnUpdateLevel;


            SetupGameTimer();

            InitializeGameGrid();
            InitializeGame();
        }

        private void PlayMusic()
        {
            /*while(isMusicPlaying)
            {
                if(!backgroundMusicPlayer.IsLoadCompleted)
                {
                    backgroundMusicPlayer.Load();
                }
                backgroundMusicPlayer.PlayLooping();
            }*/
            backgroundMusicPlayer.PlayLooping();
        }

        private void OnUpdateScore(int newScore)
        {
            UpdateScore(newScore);
        }

        private void OnUpdateLevel(int newLevel)
        {
            UpdateLevel(newLevel);
            UpdateSpeed(gameManager.scoreManager.SuggestSpeed());
        }

        private void SetupGameTimer()
        {
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(200);
            gameTimer.Tick += GameTick;
        }


        private void GameTick(object sender, EventArgs e)
        {
            //It is called at every tick of timer
            //Move the block down and update the UI
            gameManager.MoveBlockDown();
            UpdateUI();
        }

        private void UpdateUI()
        {
            ClearGameGrid();

            DrawNextBlock();

            var board = gameManager.GetBoardState();

            for (int row = 0; row < board.GetLength(0); row++)
            {

                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col])
                    {

                        DrawBlock(row, col, gameManager.GetCurrentBlockColor());
                    }
                }
            }

        }

        private void DrawBlock(int row, int col, string colorString)
        {
            //Convert the color string to a Color
            var color = (Color)ColorConverter.ConvertFromString(colorString);

            double cellWidth=GameGrid.ActualWidth/GameGrid.ColumnDefinitions.Count;
            double cellHeight = GameGrid.ActualHeight / GameGrid.RowDefinitions.Count;



            //Create a new rectangle
            Rectangle rect = new Rectangle
            {
                Width = cellWidth,
                Height = cellHeight,
                Fill = new SolidColorBrush(color)
            };

            //Add the Rectangle to the Grid
            GameGrid.Children.Add(rect);

            //Set the position of the Rectangle in the Grid
            Grid.SetRow(rect, row);
            Grid.SetColumn(rect, col);
        }

        private void ClearGameGrid()
        {
            //Remove all Rectangles (blocks) from the Grid
            GameGrid.Children.Clear();
        }





        private void InitializeGame()
        {
            //Initialize game state variables
            UpdateScore(gameManager.scoreManager.CurrentScore);
            UpdateLevel(gameManager.scoreManager.CurrentLevel);
            UpdateSpeed(gameManager.scoreManager.SuggestSpeed());


            PauseGameButton.IsEnabled = false;
            EndGameButton.IsEnabled = false;
            SoundToggleButton.IsEnabled = false;
        }



        private void UpdateScore(int score)
        {
            ScoreTextBlock.Text = gameManager.scoreManager.CurrentScore.ToString();
        }

        private void UpdateLevel(int level)
        {
            LevelTextBlock.Text = gameManager.scoreManager.CurrentLevel.ToString();
            UpdateSpeed(gameManager.scoreManager.SuggestSpeed());
            
        }

        private void UpdateSpeed(int speed)
        {
            
            gameTimer.Interval=TimeSpan.FromMilliseconds(speed);



            string speedScale = gameManager.scoreManager.GetSpeedScale();

            SpeedTextBlock.Text = speedScale.ToString();
        }


        private void InitializeGameGrid()
        {
            //20 rows
            for (int i = 0; i < 20; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
            }

            //10 columns
            for (int j = 0; j < 10; j++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void SoundToggleButton_Click(object sender, RoutedEventArgs e)
        {
            isMusicPlaying=!isMusicPlaying;
            if (isMusicPlaying)
            {
                if(musicThread==null||!musicThread.IsAlive)
                {
                    musicThread = new Thread(PlayMusic);
                    musicThread.Start();
                }
                SoundImage.Source =  new BitmapImage(new Uri("Images/mute_icon.png", UriKind.Relative));
            }
            else
            {
                backgroundMusicPlayer.Stop();
                SoundImage.Source = new BitmapImage(new Uri("Images/sound_icon.png", UriKind.Relative));
            }

        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();


            gameManager.StartGame();
            gameTimer.Start();
            StartGameButton.IsEnabled = false;
            PauseGameButton.IsEnabled = true;
            EndGameButton.IsEnabled = true;
            SoundToggleButton.IsEnabled = true;
            isGamePause = false;

            isMusicPlaying = true;
            musicThread=new Thread(PlayMusic);
            musicThread.Start();

            if (isMusicPlaying)
            {
                backgroundMusicPlayer.PlayLooping();
                SoundImage.Source = new BitmapImage(new Uri("Images/mute_icon.png", UriKind.Relative));
            }

            UpdateUI();
        }

        private void ResetGame()
        {
            gameManager.scoreManager.ResetScore();
            //Initialize game state variables
            UpdateScore(gameManager.scoreManager.CurrentScore);
            UpdateLevel(gameManager.scoreManager.CurrentLevel);
            UpdateSpeed(gameManager.scoreManager.SuggestSpeed());

            NextBlockCanvas.Children.Clear();
            ClearGameGrid();
        }

        private void PauseGameButton_Click(object sender, RoutedEventArgs e)
        {

            isGamePause = !isGamePause;

            if (isGamePause)
            {
                gameTimer.Stop();

                PauseGameButton.Content = "Resume";

            }
            else
            {
                gameTimer.Start();

                PauseGameButton.Content = "Pause";
            }

        }

        private void EndGameButton_Click(object sender, RoutedEventArgs e)
        {
            isMusicPlaying = false;
            backgroundMusicPlayer.Stop();
            SoundImage.Source = new BitmapImage(new Uri("Images/sound_icon.png", UriKind.Relative));
            SoundToggleButton.IsEnabled = false;
            gameManager.EndGame();
            PauseGameButton.IsEnabled = false;
            EndGameButton.IsEnabled = false;
            StartGameButton.IsEnabled = true;
            backgroundMusicPlayer.Stop();

            
        }

        private void OnGameOver(int finalScore, int finalLevel)
        {
            isMusicPlaying = false;
            backgroundMusicPlayer.Stop();
            SoundImage.Source = new BitmapImage(new Uri("Images/sound_icon.png", UriKind.Relative));
            SoundToggleButton.IsEnabled = false;

            //Execute in the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                GameOverWindow gameOverWindow = new GameOverWindow(finalScore, finalLevel);
                gameOverWindow.ShowDialog();
            });

            StartGameButton.IsEnabled = true;

            ResetGame();

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!gameManager.isGameRunning)
                return;

            switch (e.Key)
            {
                case Key.Left:
                    gameManager.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameManager.MoveBlockRight();
                    break;
                case Key.Up:
                    gameManager.RotateBlock();
                        break;
            }

            UpdateUI();
        }



        private void DrawNextBlock()
        {
            //Clear Canvas
            NextBlockCanvas.Children.Clear();

            var nextBlock = gameManager.GetNextBlock();
            if (nextBlock == null)
            {
                return;
            }

            //Define scaling factor to reduce size of each block cell
            double scaleFactor = 0.5;



            //Calculate size of each cell in the canvas
            double cellSize = Math.Min(NextBlockCanvas.ActualWidth / nextBlock.Shape.GetLength(1), NextBlockCanvas.ActualHeight / nextBlock.Shape.GetLength(0))*scaleFactor;

            //Calculate starting position to center the block in the canvas
            double startX = (NextBlockCanvas.ActualWidth - (cellSize * nextBlock.Shape.GetLength(1))) / 2;
            double startY = (NextBlockCanvas.ActualHeight - (cellSize * nextBlock.Shape.GetLength(0))) / 2;

            //Draw the block
            for (int i = 0; i < nextBlock.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < nextBlock.Shape.GetLength(1); j++)
                {

                    if (nextBlock.Shape[i, j])
                    {
                        DrawCanvasBlock(i, j, cellSize, nextBlock.Color, startX, startY);
                    }
                }
            }
        }

        private void DrawCanvasBlock(int row, int col, double size, string colorString, double startX, double startY)
        {
            var color = (Color)ColorConverter.ConvertFromString(colorString);
            Rectangle rect = new Rectangle
            {
                Width = size,
                Height = size,
                Fill = new SolidColorBrush(color),
            };

            NextBlockCanvas.Children.Add(rect);
            Canvas.SetLeft(rect, startX+ col * size);
            Canvas.SetTop(rect, startY+ row * size);

        }
    }
}