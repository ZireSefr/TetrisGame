using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TetrisGame.BLL.Block;

namespace TetrisGame.BLL
{
    public class BlockManager
    {
        private Block currentBlock;
        private Random random;
        private Block nextBlock;
        private BoardManager boardManager;



        public BlockManager(BoardManager boardManager) 
        {

            this.boardManager = boardManager;

            random = new Random();
            GenerateNextBlock();

        }

        public Block GetCurrentBlock()
        {
            return currentBlock;
        }

        public Block GetNextBlock()
        {
            return nextBlock;
        }

        public void GenerateNextBlock()
        {
            //Define set of possible shapes
            bool[][,] shapes =
            {
                //Line shape I
                new bool[,]{{true,true,true,true,true}},

                //Square shape O
                new bool[,]{{true,true}, { true, true } },


                //T shape
                new bool[,] { { true, true, true },{ false, true, false } },


                //Z shape
                new bool[,] { { true, true,false},{false,true,true } },

            };

            //Set of possible colors
            Color[] colors = { Color.Yellow };

            //Randomlz select a shape and a color
            bool[,] shape = shapes[random.Next(shapes.Length)];
            Color color = colors[random.Next(colors.Length)];

            Block.BlockShape shapeType = DetermineShapeType(shape);


            //Create a new block with the sekected shape and color
            nextBlock = new Block(color, shape, shapeType);


        }

        private Block.BlockShape DetermineShapeType(bool[,] shape)
        {

            int rows=shape.GetLength(0);
            int cols=shape.GetLength(1);  

            //Check for Line shape
            if((rows==1&&cols==5)||(rows==5&&cols==1))
            {
                return Block.BlockShape.Line;
            }

            //Check for Square shape
            if (rows == 2 && cols == 2 && shape[0, 0] && shape[0, 1] && shape[1, 0] && shape[1,1])
            {
                return Block.BlockShape.O;
            }

            //Check for T shape
            if (rows == 2 && cols == 3 && shape[0, 0] && shape[0, 1] && shape[0, 2] && shape[1,1])
            {
                return Block.BlockShape.T;
            }

            //Check for Z shape
            if (rows == 2 && cols == 3 && shape[0, 0] && shape[0, 1] && shape[1, 1] && shape[1, 2])
            {
                return Block.BlockShape.Z;
            }

            //Default return if no shape matches
            return Block.BlockShape.Line;
        }

        public void MoveToTheNextBlock()
        {
            if(nextBlock==null)
            {
                GenerateNextBlock() ;
            }
            currentBlock= nextBlock;
            GenerateNextBlock();
        }


        public void MoveCurrentBlockLeft()
        {

            if (currentBlock == null) return;


            //Calculate new position
            var newPosition = new Block.Position(currentBlock.CurrentPosition.Row, currentBlock.CurrentPosition.Column-1);

            //Check if the move is valid
            if (IsValidMove(newPosition))
            {
                //Clear the block's current position on the board
                boardManager.ClearBlock(currentBlock);

                //Move the block
                currentBlock.MoveLeft();

                //Update the board
                boardManager.UpdateBoard(currentBlock);

            }

        }

        private bool IsValidMove(Block.Position newPosition)
        {
            var shape = currentBlock.Shape;

            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j])
                    {
                        int newRow = newPosition.Row + i;
                        int newCol = newPosition.Column + j;

                        //Check if the new position is outside the board's boundaries
                        if (newRow < 0 || newRow >= BoardManager.Rows || newCol < 0 || newCol >= BoardManager.Columns)
                        {
                            return false;
                        }

                        //Check if the new position collides with a locked block
                        if (boardManager.GetLockedBoardState()[newRow, newCol])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        public void MoveCurrentBlockRight()
        {
            if (currentBlock == null) return;

            //Calculate new position
            var newPosition = new Block.Position(currentBlock.CurrentPosition.Row, currentBlock.CurrentPosition.Column + 1);

            //Check if the move is valid
            if (IsValidMove(newPosition))
            {
                //Clear the block's current position on the board
                boardManager.ClearBlock(currentBlock);

                //Move the block
                currentBlock.MoveRight();

                //Update the board
                boardManager.UpdateBoard(currentBlock);
            }
        }

        public void MoveCurrentBlockDown()
        {
            if(currentBlock == null)
            {
                //Handle the null case by generating a new block
                MoveToTheNextBlock();
            }
            else
            {
                currentBlock.MoveDown();
            }

        }

        public void RotateCurrentBlock()
        {
            if (currentBlock == null)
                return;

            //Save orginal shape in case we need to revert
            var originalShape = currentBlock.Shape;
            var originalPosition = currentBlock.CurrentPosition;

            boardManager.ClearBlock(currentBlock);
            //Rotate the block
            currentBlock.Rotate();


            //Check if the new position is valid after rotation
            if (IsValidRotation(currentBlock))
            {


                boardManager.UpdateBoard(currentBlock);
            }
            else
            {
                currentBlock.UpdateShape(originalShape);
                currentBlock.CurrentPosition = originalPosition;
                boardManager.UpdateBoard(currentBlock);

            }
        }

        private bool IsValidRotation(Block block)
        {
            if (block.ShapeType == Block.BlockShape.Line)
            {
                bool result= IsWithinGridAfterRotation(block.Shape, currentBlock.CurrentPosition);
                return result;
            }


            var shape = block.Shape;
            var position = block.CurrentPosition;

            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j])
                    {
                        int newRow = position.Row + i;
                        int newCol = position.Column + j;

                        //Check if the new position is outside the board's boundaries
                        if (newRow < 0 || newRow >= BoardManager.Rows || newCol < 0 || newCol >= BoardManager.Columns)
                        {
                            return false;
                        }

                        //Check if the new position collides with locked block
                        if (boardManager.GetLockedBoardState()[newRow, newCol])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool IsWithinGridAfterRotation(bool[,] rotatedShape, Position newPosition)
        {


            for (int i = 0; i < rotatedShape.GetLength(0); i++)
            {
                for (int j = 0; j < rotatedShape.GetLength(1); j++)
                {
                    if (rotatedShape[i, j])
                    {

                        int newRow = newPosition.Row + i + 1;
                        int newCol = newPosition.Column + j;


                        if (newRow < 0 || newRow >= BoardManager.Rows || newCol < 0 || newCol >= BoardManager.Columns)
                        {
                            return false;
                        }

                    }
                }
            }
            return true;
        }

        public bool CanMoveDown()
        {
            
            if(currentBlock == null)
            {
                return false;
            }

            var position = currentBlock.CurrentPosition;
            var shape=currentBlock.Shape;

            for(int i=0; i<shape.GetLength(0); i++)
            {
                for(int j=0; j<shape.GetLength(1); j++)
                {
                    if (shape[i,j])
                    {

                        int newRow = position.Row + i + 1;
                        int newCol = position.Column + j;

                        //Check if the new position is outside the board's bottom boundary
                        if (newRow>=BoardManager.Rows)
                        {
                            return false;
                        }

                        //Check if the new position is outside left or right boundaries
                        if (newCol >= BoardManager.Columns || newCol<0 )
                        {
                            return false;
                        }

                        //Check if the nre position collides with an existing block on the board
                        if(boardManager.GetLockedBoardState()[newRow, newCol])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
