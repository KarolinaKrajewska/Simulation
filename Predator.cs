using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    class Predator : Animal
    { 
        public int hasEaten = 0; //has the Predator eaten this turn? 0 = no, 1 = yes
        Random random = new Random();
        int lastMeal;

        public Predator(int sex, int x, int y, int index, int meal)
        {
            this.sex = sex;
            this.position_x = x;
            this.position_y = y;
            this.id = index;
            this.lastMeal = meal;
        }

        
        public void Eat() //1. loop through predators, 2. loop through nonpredators
        {
            Random rnd = new Random();
            for (int i = 0; i < Globals.nonpredators.Count; i++) //loop through all nonpredators
            {
                if (this.position_x == Globals.nonpredators[i].position_x && this.hasEaten == 0) //if x of a predator = x of a nonpredator...
                {
                    if (this.position_y == Globals.nonpredators[i].position_y)//... and if y = y
                    {
                        Console.WriteLine("A NonPredator {0} has been eaten by Predator {1}!", Globals.nonpredators[i].id, this.id);
                        Globals.nonpredators.Remove(Globals.nonpredators[i]); //eat the non predator
                        this.lastMeal = Globals.turn;
                        this.hasEaten = 1;
                        
                    }
                } else
                {
                    continue;
                }
            }
        }

        public void Update()
        {
            if (Globals.turn - this.lastMeal > 1)
            {
                Globals.predators.Remove(this);
                Console.WriteLine("Predator {0} died!", this.id);
            } 
        }
        
        public void Reproduce()
        {
            //1. loop through other predators
            //2. if you find a one with the same x
            //3. then check for y and sex; if sex is different add a new predator
            Random rnd = new Random();
            for (int i = 0; i < Globals.predators.Count; i++)
            {
                    if (this.position_x == Globals.predators[i].position_x && this.position_y == Globals.predators[i].position_y && this.sex != Globals.predators[i].sex && this.hasReproduced == 0 && Globals.predators[i].hasReproduced == 0)
                    {
                        Console.WriteLine("New Predator has appeared! Offspring of {0} and {1}", this.id, Globals.predators[i].id);
                        
                        //create a new predator and insert it in the list of predators
                        this.hasReproduced = 1;
                        Globals.predators[i].hasReproduced = 1;
                        Globals.predators.Add(new Predator(rnd.Next(2), rnd.Next(Globals.size_x + 1), rnd.Next(Globals.size_y + 1), Globals.pid, Globals.turn));
                        Globals.pid = Globals.pid + 1;
                    }
                
            }
        }
        }
}
