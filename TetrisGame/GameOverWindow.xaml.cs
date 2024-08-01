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
using System.Windows.Shapes;

namespace TetrisGame
{
    /// <summary>
    /// Interaction logic for GameOverWindow.xaml
    /// </summary>
    public partial class GameOverWindow : Window
    {
        public GameOverWindow(int score, int level)
        {
            InitializeComponent();
            ScoreTextBlock.Text = "Score: "+score.ToString();
            LevelTextBlock.Text = "Level: "+level.ToString();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
