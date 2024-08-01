using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.BLL
{
    public class ScoreManager
    {
        public int CurrentScore { get; private set; }
        public int CurrentLevel {  get; private set; }

        public event Action<int> ScoreUpdate;
        public event Action<int> LevelUpdate;

        // public int HighScore { get; private set; }

        public ScoreManager()
        {
            CurrentScore = 0;
            CurrentLevel = 1;
        }

        public void UpdateScore(int linesCleared)
        {

            //Update score based on the number of lines cleared
            CurrentScore += linesCleared * 100;
            ScoreUpdate?.Invoke(CurrentScore);

            CheckLevelUpdate();

        }

        private void CheckLevelUpdate()
        {
            int newLEvel = CurrentScore / 100 + 1;
            if(newLEvel!=CurrentLevel)
            {
                CurrentLevel = newLEvel;
                LevelUpdate?.Invoke(CurrentLevel);
            }
        }

        public void ResetScore()
        {
            CurrentScore = 0;
            CurrentLevel= 1;
        }

        public int SuggestSpeed()
        {
            int minimumSpeed = 500;
            int maximumSpeed = 100;
            //Decrease the increment for more gradual increase in speed
            int incrementSpeed = 100;

            int newSpeed=minimumSpeed-(CurrentLevel-1)*incrementSpeed;
            return Math.Max(newSpeed, maximumSpeed);


        }

        public string GetSpeedScale()
        {
            int speedLevel = DetermineSpeedLevel();
            string speedScale = "";

            switch(speedLevel)
            {
                case 1:
                    speedScale = "Very slow";
                    break;
                case 2:
                    speedScale = "Slow";
                    break;
                case 3:
                    speedScale = "Medium";
                    break;
                case 4:
                    speedScale = "Fast";
                    break;
                case 5:
                    speedScale = "Very Fast!";
                    break;
            }

            return speedScale;
        }

        private int DetermineSpeedLevel()
        {
            int speed=SuggestSpeed();
            if (speed >= 400)
                return 1;

            if (speed >= 300)
                return 2;

            if (speed >= 200)
                return 3;

            if (speed >= 100)
                return 4;

            return 5;
        }

    }
}
