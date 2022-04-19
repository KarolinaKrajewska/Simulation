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
        int quantity;
        int index; //indices of animals; has to be incremented before each use
        public List<Predator> predators = new List<Predator>();
        public List<NonPredator> nonpredators = new List<NonPredator>();
        public delegate void collectDeadAnimals(Animal animal, object[,] grid);
        collectDeadAnimals collect;
        List<Animal> deadAnimals = new List<Animal>();
        List<Animal> hasReproducedList = new List<Animal>();
        public object[,] grid;

        Random random = new Random();

        public Simulation(int size_x, int size_y, int maxTurn, int quantity)
        {
            this.size_x = size_x;
            this.size_y = size_y;
            this.maxTurn = maxTurn;
            this.quantity = quantity;
        }

        public void AddAnimals(object[,] grid)
        {//i = x + (size_x * y)
            
            for (int i = 0; i < quantity; i++) //create predators and nonpredators
            {
                int isPredator = random.Next(2);
                int x = random.Next(size_x);
                int y = random.Next(size_y);
                index++;
                if (isPredator == 1) //create a predator
                {
                    Predator predator = new Predator(GetRandomSex(), x, y, index, 1, this.size_x);
                    predators.Add(predator);
                    AddAnimalToGrid(grid, predator.position_i, predator);
                    Console.WriteLine("New Predator {0} - x: {1}, y: {2}, sex: {3}", predator.id,
                        predator.position_x, predator.position_y, predator.sex);
                }
                else //create a nonpredator
                {
                    NonPredator nonpredator = new NonPredator(GetRandomSex(), x, y, index, this.size_x);
                    nonpredators.Add(nonpredator);
                    AddAnimalToGrid(grid, nonpredator.position_i, nonpredator);
                    Console.WriteLine("New NonPredator {0} - x: {1}, y: {2}, sex: {3}", nonpredator.id,
                        nonpredator.position_x, nonpredator.position_y, nonpredator.sex);
                }
            }
        }

        void AddAnimalToGrid(object[,] grid, int index, Animal animal)
        {
            
            if (animal is Predator)
            {
                for (int i = 4; i <= 6; i++)
                {
                    if (grid[index, i] == null)
                    {
                        grid[index, i] = animal;
                        Predator predator = (Predator)grid[index, i];
                        animal.indexAt2D = i;
                        break;
                    }
                    else if (grid[index, 6] != null)
                    {
                        Console.WriteLine("Too many predators on this tile! Predator {0} will die.", animal.id);
                        grid[animal.position_i, animal.indexAt2D] = null;
                        deadAnimals.Add(animal);
                        animal.hasReproduced = true;
                        animal.alive = false;
                        break;
                    }
                }

            }
            else if (animal is NonPredator)
            {
                for (int j = 7; j <= 9; j++)
                {
                    if (grid[index, j] == null)
                    {
                        grid[index, j] = animal;
                        animal.indexAt2D = j;
                        break;
                    }
                    else if (grid[index, 9] != null)
                    {
                        Console.WriteLine("Too many nonpredators on this tile! NonPredator {0} will die.", animal.id);
                        deadAnimals.Add(animal);
                        grid[animal.position_i, animal.indexAt2D] = null;
                        animal.hasReproduced = true;
                        break;
                    }
                }
            }

            
        }
        public object[,] InitializeGrid()
        {
            //In 2nd dimension array 0-3 are neighbouring tiles, 4-6 predators on a tile, 7-9 nonpredators on a tile
            //When you want to do eat you have to iterate through all the tiles and check if 4-6 and 7-9 have animals in them
            //(0,0) = bottom left corner
            int size = this.size_x * this.size_y;
            object[,] grid = new object[size, 10]; //up to 4 neighbours + 6 animals on a tile
            for (int i = 0; i < size; i++) //initialize the grid
            {
                int currentX = i % (this.size_x);
                int currentY = i / (this.size_x);
                if (currentX > 0 && currentX < this.size_x)
                {
                    grid[i, 0] = (currentX - 1) + (this.size_x * currentY);//left

                }
                if (currentX >= 0 && currentX < this.size_x - 1)
                {
                    grid[i, 1] = (currentX + 1) + (this.size_x * currentY); //right
                }
                if (currentY >= 0 && currentY < this.size_y - 1)
                {

                    grid[i, 2] = currentX + (this.size_x * (currentY + 1)); //down
                }
                if (currentY > 0 && currentY < this.size_y)
                {
                    grid[i, 3] = currentX + (this.size_x * (currentY - 1)); //up
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
            collect = new collectDeadAnimals(RemoveDeadAnimal);
            grid = InitializeGrid();
            AddAnimals(grid);
            int currentTurn = 0;
            while (currentTurn < maxTurn && predators.Count > 0)
            {
                foreach (Animal animal in deadAnimals)
                {
                    collect(animal, grid);
                }
                foreach (Animal animal in hasReproducedList)
                {
                    animal.hasReproduced = false;
                }
                hasReproducedList.Clear();
                currentTurn++;
                Console.WriteLine("\nTurn: {0}", currentTurn);
                MoveAnimals(grid);
                Eat(currentTurn, grid);
                ReproduceAllAnimals(currentTurn);
                foreach (Animal animal in deadAnimals)
                {
                    collect(animal, grid);
                }
                

            }
        }

        void MoveAnimals(object[,] grid)
        {
            foreach (Predator predator in predators)
            {
                MoveAnimal(predator, grid);
            }
            foreach (NonPredator nonpredator in nonpredators)
            {
                MoveAnimal(nonpredator, grid);
            }
        }

        void MoveAnimal(Animal animal, object[,] grid)
        {

            grid[animal.position_i, animal.indexAt2D] = null;
            int result = GetRandomPosition(animal.position_i, grid);
            animal.position_x = result % this.size_x;
            animal.position_y = result / this.size_x;
            animal.position_i = animal.position_x + (size_x * animal.position_y);
            AddAnimalToGrid(grid, result, animal);

            if (animal is Predator)
            {
                Console.WriteLine("Predator {0} after moving - x: {1}, y: {2}", animal.id, animal.position_x,
                    animal.position_y);
            }
            else if (animal is NonPredator)
            {
                Console.WriteLine("NonPredator {0} after moving - x: {1}, y: {2}", animal.id, animal.position_x,
                    animal.position_y);
            }
        }

        int GetRandomPosition(int i, object[,] grid) //get one of possibilities of movement randomly
        {

            int rnd = random.Next(4);
            if (grid[i, rnd] != null && (int)grid[i, rnd] != 0)
            {
                return (int)grid[i, rnd];
            }
            else
            {
                return GetRandomPosition(i, grid);
            }
        }

        void Eat(int currentTurn, object[,] grid)
        {
            collect = new collectDeadAnimals(RemoveDeadAnimal);
            foreach (Animal animal in deadAnimals)
            {
                collect(animal, grid);
            }
            for (int i = 0; i < size_x * size_y; i++)
            {
                for (int j = 4; j <= 6; j++)
                {
                    if (grid[i, j] != null && grid[i, j+3] != null)
                    {
                        Predator predator = (Predator)grid[i, 4];
                        predator.lastMeal = currentTurn;
                        NonPredator nonpredator = (NonPredator)grid[i, 7];
                        deadAnimals.Add(nonpredator);
                        Console.WriteLine("NonPredator {0} has been eaten by Predator {1}", nonpredator.id, predator.id);

                    }
                }

                if (grid[i, 4] != null)
                {
                    Predator predator = (Predator)grid[i, 4];
                    if (currentTurn - predator.lastMeal >= 2)
                    {
                        deadAnimals.Add(predator);
                        Console.WriteLine("Predator {0} has died", predator.id);
                    }
                }

            }

            foreach (Animal animal in deadAnimals)
            {
                collect(animal, grid);
            }

        }

        void RemoveDeadAnimal(Animal animal, object[,] grid)
        {
            grid[animal.position_i, animal.indexAt2D] = null;

            if (animal is Predator)
            {
                predators.Remove((Predator)animal);
                
                
            }
            else if (animal is NonPredator)
            {
                nonpredators.Remove((NonPredator)animal);
            }
        }

        void CreateNewAnimal(int currentTurn, Animal animal1, Animal animal2)
        {
            index++;
            animal1.hasReproduced = true;
            animal2.hasReproduced = true;
            hasReproducedList.Add(animal1);
            hasReproducedList.Add(animal2);

            if (animal1 is Predator)
            {
                
                Predator newAnimal = new Predator(GetRandomSex(), random.Next(this.size_x), random.Next(this.size_y),
                    index, currentTurn, this.size_x);
                predators.Add(newAnimal);
                AddAnimalToGrid(grid, newAnimal.position_i, newAnimal);
                Console.WriteLine("New Predator! Child of {0} and {1}, x: {2}, y: {3}", 
                    animal1.id, animal2.id, newAnimal.position_x, newAnimal.position_y);

            }
            else if (animal1 is NonPredator)
            {
                
                NonPredator newAnimal = new NonPredator(GetRandomSex(), random.Next(this.size_x), random.Next(this.size_y),
                    index, this.size_x);
                nonpredators.Add(newAnimal);
                AddAnimalToGrid(grid, newAnimal.position_i, newAnimal);
                Console.WriteLine("New NonPredator! Child of {0} and {1}, x: {2}, y: {3}", 
                    animal1.id, animal2.id, newAnimal.position_x, newAnimal.position_y);
            }
            
        }

        void ReproduceSpecies(int currentTurn, bool isPredator, object[,] grid, int i) //i = current index
        {
            int j;
            if (isPredator == true)
            {
                j = 4;
            } else
            {
                j = 7;
            }

            if (grid[i, j] != null && grid[i, j+1] != null) //predators
            {

                Animal animal1 = (Animal)grid[i, j];
                Animal animal2 = (Animal)grid[i, j+1];
                Sex sex1 = animal1.sex;
                Sex sex2 = animal2.sex;
                if (sex1 != sex2 && animal1.hasReproduced == false && animal2.hasReproduced == false)
                {
                    CreateNewAnimal(currentTurn, animal1, animal2);

                }

                if (grid[i, j+2] != null)
                {

                    Animal animal3 = (Animal)grid[i, j+2];

                    Sex sex3 = animal3.sex;
                    if (sex1 != sex3 && animal1.hasReproduced == false && animal3.hasReproduced == false)
                    {
                        CreateNewAnimal(currentTurn, animal1, animal3);

                    }
                    else if (sex2 != sex3 && animal2.hasReproduced == false && animal3.hasReproduced == false)
                    {
                        CreateNewAnimal(currentTurn, animal2, animal3);

                    }
                }

            }

        }

        void ReproduceAllAnimals(int currentTurn)
        {
            for (int i = 0; i < size_x * size_y; i++)
            {

                ReproduceSpecies(currentTurn, true, grid, i);
                ReproduceSpecies(currentTurn, false, grid, i);
                
            }
            
        }
    }


}

