using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    class Animal
    {
        public int sex; // 1 = female, 0 = male
        public int position_x;
        public int position_y;
        public int id; //ID number
        public int hasReproduced = 0;//0 - hasn't, 1 - had
        Random random = new Random();

        public Animal()
        {

        }
        public void move()
        {
            if (position_x < Globals.size_x && position_x > 0) //if x is in range 0-10
            {
                this.position_x = this.position_x + random.Next(-1, 2); //randomly change position by max 1
            }
            else if (position_x == Globals.size_x)
            {
                this.position_x = this.position_x + random.Next(-1, 0);
            }
            else if (position_x == 0)
            {
                this.position_x = this.position_x + random.Next(2);
            }

            if (position_y < Globals.size_y && position_y > 0) //if y is in range 0-10
            {
                this.position_y = this.position_y + random.Next(-1, 2); //randomly change position by max 1
            }
            else if (position_y == Globals.size_y)
            {
                this.position_y = this.position_y + random.Next(-1, 0);
            }
            else if (position_y == 0)
            {
                this.position_y = this.position_y + random.Next(2);
            }
        }

    }
}
