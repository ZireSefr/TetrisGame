namespace TetrisGame.BLL
{
    public class BoardManager
    {

        private bool[,] board;
        public static int Rows { get; } = 20;
        public static int Columns { get; } = 10;

        private bool[,] lockedBlockes;

        public BoardManager()
        {
            //Initialize the board array based on game dimensions
            board = new bool[Rows, Columns];

            //locked blocks array
            lockedBlockes = new bool[Rows, Columns];
        }

        public bool[,] GetBoardState()
        {
            return board;
        }

        public bool[,] GetLockedBoardState()
        {
            return lockedBlockes;
        }

        public void ResetBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    board[row, col] = false;// All positions to false(empty)
                    lockedBlockes[row, col] = false; ;
                }
            }
        }


        public void UpdateBoard(Block currentBlock)
        {
            // ResetBoard();


            //Reset only the positions previously occupied by the current block
            ClearCurrentBlockFromBoard(board, currentBlock);


            if (currentBlock == null || currentBlock.Shape == null)
            {
                return;
            }

            //Place the block in its position
            for (int i = 0; i < currentBlock.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < currentBlock.Shape.GetLength(1); j++)
                {
                    if (currentBlock.Shape[i, j])
                    {
                        int row = currentBlock.CurrentPosition.Row + i;
                        int col = currentBlock.CurrentPosition.Column + j;

                        if (row >= 0 && row < Rows && col >= 0 && col < Columns)
                        {
                            board[row, col] = true;
                        }
                    }
                }
            }

            //Combine with locked blocks
            for(int row = 0;row < Rows; row++)
            {
                for(int col = 0;col  < Columns; col++)
                {
                    if (lockedBlockes[row, col])
                    {
                        board[row, col] = true;
                    }
                }
            }
        }


        public int ClearCompletedLines()
        {

            int clearedLines = 0;

            for (int row = 0; row < Rows; row++)
            {
                if (IsLineComplete(row))
                {
                    ClearLine(row);
                    ShiftDown(row);
                    clearedLines++;
                    //To recheck same row index because the lines above have shifted down
                    row--;
                }
            }

            return clearedLines;
        }

        private bool IsLineComplete(int row)
        {
            for (var col = 0; col < Columns; col++)
            {
                if (!board[row, col])
                {

                    return false; //If any column ubn the row is empty, the line is not complete
                }
            }

            return true;
        }

        private void ClearLine(int row)
        {
            for (int col = 0; col < Columns; col++)
            {
                //Clear the line
                board[row, col] = false;
                //Also clear the locked blocks array
                lockedBlockes[row, col] = false;
            }
        }

        private void ShiftDown(int startRow)
        {
            for (int row = startRow; row > 0; row--)
            {
                for (int col = 0; col < Columns; col++)
                {
                    //Shift every line down
                    board[row, col] = board[row - 1, col];
                    //Shift down locked blocks as well
                    lockedBlockes[row, col] = lockedBlockes[row - 1, col];
                }
            }

            //Clear the top line
            for (int col = 0; col < Columns; col++)
            {
                board[0, col] = false;
                lockedBlockes[0, col] = false;
            }
        }

        public void LockBlock(Block block)
        {
            var shape=block.Shape;
            var position = block.CurrentPosition;

            for(int i = 0; i< shape.GetLength(0);i++)
            {
                for(int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j])
                    {
                        int row=position.Row+i;
                        int col=position.Column+j;

                        if(row >= 0&&row<Rows&&col >= 0&&col < Columns)
                        {
                            lockedBlockes[row, col] = true;
                        }
                    }
                }
            }
        }

        private void ClearCurrentBlockFromBoard(bool[,] tempBoard, Block currentBlock)
        {
            if (currentBlock == null || currentBlock.Shape == null)
            {
                return;
            }

            //Get the previous position of the current block
            var prevPosition = new Block.Position(
                currentBlock.CurrentPosition.Row - 1, currentBlock.CurrentPosition.Column);

            //Clear only positions previously occupied by the current block
            for (int i = 0; i < currentBlock.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < currentBlock.Shape.GetLength(1); j++)
                {
                    if (currentBlock.Shape[i, j])
                    {
                        int row = prevPosition.Row + i;
                        int col = prevPosition.Column + j;

                        if (row >= 0 && row < Rows && col >= 0 && col < Columns)
                        {
                            //Only clear the position if it is not a locked block
                            if (!lockedBlockes[row, col])
                            {
                                tempBoard[row, col] = false;
                            }
                        }
                    }
                }
            }
        }

        public void ClearBlock(Block block)
        {

            if (block == null || block.Shape == null)
            {
                return;
            }

            
            //Clear only positions previously occupied by the current block
            for (int i = 0; i < block.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < block.Shape.GetLength(1); j++)
                {
                    if (block.Shape[i, j])
                    {
                        int row = block.CurrentPosition.Row + i;
                        int col = block.CurrentPosition.Column + j;

                        if (row >= 0 && row < Rows && col >= 0 && col < Columns)
                        {
                            //Only clear the position if it is not a locked block
                            if (!lockedBlockes[row, col])
                            {
                                board[row, col] = false;
                            }
                        }
                    }
                }
            }
        }

        public bool IsGameOver()
        {
            for (int col = 0; col < Columns; col++)
            {
                if (lockedBlockes[0, col])
                {
                    //Game over if any cell in the first row is occupied
                    return true;
                }
            }

            return false;
        }
    }
}
