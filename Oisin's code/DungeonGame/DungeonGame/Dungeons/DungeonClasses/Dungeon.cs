using System.Collections.Generic;

namespace DungeonGame.Dungeons.DungeonClasses
{
    /*
     * This is the dungeon class and represents the dungeon the player is in
     * it holds a list of levels
     */
    class Dungeon
    {
        //////////////////// class variables ////////////////////
        public List<Level> Levels { get; private set; }    //List of the levels in the dungeon
        public int Depth { get; private set; }  //the amount of levels in the dungeon 

        public Dungeon(int depth, List<Level> levels)
        {
            this.Depth = depth;
            this.Levels = levels;
        }
    }
}
