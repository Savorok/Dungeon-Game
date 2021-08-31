using System.Collections.Generic;
using DungeonGame.Other;

namespace DungeonGame.Dungeons.DungeonClasses
{
    /*
     * This is the level class and represents a level in the dungeon
     * it holds the information on the rooms
     */
    class Level
    {
        //////////////////// class variables ////////////////////
        //private List<List<Room>> roomsByRegion; //A list of lists, where each internal lists holds all the rooms in a region
        public int[] Size { get; private set; }                     //the width and height of a level
        public int Depth { get; private set; }                      //The depth of the level in the dungeon
        public Room[,] Rooms { get; private set; }                  //The array of rooms in the level 
        public int[,] RoomArray  { get; private set; }              //The array showing all the rooms in the level       
        public int[,] LevelRegionArray { get; private set; }        //The array of overall level regions
        public int[,] WeightArray { get; private set; }             //The array of the weighted position of each room
        
        //int[,] RoomRegionArray;                 //The array of regions per region (room regions)
        //int[,] RoomArrayNegative;               //The array showing the negative of the room array

        /*
         * This is the public constructor for a level
         */
        public Level(int[] size,int depth, Room[,] rooms, int[,] roomArray, int[,] levelRegionArray, int[,] weightArray)
        {
            this.Size = size;
            this.Depth = depth;
            this.Rooms = rooms;
            this.RoomArray = roomArray;
            this.LevelRegionArray = levelRegionArray;
            this.WeightArray = weightArray;
        }

        /*
         * This method will print the level to the console
         */
        public void printLevel()
        {
            PrintArrays.printRoomArray(this.RoomArray, "Room Array", null);
            PrintArrays.printRegionArray(this.LevelRegionArray, "Region Array", null);
            PrintArrays.printRoomWeightArray(this.WeightArray, "Room Weight Array", null);
            PrintArrays.printExitsArray(this.Rooms, "Room Exits Array", null);
        }




    }
}
