
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    class Predator : Animal
    {
        public int hasEaten = 0; //has the Predator eaten this turn? 0 = no, 1 = yes
        Random random = new Random();
        public int lastMeal;

        public Predator(Sex sex, int x, int y, int index, int meal, int size_x)
        {
            this.sex = sex;
            this.position_x = x;
            this.position_y = y;
            this.id = index;
            this.lastMeal = meal;
            this.alive = true;
            this.position_i = x + (size_x * y);
        }

    }
}

