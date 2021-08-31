using DungeonGame.Constants;
using System.Collections.Generic;

namespace DungeonGame.Dungeons.DungeonClasses
{
    class Room
    {
        //class variables
        public int[] CoOrdinates { get; private set; }    //The rooms co-ordinates
        public int RoomWeight { get; private set; }     //The rooms positional weight
        public List<Direction> Exits { get; set; }      //The exits for a room

        /*
         * This is the public constructor for a room object
         */
        public Room(int x, int y, int weight, List<Direction> exits)
        {     
            this.RoomWeight = weight;
            this.Exits = exits;
        }

        /*
         * each input variable can be one of three vlaues -1,0,1
         * -1 means take away exit, 0 means do nothing, 1 means add exit
         */
        public void addRemoveExits(int north, int east, int south, int west)
        {
            //north
            switch (north)
            {
                //remove north exit
                case -1: if (Exits.Contains(Direction.N)) { Exits.Remove(Direction.N); }
                    break;             
                //do nothing
                case 0:
                    break;
                //add north exit
                case 1: if (!Exits.Contains(Direction.N)) { Exits.Add(Direction.N); }
                    break;
            }

            //south
            switch (south)
            {
                //remove south exit
                case -1: if (Exits.Contains(Direction.S)) { Exits.Remove(Direction.S); }
                    break;
                //do nothing
                case 0:
                    break;
                //add south exit
                case 1: if (!Exits.Contains(Direction.S)) { Exits.Add(Direction.S); }
                    break;
            }

            //north
            switch (east)
            {
                //remove east exit
                case -1: if (Exits.Contains(Direction.E)) { Exits.Remove(Direction.E); }
                    break;
                //do nothing
                case 0:
                    break;
                //add east exit
                case 1: if (!Exits.Contains(Direction.E)) { Exits.Add(Direction.E); }
                    break;
            }

            //north
            switch (west)
            {
                //remove west exit
                case -1: if (Exits.Contains(Direction.W)) { Exits.Remove(Direction.W); }
                    break;
                //do nothing
                case 0:
                    break;
                //add west exit
                case 1: if (!Exits.Contains(Direction.W)) { Exits.Add(Direction.W); }
                    break;
            }


        }
    }
}
