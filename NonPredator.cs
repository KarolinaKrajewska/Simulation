using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    class NonPredator : Animal
    {
        
        Random random = new Random();

        public NonPredator(int sex, int x, int y, int index)
        {
            this.sex = sex;
            this.position_x = x;
            this.position_y = y;
            this.id = index;
        }

        public void Reproduce()
        {
            //1. loop through other predators
            //2. if you find a one with the same x
            //3. then check for y and sex; if sex is different add a new predator
            Random rnd = new Random();
            for (int i = 0; i < Globals.nonpredators.Count; i++)
            {
                    if (this.position_x == Globals.nonpredators[i].position_x && this.position_y == Globals.nonpredators[i].position_y && this.sex != Globals.nonpredators[i].sex && this.hasReproduced == 0 && Globals.nonpredators[i].hasReproduced == 0)
                    {
                    Console.WriteLine("New NonPredator has appeared! Offspring of {0} and {1}", this.id, Globals.nonpredators[i].id);
                        //create a new predator and insert it in the list of predators
                        this.hasReproduced = 1;
                        Globals.nonpredators[i].hasReproduced = 1;
                        Globals.nonpredators.Add(new NonPredator(rnd.Next(2), rnd.Next(Globals.size_x + 1), rnd.Next(Globals.size_y + 1), Globals.npid));
                        Globals.npid = Globals.npid + 1;
                    }
                
            }
        }
    }
}
