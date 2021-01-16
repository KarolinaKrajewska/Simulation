
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    class NonPredator : Animal
    {
        

        public NonPredator(Sex sex, int x, int y, int index, int size_x)
        {
            this.sex = sex;
            this.position_x = x;
            this.position_y = y;
            this.id = index;
            this.alive = true;
            this.position_i = x + (size_x * y);
        }

       
    }
}

