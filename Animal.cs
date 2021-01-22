using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    
    public enum Sex
    {
        Male, Female
    }
    public class Animal
    {
        public int indexAt2D;
        public Sex sex; 
        public int position_x;
        public int position_y;
        public int position_i; //index in 1D array
        public int id; //ID number
        public bool hasReproduced = false;//false - hasn't, true - had
        public bool alive; //true = alive, false = dead


    }
}

