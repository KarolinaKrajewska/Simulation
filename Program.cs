
﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulation
{
    class Program
    {

        public static void Main(string[] args)
        {

            Simulation simulation = new Simulation(5, 5, 5, 5, 10);
            simulation.AddAnimals();
            simulation.Run();
            
        }
    }
}


