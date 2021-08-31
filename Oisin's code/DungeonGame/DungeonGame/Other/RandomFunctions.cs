using System;

namespace DungeonGame.Other
{
    /*
    * A class containing methods that prduce random numbers
    */
    class RandomFunctions
    {
        /*
         * Return random number between range max is allowed to be a return
         */
        public static int randomRangeWithMax(int min, int max)
        {
            Random rand = new Random();
            //max + 1 to let return number also be max
            return rand.Next(min, max + 1);
        }

        /*
        * Return random number between range
        */
        public static int randomRange(int min, int max)
        {
            Random rand = new Random();
            return rand.Next(min, max);
        }

        /*
         * calculates a number, if its <= your chance 
         * return true
         */
        public static bool randomChance(double chance)
        { 
            Random random = new Random();
            if(random.NextDouble() <= chance)
            {
                return true;
            }
            return false;
        }
    }
}
