using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    static class Globals
    {
        public static int size_x = 5; //size x of the grid
        public static int size_y = 5; //size y of the grid
        public static List<NonPredator> nonpredators = new List<NonPredator>(); //nonpredators
        public static List<Predator> predators = new List<Predator>(); //predators
        public static int turn = 0; //number of current turn
        public static int maxTurn = 10; //maximum number of turns
        public static int pquantity = 5; //quantity of predators
        public static int npquantity = 10; //quantity of nonpredators
        public static int pid = 1; //predator id number generator
        public static int npid = 1; //nonpredator id generator
    }
}
