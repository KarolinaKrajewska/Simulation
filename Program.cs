using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulation
{
    class Program
    {
        
        public static void Main(string[] args)
        {

            Random rnd = new Random(); //for generating random numbers
            
            for (int i = 0; i < Globals.npquantity; i++)
            {   //create non nonpredators
                Globals.nonpredators.Insert(i, new NonPredator(rnd.Next(2), rnd.Next(Globals.size_x + 1), rnd.Next(Globals.size_y + 1), Globals.npid));
                NonPredator currentnon = Globals.nonpredators[i];
                Console.WriteLine("New non-predator; x: {0}, y: {1}, sex: {2}", currentnon.position_x, currentnon.position_y, currentnon.sex);
                Globals.npid = Globals.npid + 1;

            }

            for (int i = 0; i < Globals.pquantity; i++)
            {
                //create predators
                Globals.predators.Insert(i, new Predator(rnd.Next(2), rnd.Next(Globals.size_x + 1), rnd.Next(Globals.size_y + 1), Globals.pid, Globals.turn));//sex, x, y, index, lastMeal
                Predator current = Globals.predators[i];
                Console.WriteLine("New predator; x: {0}, y: {1}, sex: {2}", current.position_x, current.position_y, current.sex);
                Globals.pid = Globals.pid + 1;
            }

            while (true) //infinite loop
            {
                string next = Console.ReadLine();
                if (next == "n" && Globals.turn != Globals.maxTurn && Globals.predators.Count != 0) //next turn
                {   
                    foreach (Predator predator in Globals.predators) //zero the flags
                    {
                        predator.hasEaten = 0;
                        predator.hasReproduced = 0;
                    }
                    foreach (NonPredator nonpredator in Globals.nonpredators)
                    {
                        nonpredator.hasReproduced = 0; //zero the flag
                    }
                    Globals.turn = Globals.turn + 1;
                    Console.WriteLine("Turn: {0}", Globals.turn);
                    NonPredator[] nons = new NonPredator[Globals.nonpredators.Count];
                    Globals.nonpredators.CopyTo(nons);
                    foreach (NonPredator nonpredator in nons) //move all nonpredators
                    {
                        
                        nonpredator.move();
                        Console.WriteLine("Nonpredator {0} after moving - x: {1}, y: {2}", nonpredator.id, nonpredator.position_x, nonpredator.position_y);
                        
                    }
                    
                    Predator[] pred = new Predator[Globals.predators.Count];
                    Globals.predators.CopyTo(pred);
                    foreach (Predator predator in pred) //move all predators, make them eat, update to see if they are still alive, reproduce
                    {
                        predator.move();
                        Console.WriteLine("Predator {0} after moving - x: {1}, y: {2}", predator.id, predator.position_x, predator.position_y);
                        predator.Eat();
                        predator.Update();
                        //predator.Reproduce();
                    }
                    for (int i = 0; i < Globals.predators.Count; i++) 
                    {
                        Globals.predators[i].Reproduce(); //this has to be separate  from the loop above because animals are supposed to reproduce after ALL of them has moved
                    }
                    for (int i = 0; i < Globals.nonpredators.Count; i++)
                    {
                        Globals.nonpredators[i].Reproduce(); //this has to be separate from the loop above because animals are supposed to reproduce after ALL of them has moved
                    }



                } else if (Globals.turn == Globals.maxTurn || Globals.predators.Count == 0) //if it's the last turn or there are no predators left
                {
                    Console.WriteLine("End of simulation");
                }
            }
        }
    }
}

