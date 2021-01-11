using System;
using System.Collections.Generic;
using System.Text;


namespace Simulation
{
    class Simulation
    {
        int size_x;//size x of the grid
        int size_y; //size y of the grid
        int maxTurn; //max number of turns
        int predatorQuantity;
        int nonPredatorQuantity;
        int index; //indices of animals; has to be incremented before each use
        public List<Predator> predators = new List<Predator>();
        public List<NonPredator> nonpredators = new List<NonPredator>();

        Random random = new Random();

        public Simulation(int size_x, int size_y, int maxTurn, int predatorQuantity, int nonPredatorQuantity)
        {
            this.size_x = size_x;
            this.size_y = size_y;
            this.maxTurn = maxTurn;
            this.predatorQuantity = predatorQuantity;
            this.nonPredatorQuantity = nonPredatorQuantity;
        }

        public void Animals()
        {

            for (int i = 0; i < predatorQuantity; i++) //create predators
            {
                int x = random.Next(size_x);
                int y = random.Next(size_y);
                index++;
                predators.Add(new Predator(GetRandomSex(), x, y, index, 1, this.size_x));
                Predator currentPredator = predators[i];
                Console.WriteLine("New Predator {0} - x: {1}, y: {2}, sex: {3}", currentPredator.id,
                    currentPredator.position_x, currentPredator.position_y, currentPredator.sex);
            }

            for (int i = 0; i < nonPredatorQuantity; i++) //create nonpredators
            {
                int x = random.Next(size_x);
                int y = random.Next(size_y);
                index++;
                nonpredators.Add(new NonPredator(GetRandomSex(), x, y, index, this.size_x));
                NonPredator currentnonPredator = nonpredators[i];
                Console.WriteLine("New NonPredator {0} - x: {1}, y: {2}, sex: {3}", currentnonPredator.id,
                    currentnonPredator.position_x, currentnonPredator.position_y, currentnonPredator.sex);

            }


        }

        int[,] Grid()
        {
            //(0,0) = left down corner
            int size = this.size_x * this.size_y;
            int[,] grid = new int[size, 4]; //up to 4 neighbours
            for (int i = 0; i < size; i++) //initialize the grid
            {
                int currentX = i % (this.size_x);
                int currentY = i / (this.size_x);
                if (currentX > 0 && currentX <= this.size_x)
                {
                    grid[i, 0] = (currentX - 1) + (this.size_x * currentY);//left

                }
                if (currentX >= 0 && currentX < this.size_x - 1)
                {
                    grid[i, 1] = (currentX + 1) + (this.size_x * currentY); //right
                }
                if (currentY >= 0 && currentY < this.size_y - 1)
                {

                    grid[i, 2] = currentX + (this.size_x * (currentY + 1)); //up
                }
                if (currentY > 0 && currentY <= this.size_y)
                {
                    grid[i, 3] = currentX + (this.size_x * (currentY - 1)); //down
                }

            }
            return grid;
        }

        Sex GetRandomSex()
        {
            if (random.Next(2) == 0)
            {
                return Sex.Male;
            }
            else
            {
                return Sex.Female;
            }
        }

        public void Run()
        {
            int[,] grid = Grid();
            int currentTurn = 0;
            while (currentTurn < maxTurn && predators.Count > 0)
            {
                foreach(Predator predator in predators)
                {
                    predator.hasReproduced = false;
                }
                foreach(NonPredator nonpredator in nonpredators)
                {
                    nonpredator.hasReproduced = false;
                }
                currentTurn++;
                Console.WriteLine("\nTurn: {0}", currentTurn);
                this.Move(grid);
                this.Eat(currentTurn);
                this.Reproduce(currentTurn);

            }
        }

