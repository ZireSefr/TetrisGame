using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.BLL
{
    public class Settings
    {
        public int DifficultyLevel { get;set; }
        public bool SoundOn {  get;set; }

        public Settings() 
        {
            //Initialize default settings
            DifficultyLevel = 1;
            SoundOn = true;
        }
    }
}
