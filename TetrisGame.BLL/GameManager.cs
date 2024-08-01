using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TetrisGame.BLL
{
    public class GameManager
    {

        private BoardManager boardManager;
        private BlockManager blockManager;
        public ScoreManager scoreManager {  get; private set; }

        public bool isGameRunning { get; private set; }

        private int speed;


        //Event with score and level as parameters
        public event Action<int, int> GameOver;

        public GameManager()
        {
            boardManager = new BoardManager();
            blockManager = new BlockManager(boardManager);
            scoreManager = new ScoreManager();
            isGameRunning = false;
            speed = 1;

        }

        public bool[,] GetBoardState()
        {
            return boardManager.GetBoardState();
        }

        public string GetCurrentBlockColor()
        {
            return blockManager.GetCurrentBlock().Color;
        }

        public void StartGame()
        {
            scoreManager.ResetScore();
            UpdateSpeed(scoreManager.SuggestSpeed());

            //Initialize game start
            isGameRunning = true;

            //Initialize the board and the first block
            boardManager.ResetBoard();

            //Generate the first block and set it as the current block
            blockManager.GenerateNextBlock();

            //This will set the nextBlock as the currentBlock
            blockManager.MoveToTheNextBlock();
        }

        public void PauseGame()
        {
            isGameRunning = !isGameRunning;
        }

        public void EndGame()
        {
            isGameRunning = false;

            boardManager.ResetBoard();

            int finalScore = scoreManager.CurrentScore;
            int finalLevel = scoreManager.CurrentLevel;

            //Raise the event
            GameOver?.Invoke(finalScore, finalLevel);
        }

        
        
        public void UpdateSpeed(int newSpeed)
        {
            speed = newSpeed;
        }

        public void MoveBlockLeft()
        {
            if (isGameRunning)
            {
                blockManager.MoveCurrentBlockLeft();
                UpdateBoard();
            }
        }

        public void MoveBlockRight()
        {
            if (isGameRunning)
            {
                blockManager.MoveCurrentBlockRight();
                UpdateBoard();
            }
        }



        public void MoveBlockDown()
        {
            if (isGameRunning)
            {
                if (blockManager.CanMoveDown())
                {
                    blockManager.MoveCurrentBlockDown();
                }
                else
                {
                    //Lock the block's position on the board
                    boardManager.LockBlock(blockManager.GetCurrentBlock());

                    //Check for the Completed lines after locking the block
                    CheckForCompletedLines();

                    //Check for game over
                    if (boardManager.IsGameOver())
                    {
                        EndGame();
                        //Stop further execution if the game is over
                        return;
                    }

                    //The current block can no longer move down. Generate new block
                    blockManager.MoveToTheNextBlock();




                }

                //Update the board state
                UpdateBoard();
            }
        }


        private void UpdateBoard()
        {
            //Update the board with the current block's position
            boardManager.UpdateBoard(blockManager.GetCurrentBlock());
        }

        private void CheckForCompletedLines()
        {
            int linesCleared = boardManager.ClearCompletedLines();
            if (linesCleared > 0)
            {
                scoreManager.UpdateScore(linesCleared);
                UpdateSpeed(scoreManager.SuggestSpeed());

            }
        }

        public void RotateBlock()
        {
            if (isGameRunning)
            {
                blockManager.RotateCurrentBlock();
                UpdateBoard();
            }
        }

        private int CalculateScore(int linesCleared)
        {
            //Scoring logic based on lines cleared
            return linesCleared * 100;
        }

        public Block GetNextBlock()
        {
            return blockManager.GetNextBlock();
        }
    }
}