        void Move(int[,] grid)
        {
            foreach (Predator predator in predators) //move all predators
            {
                int x = predator.position_x;
                int y = predator.position_y;
                int i = x + (this.size_x * y);
                int result = GetRandomPosition(i, grid);
                predator.position_x = result % this.size_x;
                predator.position_y = result / this.size_x;
                Console.WriteLine("Predator {0} after moving - x: {1}, y: {2}", predator.id, predator.position_x, predator.position_y);

            }
            foreach (NonPredator nonpredator in nonpredators)
            {
                int x = nonpredator.position_x;
                int y = nonpredator.position_y;
                int i = x + (this.size_x * y);
                int result = GetRandomPosition(i, grid);
                nonpredator.position_x = result % this.size_x;
                nonpredator.position_y = result / this.size_x;
                Console.WriteLine("NonPredator {0} after moving - x: {1}, y: {2}", nonpredator.id, nonpredator.position_x, nonpredator.position_y);
            }
        }

        int GetRandomPosition(int i, int[,] grid) //get one of possibilities of movement randomly
        {

            int rnd = random.Next(4);
            if (grid[i, rnd] > 0)
            {
                return grid[i, rnd];
            }
            else
            {
                return GetRandomPosition(i, grid);
            }
        }

        void Eat(int currentTurn)
        {
            foreach (Predator predator in predators)
            {

                foreach (NonPredator nonpredator in nonpredators)
                {

                    if (predator.position_i == nonpredator.position_i)
                    {
                        nonpredator.alive = false;
                        predator.lastMeal = currentTurn;
                        break;
                    }
                }
                if (currentTurn - predator.lastMeal >= 2)
                {
                    predator.alive = false;
                }


            }
            RemoveDeadAnimals();
        }

        void RemoveDeadAnimals()
        {
            for (int i = nonpredators.Count - 1; i >= 0; i--)
            {
                if (nonpredators[i].alive == false)
                {
                    Console.WriteLine("NonPredator {0} is going to be removed!", nonpredators[i].id);
                    nonpredators.Remove(nonpredators[i]);
                    i--;
                }
            }
            for (int i = predators.Count - 1; i >= 0; i--)
            {
                if (predators[i].alive == false)
                {
                    Console.WriteLine("Predator {0} is going to be removed!", predators[i].id);
                    predators.Remove(predators[i]);
                    i--;
                }
            }
        }

        void Reproduce(int currentTurn)
        {
            for (int j = predators.Count - 1; j >=0; j--)
            {
                for (int i = predators.Count - 1; i >= 0; i--)
                {
                    if (predators[j].hasReproduced == false && predators[i].hasReproduced == false)
                    {
                        if (predators[j].position_x == predators[i].position_x &&
                            predators[j].position_y == predators[i].position_y &&
                            predators[j].sex != predators[i].sex)
                        {
                            index++;
                            predators.Add(new Predator(GetRandomSex(), random.Next(this.size_x), random.Next(this.size_y),
                                index, currentTurn, this.size_x));
                            Console.WriteLine("New Predator! Child of {0} and {1}", predators[j].id, predators[i].id);
                            break;
                        }
                    }
                }
            }
            for(int j = nonpredators.Count-1;j >=0; j--)
            {
                for (int i = nonpredators.Count-1; i >= 0; i--)
                {
                    if (nonpredators[j].hasReproduced == false && nonpredators[i].hasReproduced == false)
                    {
                        if (nonpredators[j].position_x == nonpredators[i].position_x &&
                            nonpredators[j].position_y == nonpredators[i].position_y &&
                            nonpredators[j].sex != nonpredators[i].sex)
                        {
                            index++;
                            nonpredators.Add(new NonPredator(GetRandomSex(), random.Next(this.size_x), random.Next(this.size_y),
                                index, this.size_x));
                            nonpredators[j].hasReproduced = true;
                            nonpredators[i].hasReproduced = true;
                            Console.WriteLine("New Non Predator! Child of {0} and {1}", nonpredators[j].id, nonpredators[i].id);
                            break;
                        }
                    }
                }
            }
        }


    }
}
