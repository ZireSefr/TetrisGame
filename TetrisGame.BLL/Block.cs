using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.BLL
{
    public class Block
    {
        //Define a Position structure
        public struct Position 
        {
            public int Row; public int Column;

            public Position(int row, int column)
            {
                Row = row; Column = column;
            }
        
        }

        public enum BlockShape
        {
            Line,// Line shape
            O,// Square shape
            T,// T shape
            Z,// Z shape
        }


        //Structure as a property
        public BlockShape ShapeType { get; private set; }
        public Position CurrentPosition { get; set; }
        public string Color { get; private set; }
        public bool[,] Shape { get; private set; }


        public Block(Color color, bool[,] shape, BlockShape shapeType) 
        {
            //Convert color to a hex string
            Color = ColorToHexString(color);
            Shape = shape;
            ShapeType = shapeType;
            //Initial position
            CurrentPosition = new Position(0, 3);
        }

        public void UpdateShape(bool[,] newShape)
        {
            Shape = newShape;
        }

        private string ColorToHexString(Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public void MoveLeft()
        {
            CurrentPosition = new Position(CurrentPosition.Row, CurrentPosition.Column-1);
        }

        public void MoveRight()
        {
            CurrentPosition = new Position(CurrentPosition.Row, CurrentPosition.Column + 1);
        }

        public void MoveDown()
        {
            CurrentPosition = new Position(CurrentPosition.Row+1, CurrentPosition.Column);
        }

        public void Rotate()
        {
            switch (ShapeType)
            {
                case BlockShape.Line:
                    RotateLine(); 
                    break;
                case BlockShape.Z:
                    RotateZ(); 
                    break;
                case BlockShape.T:
                    RotateT(); 
                    break;
                case BlockShape.O: RotateO(); 
                    break;
            }
        }

        private void RotateLine()
        {
            bool[,] originalShape = Shape;
            Position originalPosition = CurrentPosition;

            Shape = Rotate90Degrees(Shape);

            int originalRows = originalShape.GetLength(0);
            int originalCols = originalShape.GetLength(1);

            if (originalRows > originalCols)
            {
                CurrentPosition = new Position(CurrentPosition.Row + 2, CurrentPosition.Column - 2);
            }
            else
            {
                CurrentPosition = new Position(CurrentPosition.Row - 2, CurrentPosition.Column + 2);
            }

        }


        private void RotateO()
        {
            Shape = Rotate90Degrees(Shape);
        }

        private void RotateZ()
        {
            Shape = Rotate90Degrees(Shape);
        }

        private void RotateT()
        {
            Shape = Rotate90Degrees(Shape);
        }

        private bool[,] Rotate90Degrees(bool[,] shape)
        {
            int rows =shape.GetLength(0);
            int cols =shape.GetLength(1);
            bool[,] rotatedShape = new bool[cols, rows];

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    rotatedShape[j,rows-1-i] = shape[i,j];
                }
            }

            return rotatedShape;
        }
    }
}
